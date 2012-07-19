// <copyright file="EnumConverter.cs" company="HD">
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
// <summary>Convert enum value to friendly text.</summary>

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using Nfm.Core.Models;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Convert enum value to friendly text.
	/// </summary>
	public class EnumConverter : IValueConverter
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
			if (typeof (int).IsAssignableFrom(targetType))
			{
				return (int) value;
			}

			return new BindableEnum
			       {
			       	Value = value,
			       	UnderlyingValue = (int) value,
			       	DisplayName = Enum.GetName(value.GetType(), value)
			       };
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
			if (value == null)
			{
				return null;
			}

			if (value is string)
			{
				return Enum.Parse(targetType, value.ToString(), true);
			}

			return value.GetType() == targetType
			       	? value
			       	: ((BindableEnum) value).Value;
		}

		#endregion

		#region Singleton

		/// <summary>
		/// Singleton instance.
		/// </summary>
		private static EnumConverter instance;

		/// <summary>
		/// Prevents a default instance of the <see cref="EnumConverter"/> class from being created.
		/// </summary>
		private EnumConverter()
		{
		}

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		public static EnumConverter Inst
		{
			[DebuggerStepThrough]
			get
			{
				if (instance == null)
				{
					instance = new EnumConverter();
				}

				return instance;
			}
		}

		#endregion
	}
}