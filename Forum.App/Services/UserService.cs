using Forum.Data;
using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Forum.App.Controllers.SignUpController;

namespace Forum.App.Services
{
    public static class UserService
    {
        /// <summary>
        /// Validate the username and password and check if it exists in the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool TryLogInUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            var forumData = new ForumData();

            bool userExists = forumData.Users.Any(u => u.Username == username && u.Password == password);

            return userExists;
        }

        public static SignUpStatus TrySignUpUser(string username, string password)
        {
            var validUsername = !string.IsNullOrWhiteSpace(username) && username.Length > 3;
            var validPassword = !string.IsNullOrWhiteSpace(password) && password.Length > 3;

            if (!validUsername || !validPassword)
                return SignUpStatus.DetailsError;

            var forumData = new ForumData();

            bool userAlreadyExists = forumData.Users.Any(u => u.Username == username && u.Password == password);

            if (!userAlreadyExists)
            {
                //Look for the last entry and if there is no such return null. if the entry is null than make the first id be equal to 1
                var userId = forumData.Users.LastOrDefault()?.Id + 1 ?? 1;
                var user = new User(userId, username, password);
                forumData.Users.Add(user);
                forumData.SaveChanges();

                return SignUpStatus.Success;
            }

            return SignUpStatus.UsernameTakenError;
        }
    }
}
