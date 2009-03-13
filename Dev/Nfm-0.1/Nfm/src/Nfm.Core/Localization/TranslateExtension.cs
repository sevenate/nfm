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
		private readonly string key;
		private DependencyObject targetObject;
		private DependencyProperty targetProperty;

		public TranslateExtension(string key)
		{
			this.key = key;
			Translator.CultureChanged += Translator_CultureChanged;
		}

		private void Translator_CultureChanged(object sender, EventArgs e)
		{
			if (targetObject != null && targetProperty != null)
			{
				targetObject.SetValue(targetProperty, GetLocalizedValue(key));
			}
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var targetHelper = (IProvideValueTarget) serviceProvider.GetService(typeof (IProvideValueTarget));
			targetObject = targetHelper.TargetObject as DependencyObject;
			targetProperty = targetHelper.TargetProperty as DependencyProperty;
			return GetLocalizedValue(key);
		}

		protected static object GetLocalizedValue(string localizationItemKey)
		{
			throw new NotImplementedException("Lookup in XML file is not implemented yet.");
			//return Resources.ResourceManager.GetObject(localizationItemKey);
		}
	}
}