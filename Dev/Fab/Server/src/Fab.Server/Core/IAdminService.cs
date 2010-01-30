// <copyright file="IAdminService.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-01-28</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-01-28</date>
// </editor>
// <summary>Administrative service.</summary>

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Fab.Server.Core
{
    /// <summary>
    /// Administrative service.
    /// </summary>
    [ServiceContract]
    public interface IAdminService
    {
        /// <summary>
        /// Retrieve all registered users from the system.
        /// </summary>
        /// <returns>All users.</returns>
        [OperationContract]
        IEnumerable<User> GetAll();

        /// <summary>
        /// Disable login for specific user by his internal unique ID.
        /// </summary>
        /// <param name="userId">User ID to disable.</param>
        [OperationContract]
        void Disable(Guid userId);
    }
}