// <copyright file="FileSizeUtility.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-28</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-28</date>
// </editor>
// <summary>Set of helper dictionaries.</summary>

using System.Collections.Generic;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Set of helper dictionaries.
	/// </summary>
	public static class FileSizeUtility
	{
		#region Size unit constants

		/// <summary>
		/// Byte size measuring description.
		/// </summary>
		public const string BYTE = "B";

		/// <summary>
		/// Giga-byte size measuring description.
		/// </summary>
		public const string GBYTE = "G";

		/// <summary>
		/// Kilo-byte size measuring description.
		/// </summary>
		public const string KBYTE = "K";

		/// <summary>
		/// Mega-byte size measuring description.
		/// </summary>
		public const string MBYTE = "M";

		/// <summary>
		/// Tera-byte size measuring description.
		/// </summary>
		public const string TBYTE = "T";

		#endregion

		#region Helper Dictionaries

		/// <summary>
		/// File size extreme values multipliers: "byte", "kilo-byte", "mega-byte", "giga-byte" and "tera-byte".
		/// </summary>
		public static Dictionary<FileSizeUnit, long> Size = new Dictionary<FileSizeUnit, long>
		{
			{
				FileSizeUnit.Byte, 1L
				},
			{
				FileSizeUnit.KiloByte, 1024L
				},
			{
				FileSizeUnit.MegaByte, 1024*1024L
				},
			{
				FileSizeUnit.GigaByte, 1024*1024*1024L
				},
			{
				FileSizeUnit.TeraByte, 1024*1024*1024*1024L
				}
		};

		/// <summary>
		/// File size predefined string templates for "x", "x.x", "x.xx" and "x.xxx" formats.
		/// </summary>
		public static Dictionary<FileSizePrecision, string> Format = new Dictionary<FileSizePrecision, string>
		{
			{
				FileSizePrecision.ZeroDecimalPlaces, "{0:N0}"
				},
			{
				FileSizePrecision.OneDecimalPlaces, "{0:N1}"
				},
			{
				FileSizePrecision.TwoDecimalPlaces, "{0:N2}"
				},
			{
				FileSizePrecision.ThreeDecimalPlaces, "{0:N3}"
				}
		};

		#endregion
	}
}