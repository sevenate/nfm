// <copyright file="IAdminService.cs" company="HD">
// 	Copyright (c) 2009-2010 nReez. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="78@nreez.com" date="2010-01-28" />
// <summary>Administrative service contract.</summary>

using System;
using System.Collections.Generic;
using System.ServiceModel;
using Fab.Server.Core.DTO;

namespace Fab.Server.Core
{
	/// <summary>
	/// Administrative service contract.
	/// </summary>
	[ServiceContract]
	public interface IAdminService
	{
		/// <summary>
		/// Retrieve all registered users from the system.
		/// </summary>
		/// <returns>All users.</returns>
		[OperationContract]
		IList<UserDTO> GetAllUsers();

		/// <summary>
		/// Disable login for specific user by his internal unique ID.
		/// </summary>
		/// <param name="userId">User ID to disable.</param>
		[OperationContract]
		void DisableUser(Guid userId);
	}
}