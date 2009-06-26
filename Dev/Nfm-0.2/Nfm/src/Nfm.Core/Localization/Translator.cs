// <copyright file="Translator.cs" company="HD">
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
// <summary>Helper translation changer.</summary>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace Nfm.Core.Localization
{
	/// <summary>
	/// Helper translation changer.
	/// <code>
	/// Translator.Culture = (CultureInfo)languages.SelectedItem
	/// </code>
	/// </summary>
	public static class Translator
	{
		#region Constants

		/// <summary>
		/// Fallback culture.
		/// </summary>
		public static readonly CultureInfo DefaultCulture;

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes static members of the <see cref="Translator"/> class.
		/// </summary>
		static Translator()
		{
			DefaultCulture = new CultureInfo("en-US");
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets current localization resource provider.
		/// </summary>
		public static ILocalizationProvider LocalizationProvider { get; set; }

		/// <summary>
		/// Gets all available localizations.
		/// </summary>
		/// <returns>Enumeratates through all available localizations.</returns>
		public static IEnumerable<LocalizationInfo> AvailableLocalizations
		{
			get { return LocalizationProvider.AvailableLocalizations; }
		}

		/// <summary>
		/// Gets or sets current localization to new value.
		/// </summary>
		public static LocalizationInfo CurrentLocalization
		{
			get { return LocalizationProvider.CurrentLocalization; }
			set
			{
				LocalizationProvider.CurrentLocalization = value;

				if (value != null && value.Culture != null)
				{
					Thread.CurrentThread.CurrentUICulture = value.Culture;
				}
				else
				{
					// fallback to default culture available in resources
					Thread.CurrentThread.CurrentUICulture = DefaultCulture;
				}

				if (CultureChanged != null)
				{
					CultureChanged(null, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Gets current UI culture.
		/// </summary>
		public static CultureInfo CurrentUICulture
		{
			get { return Thread.CurrentThread.CurrentUICulture; }
		}

		/// <summary>
		/// Gets current culture.
		/// </summary>
		public static CultureInfo CurrentCulture
		{
			get { return Thread.CurrentThread.CurrentCulture; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Refresh available localizations.
		/// </summary>
		public static void RefreshAvailableLocalizations()
		{
			LocalizationProvider.RefreshAvailable();
		}

		/// <summary>
		/// Gets a localized value for the specified resource key.
		/// </summary>
		/// <param name="key">The localized resource key.</param>
		/// <returns>Localized resource value.</returns>
		public static object GetValue(string key)
		{
			object result = null;

			if (LocalizationProvider != null && LocalizationProvider.CurrentLocalization != null)
			{
				result = LocalizationProvider.GetValue(key);
			}

			// Fallback strategy: lookup in assembly resources
			return result ?? Properties.Resources.ResourceManager.GetString(key);
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when current application culture was changed.
		/// </summary>
		public static event EventHandler CultureChanged;

		#endregion
	}
}