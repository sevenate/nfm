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
            Assert.True(false);
        }

        [Fact]
        public void CheckIsUserLoginUnique()
        {
            Assert.True(false);
        }

        [Fact]
        public void RegisterNewUser()
        {
            Assert.True(false);
        }

        [Fact]
        public void UpdateUser()
        {
            Assert.True(false);
        }

        [Fact]
        public void ResetUserPassword()
        {
            Assert.True(false);
        }

        #endregion

        #region Admin Service

        [Fact]
        public void GetAllUsers()
        {
            Assert.True(false);
        }

        [Fact]
        public void DisableUser()
        {
            Assert.True(false);
        }

        #endregion

        #region Transaction Service

        [Fact]
        public void GetAllAssetTypes()
        {
            ApiService service = new ApiService();

            var assets = service.GetAllAssetTypes();

            Assert.True(assets.Count == 4);
        }

        [Fact]
        public void GetAllJournalTypes()
        {
            ApiService service = new ApiService();

            var journals = service.GetAllJournalTypes();

            Assert.True(journals.Count == 3);
        }

        #endregion
    }
}