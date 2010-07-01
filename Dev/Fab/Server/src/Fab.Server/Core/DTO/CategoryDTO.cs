// <copyright file="CategoryDTO.cs" company="HD">
// 	Copyright (c) 2009-2010 nReez. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="78@nreez.com" date="2010-06-28" />
// <summary>Category data transfer object.</summary>

using System.Runtime.Serialization;

namespace Fab.Server.Core.DTO
{
	/// <summary>
	/// Category data transfer object.
	/// </summary>
	[DataContract]
	public class CategoryDTO
	{
		/// <summary>
		/// Gets or sets category unique ID.
		/// </summary>
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets category name.
		/// </summary>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets category name.
		/// </summary>
		[DataMember]
		public byte CategoryType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a category is deleted.
		/// </summary>
		[DataMember]
		public bool IsDeleted { get; set; }
	}
}