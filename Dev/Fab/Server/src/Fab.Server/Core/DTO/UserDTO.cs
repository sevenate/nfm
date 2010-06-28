// <copyright file="UserDTO.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="alevshoff@hd.com" date="2010-06-28" />
// <summary>User data transfer object.</summary>

using System;
using System.Runtime.Serialization;

namespace Fab.Server.Core.DTO
{
	/// <summary>
	/// User data transfer object.
	/// </summary>
	[DataContract]
	public class UserDTO
	{
		/// <summary>
		/// Gets or sets user unique ID.
		/// </summary>
		[DataMember]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets user unique login name.
		/// </summary>
		[DataMember]
		public string Login { get; set; }

		/// <summary>
		/// Gets or sets user password.
		/// </summary>
		[DataMember]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets user unique email.
		/// </summary>
		[DataMember]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets user registration date.
		/// </summary>
		[DataMember]
		public DateTime Registered { get; set; }

		/// <summary>
		/// Gets or sets user last access date.
		/// </summary>
		[DataMember]
		public DateTime? LastAccess { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a user is disabled.
		/// </summary>
		[DataMember]
		public bool IsDisabled { get; set; }
	}
}