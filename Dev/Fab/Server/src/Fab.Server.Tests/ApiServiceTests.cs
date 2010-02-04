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
using System.Linq;
using Fab.Server;
using Xunit;

namespace Fab.Server.Tests
{
    /// <summary>
    /// Unit tests for <see cref="ApiService" />.
    /// </summary>
    public class ApiServiceTests
    {
        #region User Service

        [Fact]
        public void GenerateUniqueUserLogin()
        {
            var service = new ApiService();
            string uniqueLogin = service.GenerateUniqueLogin();

            bool isAvailable = service.IsLoginAvailable(uniqueLogin);

            Assert.True(isAvailable);
        }

        [Fact]
        public void CheckIsLoginAvailable()
        {
            var service = new ApiService();

            bool isAvailable = service.IsLoginAvailable(Guid.NewGuid().ToString());

            Assert.True(isAvailable);
        }

        [Fact]
        public void CheckIsLoginNotAvailable()
        {
            var service = new ApiService();
            service.Register("testUser", "testPassword");

            bool isAvailable = service.IsLoginAvailable("testUser");

            Assert.False(isAvailable);
        }

        [Fact]
        public void RegisterNewUser()
        {
            var service = new ApiService();

            service.Register("testUser", "testPassword");
        }

        [Fact]
        public void UpdateUser()
        {
            var service = new ApiService();
            service.Register("testUser", "testPassword");

            service.Update("testUser", "testPassword", "newTestPassword", "new@email");
        }

        [Fact]
        public void ResetPassword()
        {
            var service = new ApiService();
            service.Register("testUser", "testPassword");
            service.Update("testUser", "testPassword", "newTestPassword", "new@email");

            service.ResetPassword("testUser", "new@email");
        }

        #endregion

        #region Transaction Service

        [Fact]
        public void GetAllAssetTypes()
        {
            var service = new ApiService();

            var assets = service.GetAllAssetTypes();

            Assert.True(assets.Count == 4);
        }

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