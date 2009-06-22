// <copyright file="FileSizeFullStringFormat.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-09</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-09</date>
// </editor>
// <summary>Holds all string formats required to show full file size.</summary>

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Holds all string formats required to show full file size.
	/// </summary>
	public class FileSizeFullStringFormat
	{
		#region File Size String Formats

		/// <summary>
		/// Gets or sets mesearing unit for byte-sized files.
		/// </summary>
		public FileSizeUnit ByteUnit { get { return FileSizeUnit.Byte; } }

		/// <summary>
		/// Gets or sets byte-sized file precision.
		/// </summary>
		public FileSizePrecision BytePrecision { get { return FileSizePrecision.ZeroDecimalPlaces; } }

		/// <summary>
		/// Gets or sets mesearing unit for kilobyte-sized files.
		/// </summary>
		public FileSizeUnit KByteUnit { get { return FileSizeUnit.Byte; } }

		/// <summary>
		/// Gets or sets kilobyte-sized file precision.
		/// </summary>
		public FileSizePrecision KBytePrecision { get { return FileSizePrecision.ZeroDecimalPlaces; } }

		/// <summary>
		/// Gets or sets mesearing unit for megabyte-sized files.
		/// </summary>
		public FileSizeUnit MByteUnit { get { return FileSizeUnit.Byte; } }

		/// <summary>
		/// Gets or sets megabyte-sized file precision.
		/// </summary>
		public FileSizePrecision MBytePrecision { get { return FileSizePrecision.ZeroDecimalPlaces; } }

		/// <summary>
		/// Gets or sets mesearing unit for gigabyte-sized files.
		/// </summary>
		public FileSizeUnit GByteUnit { get { return FileSizeUnit.Byte; } }

		/// <summary>
		/// Gets or sets gigabyte-sized file precision.
		/// </summary>
		public FileSizePrecision GBytePrecision { get { return FileSizePrecision.ZeroDecimalPlaces; } }

		/// <summary>
		/// Gets or sets mesearing unit for terabyte-sized files.
		/// </summary>
		public FileSizeUnit TByteUnit { get { return FileSizeUnit.Byte; } }

		/// <summary>
		/// Gets or sets terabyte-sized file precision.
		/// </summary>
		public FileSizePrecision TBytePrecision { get { return FileSizePrecision.ZeroDecimalPlaces; } }

		#endregion
	}
}