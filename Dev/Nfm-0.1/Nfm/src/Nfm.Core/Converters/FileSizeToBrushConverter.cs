// <copyright file="FileSizeToBrushConverter.cs" company="HD">
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
// <summary>Convert long file size value to <see cref="Brush"/>.</summary>

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Nfm.Core.Modules.FileSystem.Configuration;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Convert long file size value to <see cref="Brush"/>.
	/// </summary>
	[ValueConversion(typeof (long), typeof (Brush))]
	public class FileSizeToBrushConverter : IValueConverter
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
				//TODO: Check, if this correct
				return null; // new SolidColorBrush();
			}

			var fileSize = (long) value;

			//Tera bytes
			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.TeraByte])
			{
				return ModuleConfig.TByteBrush;
			}

			//Giga bytes
			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.GigaByte])
			{
				return ModuleConfig.GByteBrush;
			}

			//Mega bytes
			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.MegaByte])
			{
				return ModuleConfig.MByteBrush;
			}

			//Kilo bytes
			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.KiloByte])
			{
				return ModuleConfig.KByteBrush;
			}

			//Bytes
			return ModuleConfig.ByteBrush;
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
		private static FileSizeToBrushConverter instance;

		/// <summary>
		/// Prevents a default instance of the <see cref="FileSizeToBrushConverter"/> class from being created.
		/// </summary>
		private FileSizeToBrushConverter()
		{
		}

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		public static FileSizeToBrushConverter Inst
		{
			[DebuggerStepThrough]
			get
			{
				if (instance == null)
				{
					instance = new FileSizeToBrushConverter();
				}

				return instance;
			}
		}

		#endregion
	}
}