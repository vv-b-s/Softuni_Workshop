using Forum.App.Contracts;
using Forum.Data;
using Forum.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forum.App.Services
{
    public class UserService : IUserService
    {
        private ForumData forumData;
        private ISession session;

        public UserService(ForumData forumData, ISession session)
        {
            this.forumData = forumData;
            this.session = session;
        }

        public User GetUserById(int userId)
        {
            return this.forumData.Users.FirstOrDefault(u => u.Id == userId);
        }

        public string GetUserName(int userId)
        {
            var user = GetUserById(userId);
            return user?.Username;
        }

        public bool TryLogInUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            var user = this.forumData.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user is null)
                return false;

            session.Reset();
            session.LogIn(user);

            return true;
        }

        public bool TrySignUpUser(string username, string password)
        {
            var validUsername = !string.IsNullOrWhiteSpace(username) && username.Length > 3;
            var validPassword = !string.IsNullOrWhiteSpace(password) && username.Length > 3;

            if (!validPassword || !validUsername)
                throw new ArgumentException("Username and Password must be longer than 3 symbols!");

            var userExists = this.forumData.Users.FirstOrDefault(u => u.Username == username) != null;

            if (userExists)
                throw new InvalidOperationException("Username taken!");

            var userId = this.forumData.Users.LastOrDefault()?.Id + 1 ?? 1;
            var user = new User(userId, username, password);

            this.forumData.Users.Add(user);
            this.forumData.SaveChanges();

            this.TryLogInUser(username, password);
            return true;
        }
    }
}
