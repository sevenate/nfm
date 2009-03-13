// <copyright file="BoolToVisibilityConverter.cs" company="HD">
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
// <summary>Convert boolean value to visibility.</summary>

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Convert boolean value to visibility.
	/// </summary>
	[ValueConversion(typeof (bool), typeof (Visibility))]
	public class BoolToVisibilityConverter : IValueConverter
	{
		/// <summary>
		/// Gets or sets a value indicating whether a "invisible" means Visibility.Hidden
		/// (with space reservation), instead of regulary Visibility.Collapsed.
		/// </summary>
		public bool HiddenOnly { get; set; }

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
			return value != null && (bool) value
							? Visibility.Visible
							: HiddenOnly
								? Visibility.Hidden
								: Visibility.Collapsed;
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