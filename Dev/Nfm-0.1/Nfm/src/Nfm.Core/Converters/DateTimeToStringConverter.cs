// <copyright file="DateTimeToStringConverter.cs" company="HD">
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
// <summary>Convert nullable <see cref="DateTime"/> value to user friendly string.</summary>

using System;
using System.Globalization;
using System.Windows.Data;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Convert nullable <see cref="DateTime"/> value to user friendly string.
	/// </summary>
	[ValueConversion(typeof (DateTime?), typeof (string))]
	public class DateTimeToStringConverter : IValueConverter
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
			var date = (DateTime?)value;

			if (!date.HasValue)
			{
				return string.Empty;
			}

			// TODO: finish with this "date -> string" userfriendly convertion.

/*			TimeSpan diff = DateTime.Now - date.Value;

			//Today
			if (diff < new TimeSpan(0, 1, 0))
			{
				return string.Format("{0}s ago", diff.Seconds);
			}

			if (diff < new TimeSpan(0, 60, 0))
			{
				return string.Format("{0}m, {1}s ago", diff.Minutes, diff.Seconds);
			}

			if (diff < new TimeSpan(1, 0, 0, 0))
			{
				return string.Format("{0}h, {1}m, {2}s ago", diff.Hours, diff.Minutes, diff.Seconds);
			}

			//Yesterday
//			if (new TimeSpan(1, 0, 0, 0) <= diff && diff < new TimeSpan(2, 0, 0, 0))
//			{
//				return string.Format("Yesterday at {0:00}:{1:00}", date.Value.Hour, date.Value.Minute);
//			}

			//1-7 Days ago
//			if (new TimeSpan(1, 0, 0, 0) <= diff && diff < new TimeSpan(8, 0, 0, 0))
			if (DateTime.Now.Day - date.Value.Day < 10)
			{
//				return string.Format("{0} days ago at {1:00}:{2:00}", diff.Days, date.Value.Hour, date.Value.Minute);
				return string.Format("{0}d ago at {1:00}:{2:00}:{3:00}", DateTime.Now.Day - date.Value.Day, date.Value.Hour, date.Value.Minute, date.Value.Second);
			}

//			return string.Format(culture.DateTimeFormat, "{0}", date);
			return string.Format("{0:yyyy MMM dd} {0:HH:mm:ss}", date);
*/
			return string.Format("{0:F}", date);
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