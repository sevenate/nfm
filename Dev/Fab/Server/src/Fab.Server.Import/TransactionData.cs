// <copyright file="TransactionData.cs" company="HD">
// 	Copyright (c) 2009-2010 nReez. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="78@nreez.com" date="2010-07-02" />
// <summary>Data object for imported transaction.</summary>

using System;

namespace Fab.Server.Import
{
	/// <summary>
	/// Data object for imported transaction.
	/// </summary>
	public class TransactionData
	{
		public DateTime Date { get; set; }
		public int CategoryId { get; set; }
		public string Comment { get; set; }
		public decimal Price { get; set; }
		public decimal Quantity { get; set; }
		public bool IsWithdrawal { get; set; }
	}
}