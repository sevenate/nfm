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
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using Nfm.Core.Modules.FileSystem.Configuration;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Convert long file size value to readable string.
	/// </summary>
	[ValueConversion(typeof (long), typeof (string))]
	public class FileSizeToStringConverter : IValueConverter
	{
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
				return GetStringValue(fileSize, ModuleConfig.FileSizeBrief.TByteUnit, ModuleConfig.FileSizeBrief.TBytePrecision);
			}

			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.GigaByte])
			{
				return GetStringValue(fileSize, ModuleConfig.FileSizeBrief.GByteUnit, ModuleConfig.FileSizeBrief.GBytePrecision);
			}

			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.MegaByte])
			{
				return GetStringValue(fileSize, ModuleConfig.FileSizeBrief.MByteUnit, ModuleConfig.FileSizeBrief.MBytePrecision);
			}

			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.KiloByte])
			{
				return GetStringValue(fileSize, ModuleConfig.FileSizeBrief.KByteUnit, ModuleConfig.FileSizeBrief.KBytePrecision);
			}

			return GetStringValue(fileSize, ModuleConfig.FileSizeBrief.ByteUnit, ModuleConfig.FileSizeBrief.BytePrecision);
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

		#region Singleton

		/// <summary>
		/// Singleton instance.
		/// </summary>
		private static FileSizeToStringConverter instance;

		/// <summary>
		/// Prevents a default instance of the <see cref="FileSizeToStringConverter"/> class from being created.
		/// </summary>
		private FileSizeToStringConverter()
		{
		}

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		public static FileSizeToStringConverter Inst
		{
			[DebuggerStepThrough]
			get
			{
				if (instance == null)
				{
					instance = new FileSizeToStringConverter();
				}

				return instance;
			}
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
						string.Format("{0} {1}", FileSizeUtility.Format[precision], FileSizeUtility.TBYTE),
						(decimal) size/FileSizeUtility.Size[FileSizeUnit.TeraByte]);

				case FileSizeUnit.GigaByte:
					return string.Format(
						string.Format("{0} {1}", FileSizeUtility.Format[precision], FileSizeUtility.GBYTE),
						(decimal) size/FileSizeUtility.Size[FileSizeUnit.GigaByte]);

				case FileSizeUnit.MegaByte:
					return string.Format(
						string.Format("{0} {1}", FileSizeUtility.Format[precision], FileSizeUtility.MBYTE),
						(decimal) size/FileSizeUtility.Size[FileSizeUnit.MegaByte]);

				case FileSizeUnit.KiloByte:
					return string.Format(
						string.Format("{0} {1}", FileSizeUtility.Format[precision], FileSizeUtility.KBYTE),
						(decimal) size/FileSizeUtility.Size[FileSizeUnit.KiloByte]);

				default: // FileSizeUnit.Byte
					return string.Format(
						string.Format("{0} {1}", FileSizeUtility.Format[precision], FileSizeUtility.BYTE), (decimal) size);
			}
		}
	}
}