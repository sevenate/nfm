// <copyright file="AssetTypeDTO.cs" company="HD">
// 	Copyright (c) 2009-2010 nReez. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="78@nreez.com" date="2010-06-28" />
// <summary>Asset type data transfer object.</summary>

using System.Runtime.Serialization;

namespace Fab.Server.Core.DTO
{
	/// <summary>
	/// Asset type data transfer object.
	/// </summary>
	[DataContract]
	public class AssetTypeDTO
	{
		/// <summary>
		/// Gets or sets asset type unique ID.
		/// </summary>
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets asset type name.
		/// </summary>
		[DataMember]
		public string Name { get; set; }
	}
}