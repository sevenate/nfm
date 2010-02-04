﻿// <copyright file="AdminService.svc.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-02-04</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-02-04</date>
// </editor>
// <summary>Represent administrative server API.</summary>

using System;
using System.Collections.Generic;
using Fab.Server.Core;

namespace Fab.Server
{
    /// <summary>
    /// Represent administrative server API.
    /// </summary>
    public class AdminService : IAdminService
    {
        #region Implementation of IAdminService

        /// <summary>
        /// Retrieve all registered users from the system.
        /// </summary>
        /// <returns>All users.</returns>
        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disable login for specific user by his internal unique ID.
        /// </summary>
        /// <param name="userId">User ID to disable.</param>
        public void Disable(Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}