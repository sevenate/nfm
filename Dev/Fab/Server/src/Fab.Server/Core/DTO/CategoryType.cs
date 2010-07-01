// <copyright file="CategoryType.cs" company="HD">
// 	Copyright (c) 2009-2010 nReez. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="78@nreez.com" date="2010-07-01" />
// <summary>Specify category types.</summary>

using System.Runtime.Serialization;

namespace Fab.Server.Core.DTO
{
	/// <summary>
	/// Specify category types.
	/// </summary>
	[DataContract]
	public enum CategoryType
	{
		/// <summary>
		/// Category for withdrawal (id = 1).
		/// </summary>
		[EnumMember]
		Withdrawal = 1,

		/// <summary>
		/// Category for deposit (id = 2).
		/// </summary>
		[EnumMember]
		Deposit = 2
	}
}