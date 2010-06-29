// <copyright file="AccountDTO.cs" company="HD">
// 	Copyright (c) 2009-2010 nReez. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="78@nreez.com" date="2010-06-28" />
// <summary>Account data transfer object.</summary>

using System;
using System.Runtime.Serialization;

namespace Fab.Server.Core.DTO
{
	/// <summary>
	/// Account data transfer object.
	/// </summary>
	[DataContract]
	public class AccountDTO
	{
		/// <summary>
		/// Gets or sets account unique ID.
		/// </summary>
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets account name.
		/// </summary>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets account creation date.
		/// </summary>
		[DataMember]
		public DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets account asset type DTO.
		/// </summary>
		[DataMember]
		public AssetTypeDTO AssetType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a account is deleted.
		/// </summary>
		[DataMember]
		public bool IsDeleted { get; set; }
	}
}