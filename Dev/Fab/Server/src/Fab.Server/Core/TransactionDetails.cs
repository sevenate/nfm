// <copyright file="TransactionDetails.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-06-11</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-06-11</date>
// </editor>
// <summary>Data transfer object that holds additional transaction information.</summary>

using System.Runtime.Serialization;

namespace Fab.Server.Core
{
	/// <summary>
	/// Data transfer object that holds additional transaction information.
	/// </summary>
	[DataContract]
	public class TransactionDetails
	{
		/// <summary>
		/// Gets or sets unique (for account) transaction ID.
		/// </summary>
		[DataMember]
		public int TransactionId { get; set; }

		/// <summary>
		/// Gets or sets transaction type.
		/// </summary>
		[DataMember]
		public JournalType JournalType { get; set; }

		/// <summary>
		/// Gets or sets transaction price.
		/// </summary>
		[DataMember]
		public decimal Price { get; set; }

		/// <summary>
		/// Gets or sets transaction quantity.
		/// </summary>
		[DataMember]
		public decimal Quantity { get; set; }
	}
}