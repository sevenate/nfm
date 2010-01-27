// <copyright file="IUserService.cs" company="HD">
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
// <summary>User service.</summary>


using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Fab.Server.Core
{
    /// <summary>
    /// User service.
    /// </summary>
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        string GenerateUniqueLogin();

        [OperationContract]
        void Register(string login, string password);

        [OperationContract]
        void Update(string password, string email);

        [OperationContract]
        void ResetPassword(string login);
    }
}