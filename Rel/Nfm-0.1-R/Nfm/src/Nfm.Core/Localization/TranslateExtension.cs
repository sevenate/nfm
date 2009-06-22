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
using System.Windows;
using System.Windows.Markup;

namespace Nfm.Core.Localization
{
	/// <summary>
	/// Translation markup extension.
	/// </summary>
	[MarkupExtensionReturnType(typeof (string))]
	public class TranslateExtension : MarkupExtension
	{
		#region Fields

		/// <summary>
		/// Localization value key.
		/// </summary>
		private readonly string key;

		/// <summary>
		/// Target dependence object.
		/// </summary>
		private DependencyObject targetObject;

		/// <summary>
		/// Target dependency prorerty.
		/// </summary>
		private DependencyProperty targetProperty;

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="TranslateExtension"/> class.
		/// </summary>
		/// <param name="key">Localization value key.</param>
		public TranslateExtension(string key)
		{
			this.key = key;
			Translator.CultureChanged += delegate
			                             {
											 if (targetObject != null && targetProperty != null)
											 {
												 targetObject.SetValue(targetProperty, GetLocalizedValue(key));
											 }
			                             };
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
			var targetHelper = (IProvideValueTarget) serviceProvider.GetService(typeof (IProvideValueTarget));
			targetObject = targetHelper.TargetObject as DependencyObject;
			targetProperty = targetHelper.TargetProperty as DependencyProperty;
			return GetLocalizedValue(key);
		}

		#endregion

		#region Extension Point

		/// <summary>
		/// Lookup specific localized value.
		/// </summary>
		/// <param name="localizationItemKey">Localization value key.</param>
		/// <returns>Localization value.</returns>
		protected static object GetLocalizedValue(string localizationItemKey)
		{
			throw new NotImplementedException("Lookup in XML file is not implemented yet.");
			//return Resources.ResourceManager.GetObject(localizationItemKey);
		}

		#endregion
	}
}