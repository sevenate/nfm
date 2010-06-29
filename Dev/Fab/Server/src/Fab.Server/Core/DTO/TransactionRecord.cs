// <copyright file="TransactionRecord.cs" company="HD">
// 	Copyright (c) 2009-2010 nReez. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="78@nreez.com" date="2010-02-23" />
// <summary>Simple "income / expense / balance" data object.</summary>

using System;
using System.Runtime.Serialization;
using Fab.Server.Core.DTO;

namespace Fab.Server.Core
{
	/// <summary>
	/// Simple "income / expense / balance" data object.
	/// </summary>
	[DataContract]
	public class TransactionRecord
	{
		/// <summary>
		/// Gets or sets unique (for account) transaction ID.
		/// </summary>
		[DataMember]
		public int TransactionId { get; set; }

		/// <summary>
		/// Gets or sets operation date.
		/// </summary>
		[DataMember]
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets transaction category.
		/// </summary>
		[DataMember]
		public CategoryDTO Category { get; set; }

		/// <summary>
		/// Gets or sets income part of the transaction record.
		/// </summary>
		[DataMember]
		public decimal Income { get; set; }

		/// <summary>
		/// Gets or sets expense part of the transaction record.
		/// </summary>
		[DataMember]
		public decimal Expense { get; set; }

		/// <summary>
		/// Gets or sets balance part of the transaction record.
		/// </summary>
		[DataMember]
		public decimal Balance { get; set; }

		/// <summary>
		/// Gets or sets transaction optional comment.
		/// </summary>
		[DataMember]
		public string Comment { get; set; }
	}
}