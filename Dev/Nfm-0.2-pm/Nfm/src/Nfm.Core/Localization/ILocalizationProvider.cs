// <copyright file="ILocalizationProvider.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-23</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-23</date>
// </editor>
// <summary>Common description of translatar service.</summary>

using System.Collections.Generic;

namespace Nfm.Core.Localization
{
	/// <summary>
	/// Common description of translatar service.
	/// </summary>
	public interface ILocalizationProvider
	{
		/// <summary>
		/// Gets or sets current localization.
		/// </summary>
		LocalizationInfo CurrentLocalization { get; set; }

		/// <summary>
		/// Gets all available localizations.
		/// </summary>
		/// <returns>Enumeratates through all available localizations.</returns>
		IEnumerable<LocalizationInfo> AvailableLocalizations { get; }

		/// <summary>
		/// Refresh available localizations.
		/// </summary>
		void RefreshAvailable();

		/// <summary>
		/// Gets the localized value for the specified key.
		/// </summary>
		/// <param name="key">The localization value key.</param>
		/// <returns>Localized value.</returns>
		object GetValue(string key);
	}
}