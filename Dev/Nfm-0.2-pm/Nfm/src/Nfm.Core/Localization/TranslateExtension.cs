// <copyright file="TranslateExtension.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Christian Moser">
//	<url>http://www.wpftutorial.net/LocalizeMarkupExtension.html</url>
// 	<date>2009-01-08</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-02-28</date>
// </editor>
// <summary>Translation markup extension.</summary>

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Nfm.Core.Localization
{
	/// <summary>
	/// Translation markup extension.
	/// </summary>
	[MarkupExtensionReturnType(typeof (string)), Localizability(LocalizationCategory.NeverLocalize)]
	public class TranslateExtension : MarkupExtension
	{
		#region Fields

		/// <summary>
		/// Target dependence object.
		/// </summary>
		private DependencyObject targetObject;

		/// <summary>
		/// Target dependency prorerty.
		/// </summary>
		private DependencyProperty targetProperty;

		/// <summary>
		/// Caches the resolved default type converter.
		/// </summary>
		private TypeConverter typeConverter;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the resource key.
		/// </summary>
		/// <value>The resource key.</value>
		[ConstructorArgument("key")]
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets a format string that is used to format the value
		/// </summary>
		/// <value>The format.</value>
		[ConstructorArgument("format")]
		public string Format { get; set; }

		/// <summary>
		/// Gets or sets the default value, that is used, when the key was not found or the localized value is null.
		/// </summary>
		/// <value>The default value.</value>
		[ConstructorArgument("DefaultValue")]
		public object DefaultValue { get; set; }

		/// <summary>
		/// Gets or sets the converter.
		/// </summary>
		/// <value>The converter.</value>
		[ConstructorArgument("Converter")]
		public IValueConverter Converter { get; set; }

		#endregion

		#region .Ctors / Finalize

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslateExtension"/> class.
		/// </summary>
		public TranslateExtension()
		{
			Translator.CultureChanged += CultureChanged;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslateExtension"/> class.
		/// </summary>
		/// <param name="key">Localization value key.</param>
		public TranslateExtension(string key) : this()
		{
			Key = key;
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="TranslateExtension"/> class,
		/// releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="TranslateExtension"/> is reclaimed by garbage collection.
		/// </summary>
		~TranslateExtension()
		{
			Translator.CultureChanged -= CultureChanged;
		}

		#endregion

		#region MarkupExtension Overrides

		/// <summary>
		/// When implemented in a derived class,
		/// returns an object that is set as the value of the target property for this markup extension.
		/// </summary>
		/// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
		/// <returns>The object value to set on the property where the extension is applied.</returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			// Ignore if in design mode
			if ((bool) DesignerProperties
			           	.IsInDesignModeProperty
			           	.GetMetadata(typeof (DependencyObject)).DefaultValue)
			{
				return null;
			}

			// Resolve the depending object and property
			if (targetObject == null)
			{
				var targetHelper = (IProvideValueTarget) serviceProvider.GetService(typeof (IProvideValueTarget));

				// Note: Important check for System.Windows.SharedDp,
				// when using markup extension inside of ControlTemplates and DataTemplates.
				// Details: http://social.msdn.microsoft.com/forums/en-US/wpf/thread/a9ead3d5-a4e4-4f9c-b507-b7a7d530c6a9/
				if (!(targetHelper.TargetObject is DependencyObject))
				{
					return this;
				}

				targetObject = (DependencyObject) targetHelper.TargetObject;
				targetProperty = (DependencyProperty) targetHelper.TargetProperty;
				typeConverter = TypeDescriptor.GetConverter(targetProperty.PropertyType);
			}

			return ProvideValueInternal();
		}

		#endregion

		#region Private

		/// <summary>
		/// Handles the <see cref="Translator.CultureChanged"/> event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void CultureChanged(object sender, EventArgs e)
		{
			if (targetObject != null && targetProperty != null)
			{
				targetObject.SetValue(targetProperty, ProvideValueInternal());
			}
		}

		/// <summary>
		/// Provides the value internal.
		/// </summary>
		/// <returns>Localized value.</returns>
		private object ProvideValueInternal()
		{
			// Get the localized value
			object value = Translator.GetValue(Key);

			// Automatically convert the type if a matching type converter is available
			if (value != null && typeConverter != null && typeConverter.CanConvertFrom(value.GetType()))
			{
				value = typeConverter.ConvertFrom(value);
			}

			// If the value is null, use the fallback value if available
			if (value == null && DefaultValue != null)
			{
				value = DefaultValue;
			}

			// If no fallback value is available, return the key
			if (value == null)
			{
				if (targetProperty != null
					&& (targetProperty.PropertyType == typeof (string) || targetProperty.Name == "ToolTip"))
				{
					// Return the key surrounded by question marks for string type properties
					value = string.Concat("?", Key, "?");
				}
				else
				{
					// Return the UnsetValue for all other types of dependency properties
					return DependencyProperty.UnsetValue;
				}
			}

			if (Converter != null)
			{
				value = Converter.Convert(value, targetProperty.PropertyType, null, CultureInfo.CurrentCulture);
			}

			// Format the value if a format string is provided and the type implements IFormattable
			if (value is IFormattable && Format != null)
			{
				((IFormattable) value).ToString(Format, CultureInfo.CurrentCulture);
			}

			return value;
		}

		#endregion
	}
}