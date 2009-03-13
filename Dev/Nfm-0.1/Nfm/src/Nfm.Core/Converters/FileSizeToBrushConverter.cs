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
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Convert long file size value to <see cref="Brush"/>.
	/// </summary>
	[ValueConversion(typeof(long), typeof(Brush))]
	public class FileSizeToBrushConverter : IValueConverter
	{
		#region Public Properties

		/// <summary>
		/// Gets or sets brush to draw byte size.
		/// </summary>
		public Brush ByteBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw kilo-byte size.
		/// </summary>
		public Brush KByteBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw mega-byte size.
		/// </summary>
		public Brush MByteBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw giga-byte size.
		/// </summary>
		public Brush GByteBrush { get; set; }

		/// <summary>
		/// Gets or sets brush to draw tera-byte size.
		/// </summary>
		public Brush TByteBrush { get; set; }

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
				//TODO: Check, if this correct
				return null;	// new SolidColorBrush();
			}

			var fileSize = (long) value;

			//Tera bytes
			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.TeraByte])
			{
				return TByteBrush;
			}

			//Giga bytes
			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.GigaByte])
			{
				return GByteBrush;
			}

			//Mega bytes
			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.MegaByte])
			{
				return MByteBrush;
			}

			//Kilo bytes
			if (fileSize >= FileSizeUtility.Size[FileSizeUnit.KiloByte])
			{
				return KByteBrush;
			}

			//Bytes
			return ByteBrush;
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
	}
}