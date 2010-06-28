// <copyright file="AdminService.svc.cs" company="HD">
// 	Copyright (c) 2009-2010 nReez. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="78@nreez.com" date="2010-02-04" />
// <summary>Administrative service.</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using EmitMapper;
using Fab.Server.Core;
using Fab.Server.Core.DTO;

namespace Fab.Server
{
	/// <summary>
	/// Administrative service.
	/// </summary>
	public class AdminService : IAdminService
	{
		#region Implementation of IAdminService

		/// <summary>
		/// Retrieve all registered users from the system.
		/// </summary>
		/// <returns>All users.</returns>
		public IList<UserDTO> GetAllUsers()
		{
			using (var mc = new ModelContainer())
			{
				return mc.Users.OrderBy(u => u.Registered)
									.ToList()
									.Select(user => ObjectMapperManager.DefaultInstance.GetMapper<User, UserDTO>().Map(user))
									.ToList();
			}
		}

		/// <summary>
		/// Disable login for specific user by his internal unique ID.
		/// </summary>
		/// <param name="userId">User ID to disable.</param>
		public void DisableUser(Guid userId)
		{
			using (var mc = new ModelContainer())
			{
				var user = ModelHelper.GetUserById(mc, userId);
				user.IsDisabled = true;
				mc.SaveChanges();
			}
		}

		#endregion
	}
}