// <copyright file="FileSizeUnit.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-23</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-23</date>
// </editor>
// <summary>Specify file size measuring unit.</summary>

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Specify file size measuring unit.
	/// </summary>
	public enum FileSizeUnit
	{
		/// <summary>
		/// Byte size.
		/// </summary>
		/// <remarks>This is default value.</remarks>
		Byte = 0,

		/// <summary>
		/// Kilo-byte size.
		/// </summary>
		KiloByte,

		/// <summary>
		/// Mega-byte size.
		/// </summary>
		MegaByte,

		/// <summary>
		/// Giga-byte size.
		/// </summary>
		GigaByte,

		/// <summary>
		/// Tera-byte size.
		/// </summary>
		TeraByte
	}
}