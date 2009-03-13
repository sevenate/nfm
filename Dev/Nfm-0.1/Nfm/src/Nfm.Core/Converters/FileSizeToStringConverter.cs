// <copyright file="FileSizeToStringConverter.cs" company="HD">
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
// <summary>Convert long file size value to readable string.</summary>

using System;
using System.Globalization;
using System.Windows.Data;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Convert long file size value to readable string.
	/// </summary>
	[ValueConversion(typeof (long), typeof (string))]
	public class FileSizeToStringConverter : IValueConverter
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets mesearing unit for byte-sized files.
		/// </summary>
		public FileSizeUnit ByteUnit { get; set; }

		/// <summary>
		/// Gets or sets byte-sized file precision.
		/// </summary>
		public FileSizePrecision BytePrecision { get; set; }

		/// <summary>
		/// Gets or sets mesearing unit for kilobyte-sized files.
		/// </summary>
		public FileSizeUnit KByteUnit { get; set; }

		/// <summary>
		/// Gets or sets kilobyte-sized file precision.
		/// </summary>
		public FileSizePrecision KBytePrecision { get; set; }

		/// <summary>
		/// Gets or sets mesearing unit for megabyte-sized files.
		/// </summary>
		public FileSizeUnit MByteUnit { get; set; }

		/// <summary>
		/// Gets or sets megabyte-sized file precision.
		/// </summary>
		public FileSizePrecision MBytePrecision { get; set; }

		/// <summary>
		/// Gets or sets mesearing unit for gigabyte-sized files.
		/// </summary>
		public FileSizeUnit GByteUnit { get; set; }

		/// <summary>
		/// Gets or sets gigabyte-sized file precision.
		/// </summary>
		public FileSizePrecision GBytePrecision { get; set; }

		/// <summary>
		/// Gets or sets mesearing unit for terabyte-sized files.
		/// </summary>
		public FileSizeUnit TByteUnit { get; set; }

		/// <summary>
		/// Gets or sets terabyte-sized file precision.
		/// </summary>
		public FileSizePrecision TBytePrecision { get; set; }

		#endregion

		#region Implementation of IValueConverter

		/// <summary>
		/// Converts a value. 
		/// </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return string.Empty;
			}

			var fileSize = (long) value;

			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.TeraByte])
			{
				return GetStringValue(fileSize, TByteUnit, TBytePrecision);
			}

			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.GigaByte])
			{
				return GetStringValue(fileSize, GByteUnit, GBytePrecision);
			}

			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.MegaByte])
			{
				return GetStringValue(fileSize, MByteUnit, MBytePrecision);
			}

			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.KiloByte])
			{
				return GetStringValue(fileSize, KByteUnit, KBytePrecision);
			}

			return GetStringValue(fileSize, ByteUnit, BytePrecision);
		}

		/// <summary>
		/// Converts a value. 
		/// </summary>
		/// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		#endregion

		/// <summary>
		/// Get corresponding file size string representation value based on specific parameters.
		/// </summary>
		/// <param name="size">File size.</param>
		/// <param name="unit">File size measuring unit.</param>
		/// <param name="precision">Measuring precision.</param>
		/// <returns>String representation of file size.</returns>
		private static string GetStringValue(long size, FileSizeUnit unit, FileSizePrecision precision)
		{
			// Force using "CultureInfo.CurrentCulture" instead of "culture.NumberFormat".
			// Note: "culture.NumberFormat" is the same as "CultureInfo.CurrentUICulture".

			switch (unit)
			{
				case FileSizeUnit.TeraByte:
					return string.Format(
						string.Format("{0} {1}", FileSizeUtility.Format[precision], FileSizeUtility.TBYTE), (decimal) size/FileSizeUtility.Size[FileSizeUnit.TeraByte]);

				case FileSizeUnit.GigaByte:
					return string.Format(
						string.Format("{0} {1}", FileSizeUtility.Format[precision], FileSizeUtility.GBYTE), (decimal)size / FileSizeUtility.Size[FileSizeUnit.GigaByte]);

				case FileSizeUnit.MegaByte:
					return string.Format(
						string.Format("{0} {1}", FileSizeUtility.Format[precision], FileSizeUtility.MBYTE), (decimal)size / FileSizeUtility.Size[FileSizeUnit.MegaByte]);

				case FileSizeUnit.KiloByte:
					return string.Format(
						string.Format("{0} {1}", FileSizeUtility.Format[precision], FileSizeUtility.KBYTE), (decimal) size/FileSizeUtility.Size[FileSizeUnit.KiloByte]);

				default: // FileSizeUnit.Byte
					return string.Format(string.Format("{0} {1}", FileSizeUtility.Format[precision], FileSizeUtility.BYTE), (decimal) size);
			}
		}
	}
}