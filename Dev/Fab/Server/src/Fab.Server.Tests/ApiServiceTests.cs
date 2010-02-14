// <copyright file="ApiServiceTests.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-01-30</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-01-30</date>
// </editor>
// <summary>Unit tests for ApiService.</summary>

using System;
using Xunit;

namespace Fab.Server.Tests
{
    /// <summary>
    /// Unit tests for <see cref="ApiService" />.
    /// </summary>
    public class ApiServiceTests
    {
        #region User Service

		/// <summary>
		/// Test <see cref="ApiService.Register"/> method.
		/// </summary>
    	[Fact]
    	public void RegisterNewUser()
    	{
    		var service = new ApiService();

    		Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");

			Assert.True(userId != Guid.Empty);
    	}

		/// <summary>
		/// Test <see cref="ApiService.IsLoginAvailable"/> method.
		/// </summary>
    	[Fact]
    	public void CheckIsLoginAvailable()
    	{
    		var service = new ApiService();

			bool isAvailable = service.IsLoginAvailable("testUser" + Guid.NewGuid());

    		Assert.True(isAvailable);
    	}

		/// <summary>
		/// Test <see cref="ApiService.IsLoginAvailable"/> method.
		/// </summary>
    	[Fact]
    	public void CheckIsLoginNotAvailable()
    	{
			string login = "testUser" + Guid.NewGuid();
    		var service = new ApiService();
			service.Register(login, "testPassword");

			bool isAvailable = service.IsLoginAvailable(login);

    		Assert.False(isAvailable);
    	}

		/// <summary>
		/// Test <see cref="ApiService.GenerateUniqueLogin"/> method.
		/// </summary>
    	[Fact]
    	public void GenerateUniqueUserLogin()
    	{
    		var service = new ApiService();
    		string uniqueLogin = service.GenerateUniqueLogin();

    		bool isAvailable = service.IsLoginAvailable(uniqueLogin);

    		Assert.True(isAvailable);
    	}

		/// <summary>
		/// Test <see cref="ApiService.Update"/> method.
		/// </summary>
    	[Fact]
        public void UpdateUser()
        {
            var service = new ApiService();
			Guid userId = service.Register("testUser" + Guid.NewGuid(), "testPassword");

			service.Update(userId, "testPassword", "newTestPassword", "new@email");
        }

		/// <summary>
		/// Test <see cref="ApiService.ResetPassword"/> method.
		/// </summary>
        [Fact]
        public void ResetPassword()
        {
			string login = "testUser" + Guid.NewGuid();
            var service = new ApiService();
			Guid userId = service.Register(login, "testPassword");
			service.Update(userId, "testPassword", "newTestPassword", "new@email");

			service.ResetPassword(login, "new@email");
        }

		/// <summary>
		/// Test <see cref="ApiService.GetUserId"/> method.
		/// </summary>
    	[Fact]
    	public void GetUserId()
    	{
			string login = "testUser" + Guid.NewGuid();
			var service = new ApiService();
			Guid userId = service.Register(login, "testPassword");

			Guid actualUserId = service.GetUserId(login);

			Assert.True(userId != Guid.Empty);
			Assert.True(actualUserId != Guid.Empty);
			Assert.True(userId == actualUserId);
    	}

        #endregion

        #region Transaction Service

		/// <summary>
		/// Test <see cref="ApiService.GetAllAssetTypes"/> method.
		/// </summary>
        [Fact]
        public void GetAllAssetTypes()
        {
            var service = new ApiService();

            var assets = service.GetAllAssetTypes();

            Assert.True(assets.Count == 4);
        }

		/// <summary>
		/// Test <see cref="ApiService.GetAllJournalTypes"/> method.
		/// </summary>
        [Fact]
        public void GetAllJournalTypes()
        {
            var service = new ApiService();

            var journals = service.GetAllJournalTypes();

            Assert.True(journals.Count == 3);
        }

        #endregion
    }
}