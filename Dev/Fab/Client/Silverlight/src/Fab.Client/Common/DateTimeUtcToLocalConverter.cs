// <copyright file="DateTimeUtcToLocalConverter.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-06-05</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-06-05</date>
// </editor>
// <summary>Convert <see cref="DateTime"/> from UTC to local and vise versa.</summary>

using System;
using System.Globalization;
using System.Windows.Data;

namespace Fab.Client.Common
{
	/// <summary>
	/// Convert <see cref="DateTime"/> from UTC to local and vise versa.
	/// This mean that in code all operations with any <see cref="DateTime"/> must be processed in UTC format
	/// and converted to local format only when this date need to be visualized for user.
	/// </summary>
	public class DateTimeUtcToLocalConverter : IValueConverter
	{
		#region Implementation of IValueConverter

		/// <summary>
		/// Modifies the source data before passing it to the target for display in the UI.
		/// </summary>
		/// <returns>The value to be passed to the target dependency property.</returns>
		/// <param name="value">The source data being passed to the target.</param>
		/// <param name="targetType">The <see cref="Type"/> of data expected by the target dependency property.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="culture">The culture of the conversion.</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is DateTime && ((DateTime) value).Kind == DateTimeKind.Utc
			       	? ((DateTime) value).ToLocalTime()
			       	: value;
		}

		/// <summary>
		/// Modifies the target data before passing it to the source object.
		/// This method is called only in <see cref="BindingMode.TwoWay"/> bindings.
		/// </summary>
		/// <returns>The value to be passed to the source object.</returns>
		/// <param name="value">The target data being passed to the source.</param>
		/// <param name="targetType">The <see cref="Type"/> of data expected by the source object.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="culture">The culture of the conversion.</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// DatePicker control from Silverlight Toolkit always reset Kind to DateTimeKind.Unspecified
			// So this is the reason why the second part of condition is commented.

			return value is DateTime //&& ((DateTime)value).Kind == DateTimeKind.Local
			       	? ((DateTime) value).ToUniversalTime()
			       	: value;
		}

		#endregion
	}
}