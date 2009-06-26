// <copyright file="ChangeLocalizationCommand.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-26</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-26</date>
// </editor>
// <summary>Change current localization command.</summary>

using System.Linq;
using Caliburn.PresentationFramework.Filters;
using Nfm.Core.Localization;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Change current localization command.
	/// </summary>
	public class ChangeLocalizationCommand
	{
		/// <summary>
		/// Change current application localization.
		/// </summary>
		/// <param name="localization">Panel to close.</param>
		[Preview("CanExecute")]
		public void Execute(LocalizationInfo localization)
		{
			// Try to switch to the next available localization
			if (localization == null && Translator.AvailableLocalizations.Count() > 0)
			{
				bool foundCurrentCulture = false;

				foreach (LocalizationInfo localizationInfo in Translator.AvailableLocalizations)
				{
					// Any available if fine for case, when only default fallback localization is currently used
					if (Translator.CurrentLocalization == null)
					{
						Translator.CurrentLocalization = localizationInfo;
						break;
					}

					// Setting next localization from available
					if (foundCurrentCulture)
					{
						Translator.CurrentLocalization = localizationInfo;
						foundCurrentCulture = false;
						break;
					}

					// Current localization is found and next iteration
					// will be used to apply next localization from available
					if (Translator.CurrentLocalization.FileName == localizationInfo.FileName)
					{
						foundCurrentCulture = true;
					}
				}

				// current localization was last in the available list, so starting from first available
				if (foundCurrentCulture)
				{
					Translator.CurrentLocalization = Translator.AvailableLocalizations.First();
				}
			}
			else
			{
				// Set specific localization
				Translator.CurrentLocalization = localization;
			}
		}

		/// <summary>
		/// Check if the localization is valid to be applied.
		/// </summary>
		/// <param name="localization">New localization.</param>
		/// <returns>True, if localization can be applied.</returns>
		public bool CanExecute(LocalizationInfo localization)
		{
			return true;
		}
	}
}