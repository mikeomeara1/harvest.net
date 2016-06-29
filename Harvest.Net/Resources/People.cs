using Harvest.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace Harvest.Net
{
    public partial class HarvestRestClient
    {
        // https://github.com/harvesthq/api/blob/master/Sections/People.md
        private const string PeopleResource = "people";
        private const string ResetPasswordAction = "reset_password";

        private IRestRequest ListUsersRequest(DateTime? updatedSince = null)
        {
            var request = Request(PeopleResource);

            if (updatedSince != null)
                request.AddParameter(UpdatedSinceParameter, updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            return request;
        }

        /// <summary>
        /// List all users for the authenticated account. Makes a GET request to the People resource.
        /// </summary>
        /// <param name="updatedSince">An optional filter on the user updated-at property</param>
        public IList<User> ListUsers(DateTime? updatedSince = null)
        {
            return Execute<List<User>>(ListUsersRequest(updatedSince));
        }

        /// <summary>
        /// List all users for the authenticated account. Makes a GET request to the People resource.
        /// </summary>
        /// <param name="updatedSince">An optional filter on the user updated-at property</param>
        public async Task<IList<User>> ListUsersAsync(DateTime? updatedSince = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<List<User>>(ListUsersRequest(updatedSince), cancellationToken);
        }

        /// <summary>
        /// Retrieve a user on the authenticated account. Makes a GET request to the People resource.
        /// </summary>
        /// <param name="userId">The Id of the user to retrieve</param>
        public User User(long userId)
        {
            return Execute<User>(Request($"{PeopleResource}/{userId}"));
        }

        /// <summary>
        /// Retrieve a user on the authenticated account. Makes a GET request to the People resource.
        /// </summary>
        /// <param name="userId">The Id of the user to retrieve</param>
        public async Task<User> UserAsync(long userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<User>(Request($"{PeopleResource}/{userId}"), cancellationToken);
        }

        /// <summary>
        /// Retrieve a user on the authenticated account. Makes a GET request to the People resource.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve</param>
        public User User(string email)
        {
            return Execute<User>(Request($"{PeopleResource}/{email}"));
        }

        /// <summary>
        /// Retrieve a user on the authenticated account. Makes a GET request to the People resource.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve</param>
        public async Task<User> UserAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<User>(Request($"{PeopleResource}/{email}"), cancellationToken);
        }

        public UserOptions CreateUserOptions(string email, string firstName, string lastName,
            bool isActive = true, string timezone = null, bool? isAdmin = null, string telephone = null,
            string department = null, bool? isContractor = null, bool? hasAccessToAllFutureProjects = null,
            bool? wantsNewsletter = null, bool? wantsWeeklyDigest = null, decimal? defaultHourlyRate = null,
            decimal? costRate = null)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));
            if (firstName == null)
                throw new ArgumentNullException(nameof(firstName));
            if (lastName == null)
                throw new ArgumentNullException(nameof(lastName));

            return new UserOptions()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Department = department,
                Telephone = telephone,
                Timezone = timezone,
                IsActive = isActive,
                IsAdmin = isAdmin,
                IsContractor = isContractor,
                HasAccessToAllFutureProjects = hasAccessToAllFutureProjects,
                WantsNewsletter = wantsNewsletter,
                WantsWeeklyDigest = wantsWeeklyDigest,
                DefaultHourlyRate = defaultHourlyRate,
                CostRate = costRate
            };
        }

        /// <summary>
        /// Creates a new user under the authenticated account. Makes both a POST and a GET request to the People resource.
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <param name="firstName">The user's first name</param>
        /// <param name="lastName">The user's last name</param>
        /// <param name="isActive">The status of the user</param>
        /// <param name="timezone">The user's timezone</param>
        /// <param name="isAdmin">Whether the user should be made an admin</param>
        /// <param name="telephone">The user's telephone number</param>
        /// <param name="department">The department the user belongs to</param>
        /// <param name="isContractor">Whether the user should be flagged as a contractor</param>
        /// <param name="hasAccessToAllFutureProjects">Whether the user should be automatically added to future projects created</param>
        /// <param name="wantsNewsletter">Whether the user should receive the newsletter</param>
        /// <param name="wantsWeeklyDigest">Whether the user should receive a weekly digest</param>
        /// <param name="defaultHourlyRate">The user's default hourly rate</param>
        /// <param name="costRate">The user's cost rate</param>
        public User CreateUser(string email, string firstName, string lastName, bool isActive = true,
            string timezone = null, bool? isAdmin = null, string telephone = null, string department = null,
            bool? isContractor = null, bool? hasAccessToAllFutureProjects = null, bool? wantsNewsletter = null,
            bool? wantsWeeklyDigest = null, decimal? defaultHourlyRate = null, decimal? costRate = null)
        {
            return
                CreateUser(CreateUserOptions(email, firstName, lastName, isActive, timezone, isAdmin, telephone,
                    department, isContractor, hasAccessToAllFutureProjects, wantsNewsletter, wantsWeeklyDigest,
                    defaultHourlyRate, costRate));
        }

        /// <summary>
        /// Creates a new user under the authenticated account. Makes both a POST and a GET request to the People resource.
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <param name="firstName">The user's first name</param>
        /// <param name="lastName">The user's last name</param>
        /// <param name="isActive">The status of the user</param>
        /// <param name="timezone">The user's timezone</param>
        /// <param name="isAdmin">Whether the user should be made an admin</param>
        /// <param name="telephone">The user's telephone number</param>
        /// <param name="department">The department the user belongs to</param>
        /// <param name="isContractor">Whether the user should be flagged as a contractor</param>
        /// <param name="hasAccessToAllFutureProjects">Whether the user should be automatically added to future projects created</param>
        /// <param name="wantsNewsletter">Whether the user should receive the newsletter</param>
        /// <param name="wantsWeeklyDigest">Whether the user should receive a weekly digest</param>
        /// <param name="defaultHourlyRate">The user's default hourly rate</param>
        /// <param name="costRate">The user's cost rate</param>
        public async Task<User> CreateUserAsync(string email, string firstName, string lastName, bool isActive = true,
            string timezone = null, bool? isAdmin = null, string telephone = null, string department = null,
            bool? isContractor = null, bool? hasAccessToAllFutureProjects = null, bool? wantsNewsletter = null,
            bool? wantsWeeklyDigest = null, decimal? defaultHourlyRate = null, decimal? costRate = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await
                CreateUserAsync(CreateUserOptions(email, firstName, lastName, isActive, timezone, isAdmin, telephone,
                    department, isContractor, hasAccessToAllFutureProjects, wantsNewsletter, wantsWeeklyDigest,
                    defaultHourlyRate, costRate), cancellationToken);
        }

        private IRestRequest CreateUserRequest(UserOptions options)
        {
            var request = Request(PeopleResource, RestSharp.Method.POST);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Creates a new user under the authenticated account. Makes a POST and a GET request to the People resource.
        /// </summary>
        /// <param name="options">The options for the new user to be created</param>
        public User CreateUser(UserOptions options)
        {
            return Execute<User>(CreateUserRequest(options));
        }

        /// <summary>
        /// Creates a new user under the authenticated account. Makes a POST and a GET request to the People resource.
        /// </summary>
        /// <param name="options">The options for the new user to be created</param>
        public async Task<User> CreateUserAsync(UserOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<User>(CreateUserRequest(options), cancellationToken);
        }

        /// <summary>
        /// Delete a user from the authenticated account. Makes a DELETE request to the People resource.
        /// </summary>
        /// <param name="userId">The ID of the user to delete</param>
        public bool DeleteUser(long userId)
        {
            var result = Execute(Request($"{PeopleResource}/{userId}", RestSharp.Method.DELETE));

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete a user from the authenticated account. Makes a DELETE request to the People resource.
        /// </summary>
        /// <param name="userId">The ID of the user to delete</param>
        public async Task<bool> DeleteUserAsync(long userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteAsync(Request($"{PeopleResource}/{userId}", RestSharp.Method.DELETE), cancellationToken);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Toggle the Active status of a user on the authenticated account. Makes a POST request to the People/Toggle resource and a GET request to the People resource.
        /// </summary>
        /// <param name="userId">The ID of the user to toggle</param>
        public User ToggleUser(long userId)
        {
            Execute(Request($"{PeopleResource}/{userId}/{ToggleAction}", RestSharp.Method.POST));

            return User(userId);
        }

        /// <summary>
        /// Toggle the Active status of a user on the authenticated account. Makes a POST request to the People/Toggle resource and a GET request to the People resource.
        /// </summary>
        /// <param name="userId">The ID of the user to toggle</param>
        public async Task<User> ToggleUserAsync(long userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await ExecuteAsync(Request($"{PeopleResource}/{userId}/{ToggleAction}", RestSharp.Method.POST), cancellationToken);

            return User(userId);
        }

        /// <summary>
        /// Reset the password of a user on the authenticated account. Makes a POST request to the People/Reset_Password resource and a GET request to the People resource.
        /// </summary>
        /// <param name="userId">The ID of the user to reset</param>
        public User ResetPassword(long userId)
        {
            return Execute<User>(Request($"{PeopleResource}/{userId}/{ResetPasswordAction}", RestSharp.Method.POST));
        }

        /// <summary>
        /// Reset the password of a user on the authenticated account. Makes a POST request to the People/Reset_Password resource and a GET request to the People resource.
        /// </summary>
        /// <param name="userId">The ID of the user to reset</param>
        public async Task<User> ResetPasswordAsync(long userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<User>(Request($"{PeopleResource}/{userId}/{ResetPasswordAction}", RestSharp.Method.POST), cancellationToken);
        }

        /// <summary>
        /// Update a user on the authenticated account. Makes a PUT and a GET request to the People resource.
        /// </summary>
        /// <param name="userId">The ID of the user to update</param>
        /// <param name="email">The user's email address</param>
        /// <param name="firstName">The user's first name</param>
        /// <param name="lastName">The user's last name</param>
        /// <param name="isActive">The status of the user</param>
        /// <param name="timezone">The user's timezone</param>
        /// <param name="isAdmin">Whether the user should be made an admin</param>
        /// <param name="telephone">The user's telephone number</param>
        /// <param name="department">The department the user belongs to</param>
        /// <param name="isContractor">Whether the user should be flagged as a contractor</param>
        /// <param name="hasAccessToAllFutureProjects">Whether the user should be automatically added to future projects created</param>
        /// <param name="wantsNewsletter">Whether the user should receive the newsletter</param>
        /// <param name="wantsWeeklyDigest">Whether the user should receive a weekly digest</param>
        /// <param name="defaultHourlyRate">The user's default hourly rate</param>
        /// <param name="costRate">The user's cost rate</param>
        public User UpdateUser(long userId, string email = null, string firstName = null, string lastName = null, string timezone = null, bool? isAdmin = null, string telephone = null, string department = null, bool? isContractor = null, bool? hasAccessToAllFutureProjects = null, bool? wantsNewsletter = null, bool? wantsWeeklyDigest = null, decimal? defaultHourlyRate = null, decimal? costRate = null)
        {
            return UpdateUser(userId, new UserOptions()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Department = department,
                Telephone = telephone,
                Timezone = timezone,
                IsAdmin = isAdmin,
                IsContractor = isContractor,
                HasAccessToAllFutureProjects = hasAccessToAllFutureProjects,
                WantsNewsletter = wantsNewsletter,
                WantsWeeklyDigest = wantsWeeklyDigest,
                DefaultHourlyRate = defaultHourlyRate,
                CostRate = costRate
            });
        }

        /// <summary>
        /// Update a user on the authenticated account. Makes a PUT and a GET request to the People resource.
        /// </summary>
        /// <param name="userId">The ID of the user to update</param>
        /// <param name="email">The user's email address</param>
        /// <param name="firstName">The user's first name</param>
        /// <param name="lastName">The user's last name</param>
        /// <param name="isActive">The status of the user</param>
        /// <param name="timezone">The user's timezone</param>
        /// <param name="isAdmin">Whether the user should be made an admin</param>
        /// <param name="telephone">The user's telephone number</param>
        /// <param name="department">The department the user belongs to</param>
        /// <param name="isContractor">Whether the user should be flagged as a contractor</param>
        /// <param name="hasAccessToAllFutureProjects">Whether the user should be automatically added to future projects created</param>
        /// <param name="wantsNewsletter">Whether the user should receive the newsletter</param>
        /// <param name="wantsWeeklyDigest">Whether the user should receive a weekly digest</param>
        /// <param name="defaultHourlyRate">The user's default hourly rate</param>
        /// <param name="costRate">The user's cost rate</param>
        public async Task<User> UpdateUserAsync(long userId, string email = null, string firstName = null, string lastName = null, string timezone = null, bool? isAdmin = null, string telephone = null, string department = null, bool? isContractor = null, bool? hasAccessToAllFutureProjects = null, bool? wantsNewsletter = null, bool? wantsWeeklyDigest = null, decimal? defaultHourlyRate = null, decimal? costRate = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await UpdateUserAsync(userId, new UserOptions()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Department = department,
                Telephone = telephone,
                Timezone = timezone,
                IsAdmin = isAdmin,
                IsContractor = isContractor,
                HasAccessToAllFutureProjects = hasAccessToAllFutureProjects,
                WantsNewsletter = wantsNewsletter,
                WantsWeeklyDigest = wantsWeeklyDigest,
                DefaultHourlyRate = defaultHourlyRate,
                CostRate = costRate
            }, cancellationToken);
        }

        private IRestRequest UpdateUserRequst(long userId, UserOptions options)
        {
            var request = Request("people/" + userId, RestSharp.Method.PUT);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Updates a user on the authenticated account. Makes a PUT and a GET request to the People resource.
        /// </summary>
        /// <param name="userId">The ID for the user to update</param>
        /// <param name="options">The options to be updated</param>
        public User UpdateUser(long userId, UserOptions options)
        {
            return Execute<User>(UpdateUserRequst(userId, options));
        }

        /// <summary>
        /// Updates a user on the authenticated account. Makes a PUT and a GET request to the People resource.
        /// </summary>
        /// <param name="userId">The ID for the user to update</param>
        /// <param name="options">The options to be updated</param>
        public async Task<User> UpdateUserAsync(long userId, UserOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<User>(UpdateUserRequst(userId, options), cancellationToken);
        }
    }
}
