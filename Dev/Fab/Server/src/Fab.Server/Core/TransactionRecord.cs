// <copyright file="TransactionRecord.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-02-23</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-02-23</date>
// </editor>
// <summary>Simple "income / expense / balance" data object.</summary>

using System;
using System.Runtime.Serialization;

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
		public Category Category { get; set; }

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
		/// Gets or sets quantity of the expense or income cause.
		/// </summary>
		[DataMember]
		public decimal Quantity { get; set; }
		
		/// <summary>
		/// Gets or sets price of the expense or income cause.
		/// </summary>
		[DataMember]
		public decimal Price { get; set; }

		/// <summary>
		/// Gets or sets transaction optional comment.
		/// </summary>
		[DataMember]
		public string Comment { get; set; }
	}
}