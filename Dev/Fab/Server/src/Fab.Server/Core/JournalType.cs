// <copyright file="JournalType.cs" company="HD">
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
// <summary>Specify journal types.</summary>

using System.Runtime.Serialization;

namespace Fab.Server.Core
{
	/// <summary>
	/// Specify journal records types.
	/// </summary>
	[DataContract]
	public enum JournalType : byte
	{
		/// <summary>
		/// Deposit journal record (id = 1).
		/// </summary>
		[EnumMember]
		Deposit = 1,

		/// <summary>
		/// Withdrawal journal record (id = 2).
		/// </summary>
		[EnumMember]
		Withdrawal = 2,
		
		/// <summary>
		/// Transfer journal record (id = 3).
		/// </summary>
		[EnumMember]
		Transfer = 3
	}
}