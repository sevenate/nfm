// <copyright file="AdminService.svc.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="alevshoff@hd.com" date="2010-02-04" />
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
		public IList<UserDTO> GetAll()
		{
			using (var mc = new ModelContainer())
			{
				var users = mc.Users.OrderBy(u => u.Registered)
									.ToList()
									.Select(user => ObjectMapperManager.DefaultInstance.GetMapper<User, UserDTO>().Map(user))
									.ToList();
				return users;
			}
		}

		/// <summary>
		/// Disable login for specific user by his internal unique ID.
		/// </summary>
		/// <param name="userId">User ID to disable.</param>
		public void Disable(Guid userId)
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