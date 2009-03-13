using System;
using System.Windows.Data;
using System.Diagnostics;
using System.Windows.Markup;

namespace Nfm.Core.Converters
{
	/// <summary>
	/// Convert to debug the binding values.
	/// </summary>
	public class DebugConvertor : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
//			Debugger.Break();
			//return Binding.DoNothing;
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
//			Debugger.Break();
			//return Binding.DoNothing;
			return value;
		}

		#endregion
	}

	/// <summary>
	/// Markup extension to debug databinding.
	/// </summary>
	public class DebugBindingExtension : MarkupExtension
	{
		/// <summary>
		/// Creates a new instance of the Convertor for debugging.
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <returns>Return a convertor that can be debugged to see values for the binding.</returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new DebugConvertor();
		}
	}
}