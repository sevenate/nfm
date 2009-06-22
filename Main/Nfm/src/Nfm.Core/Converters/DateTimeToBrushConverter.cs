// <copyright file="DateTimeToBrushConverter.cs" company="HD">
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
// <summary>Convert nullable <see cref="DateTime"/> value to <see cref="Brush"/>.</summary>

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Convert nullable <see cref="DateTime"/> value to <see cref="Brush"/>.
	/// </summary>
	[ValueConversion(typeof (DateTime?), typeof (Brush))]
	public class DateTimeToBrushConverter : IValueConverter
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
			var date = (DateTime?) value;

			if (!date.HasValue)
			{
				//TODO: Check, if this correct
				return null; // new SolidColorBrush();
			}

			TimeSpan diff = DateTime.Now - date.Value;

			//Today
			if (diff < new TimeSpan(1, 0, 0))
			{
				return new SolidColorBrush(Colors.LightGoldenrodYellow);
			}

			//Yesterday
			if (new TimeSpan(1, 0, 0, 0) <= diff && diff < new TimeSpan(2, 0, 0, 0))
			{
				return new SolidColorBrush(Colors.LightGray);
			}

			//2-7 Days ago
			if (new TimeSpan(2, 0, 0, 0) <= diff && diff < new TimeSpan(8, 0, 0, 0))
			{
				return new SolidColorBrush(Colors.DarkGray);
			}

			return new SolidColorBrush(Colors.Gray);
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
		private static DateTimeToBrushConverter instance;

		/// <summary>
		/// Prevents a default instance of the <see cref="DateTimeToBrushConverter"/> class from being created.
		/// </summary>
		private DateTimeToBrushConverter()
		{
		}

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		public static DateTimeToBrushConverter Inst
		{
			[DebuggerStepThrough]
			get
			{
				if (instance == null)
				{
					instance = new DateTimeToBrushConverter();
				}

				return instance;
			}
		}

		#endregion
	}
}