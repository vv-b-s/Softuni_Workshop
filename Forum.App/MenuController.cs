﻿namespace Forum.App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Forum.App.Controllers;
    using Forum.App.Controllers.Contracts;
    using Forum.App.Services;
    using Forum.App.UserInterface;
    using Forum.App.UserInterface.Contracts;

    internal class MenuController
    {
        private const int DEFAULT_INDEX = 0;

        private IController[] controllers;
        private Stack<int> controllerHistory;
        private int currentOptionIndex;
        private ForumViewEngine forumViewer;

        public MenuController(IEnumerable<IController> controllers, ForumViewEngine forumViewer)
        {
            this.controllers = controllers.ToArray();
            this.forumViewer = forumViewer;

            InitializeControllerHistory();

            this.currentOptionIndex = DEFAULT_INDEX;
        }

        private string Username { get; set; }
        private IView CurrentView { get; set; }

        private MenuState State => (MenuState)controllerHistory.Peek();
        private int CurrentControllerIndex => this.controllerHistory.Peek();
        private IController CurrentController => this.controllers[this.controllerHistory.Peek()];
        internal ILabel CurrentLabel => this.CurrentView.Buttons[currentOptionIndex];

        private void InitializeControllerHistory()
        {
            if (controllerHistory != null)
            {
                throw new InvalidOperationException($"{nameof(controllerHistory)} already initialized!");
            }
            int mainControllerIndex = 0;
            this.controllerHistory = new Stack<int>();
            this.controllerHistory.Push(mainControllerIndex);
            this.RenderCurrentView();
        }

        internal void PreviousOption()
        {
            this.currentOptionIndex--;

            if (this.currentOptionIndex < 0)
            {
                this.currentOptionIndex += this.CurrentView.Buttons.Length;
            }

            if (this.CurrentLabel.IsHidden)
            {
                this.PreviousOption();
            }
        }

        internal void NextOption()
        {
            this.currentOptionIndex++;

            int totalOptions = this.CurrentView.Buttons.Length;

            if (this.currentOptionIndex >= totalOptions)
            {
                this.currentOptionIndex -= totalOptions;
            }

            if (this.CurrentLabel.IsHidden)
            {
                this.NextOption();
            }
        }

        internal void Back()
        {
            if (this.State == MenuState.Categories || this.State == MenuState.OpenCategory)
            {
                IPaginationController currentController = (IPaginationController)this.CurrentController;
                currentController.CurrentPage = 0;
            }

            if (controllerHistory.Count > 1)
            {
                controllerHistory.Pop();
                this.currentOptionIndex = DEFAULT_INDEX;
            }
            RenderCurrentView();
        }

        internal void ExecuteCommand()
        {
            MenuState newState = this.CurrentController.ExecuteCommand(currentOptionIndex);
            switch (newState)
            {
                case MenuState.PostAdded:
                    AddPost();
                    break;

                case MenuState.OpenCategory:
                    OpenCategory();
                    break;

                case MenuState.ViewPost:
                    ViewPost();
                    break;

                case MenuState.SuccessfulLogIn:
                    SuccessfulLogin();
                    break;

                case MenuState.LoggedOut:
                    LogOut();
                    break;

                case MenuState.Back:
                    this.Back();
                    break;

                case MenuState.Error:

                case MenuState.Rerender:
                    RenderCurrentView();
                    break;

                case MenuState.AddReplyToPost:
                    RedirectToAddReply();
                    break;

                case MenuState.ReplyAdded:
                    AddReply();
                    break;

                default:
                    this.RedirectToMenu(newState);
                    break;
            }
        }

        private void AddReply()
        {
            var addReplyController = this.CurrentController as AddReplyController;

            var postId = addReplyController.Post.PostId;

            var postViewer = this.controllers[(int)MenuState.ViewPost] as PostDetailsController;
            postViewer.SetPostId(postId);

            addReplyController.ResetReply();

            //Popping the stack twice, to get back to the ViewPost page. Note that it would work without popping but it will cause problems on that page when pressing 'back'
            this.controllerHistory.Pop();
            this.controllerHistory.Pop();

            this.RedirectToMenu(MenuState.ViewPost);
        }

        private void RedirectToAddReply()
        {
            //Get the post id from the current controller
            var postDetailsController = this.CurrentController as PostDetailsController;
            var postId = postDetailsController.PostId;

            //Send the post Id to the reply controller
            var replyController = this.controllers[(int)MenuState.AddReply] as AddReplyController;
            replyController.SetPost(postId);

            //Redirect to the reply form vire
            this.RedirectToMenu(MenuState.AddReply);
        }

        private void LogOut()
        {
            this.Username = string.Empty;
            this.LogOutUser();
            this.RenderCurrentView();
        }

        private void SuccessfulLogin()
        {
            var loginController = (IReadUserInfoController)this.CurrentController;
            this.Username = loginController.Username;
            this.LogInUser();
            RedirectToMenu(MenuState.Main);
        }

        private void ViewPost()
        {
            var categoryController = this.CurrentController as CategoryController;

            var categoryId = categoryController.CategoryId;

            var posts = PostService.GetPostsByCategory(categoryId).ToArray();

            var postIndex = categoryController.CurrentPage * CategoriesController.PAGE_OFFSET + this.currentOptionIndex;
            var postId = posts[postIndex - 1].Id;

            var postController = this.controllers[(int)MenuState.ViewPost] as PostDetailsController;
            postController.SetPostId(postId);

            this.RedirectToMenu(MenuState.ViewPost);
        }

        private void OpenCategory()
        {
            var categoriesController = this.CurrentController as CategoriesController;

            int categoryIndex = categoriesController.CurrentPage * CategoriesController.PAGE_OFFSET + this.currentOptionIndex;

            var categoryCtrl = this.controllers[(int)MenuState.OpenCategory] as CategoryController;
            categoryCtrl.SetCategory(categoryIndex);

            this.RedirectToMenu(MenuState.OpenCategory);
        }

        private void AddPost()
        {
            var addPostController = this.CurrentController as AddPostController;

            var postId = addPostController.Post.PostId;

            var postViewer = this.controllers[(int)MenuState.ViewPost] as PostDetailsController;
            postViewer.SetPostId(postId);

            addPostController.ResetPost();

            this.controllerHistory.Pop();

            this.RedirectToMenu(MenuState.ViewPost);
        }

        private void RenderCurrentView()
        {
            this.CurrentView = this.CurrentController.GetView(this.Username);
            this.currentOptionIndex = DEFAULT_INDEX;
            this.forumViewer.RenderView(this.CurrentView);
        }

        private bool RedirectToMenu(MenuState newState)
        {
            if (this.State != newState)
            {
                this.controllerHistory.Push((int)newState);
                this.RenderCurrentView();
                return true;
            }

            else return false;
        }

        private void LogInUser()
        {
            foreach(var controller in this.controllers)
            {
                if (controller is IUserRestrictedController userRestrictedController)
                    userRestrictedController.UserLogIn();
            }
        }

        private void LogOutUser()
        {
            foreach (var controller in this.controllers)
            {
                if (controller is IUserRestrictedController userRestrictedController)
                    userRestrictedController.UserLogOut();
            }
        }
    }
}