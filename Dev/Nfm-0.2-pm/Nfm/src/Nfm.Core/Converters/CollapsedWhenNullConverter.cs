// <copyright file="CollapsedWhenNullConverter.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-03-27</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-03-27</date>
// </editor>
// <summary>Convert boolean value to corner radius of <see cref="Border"/>.</summary>

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Convert object reference to Visibility value.
	/// </summary>
	[ValueConversion(typeof (object), typeof (Visibility))]
	public class CollapsedWhenNullConverter : IValueConverter
	{
		#region IValueConverter Members

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
			return value == null
			       	? Visibility.Collapsed
			       	: Visibility.Visible;
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
			throw new NotImplementedException();
		}

		#endregion

		#region Singleton

		/// <summary>
		/// Singleton instance.
		/// </summary>
		private static CollapsedWhenNullConverter instance;

		/// <summary>
		/// Prevents a default instance of the <see cref="CollapsedWhenNullConverter"/> class from being created.
		/// </summary>
		private CollapsedWhenNullConverter()
		{
		}

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		public static CollapsedWhenNullConverter Inst
		{
			[DebuggerStepThrough]
			get
			{
				if (instance == null)
				{
					instance = new CollapsedWhenNullConverter();
				}

				return instance;
			}
		}

		#endregion
	}
}