// <copyright file="TransactionDTO.cs" company="HD">
// 	Copyright (c) 2009-2010 nReez. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="78@nreez.com" date="2010-06-29" />
// <summary>Transaction data transfer object.</summary>

using System.Runtime.Serialization;

namespace Fab.Server.Core.DTO
{
	/// <summary>
	/// Transaction data transfer object.
	/// </summary>
	[DataContract]
	public class TransactionDTO
	{
		/// <summary>
		/// Gets or sets transaction ID.
		/// </summary>
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets journal type.
		/// </summary>
		[DataMember]
		public byte JournalType { get; set; }
		
		/// <summary>
		/// Gets or sets quantity.
		/// </summary>
		[DataMember]
		public decimal Quantity { get; set; }

		/// <summary>
		/// Gets or sets price.
		/// </summary>
		[DataMember]
		public decimal Price { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a transaction is deleted.
		/// </summary>
		[DataMember]
		public bool IsDeleted { get; set; }

		/// <summary>
		/// Gets or sets comment.
		/// </summary>
		[DataMember]
		public string Comment { get; set; }
	}
}