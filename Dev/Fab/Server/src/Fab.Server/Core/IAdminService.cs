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
    public interface IAdminService
    {
        [OperationContract]
        IEnumerable<User> GetAll();

        [OperationContract]
        void Disable(Guid userId);
    }
}