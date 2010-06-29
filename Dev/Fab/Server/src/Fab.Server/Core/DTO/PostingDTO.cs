// <copyright file="PostingDTO.cs" company="HD">
// 	Copyright (c) 2009-2010 nReez. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="78@nreez.com" date="2010-06-29" />
// <summary>Posting data transfer object.</summary>

using System;
using System.Runtime.Serialization;

namespace Fab.Server.Core.DTO
{
	/// <summary>
	/// Posting data transfer object.
	/// </summary>
	[DataContract]
	public class PostingDTO
	{
		/// <summary>
		/// Gets or sets posting ID.
		/// </summary>
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets posting date.
		/// </summary>
		[DataMember]
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets amount.
		/// </summary>
		[DataMember]
		public decimal Amount { get; set; }
	}
}