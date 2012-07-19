// <copyright file="SwitchThemeCommand.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-02</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-02</date>
// </editor>
// <summary>Switch current application theme to next from all available.</summary>

using Caliburn.Core.Metadata;
using Caliburn.PresentationFramework.Filters;
using Nfm.Core.Commands.Interfaces;
using Nfm.Core.Themes;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Switch current application theme to next from all available.
	/// </summary>
	[Singleton(typeof(ISwitchThemeCommand))]
	public class SwitchThemeCommand : ISwitchThemeCommand
	{
		/// <summary>
		/// Switch application theme to next from available.
		/// </summary>
		[Preview("CanExecute")]
		public void Execute()
		{
			var currentTheme = Theme.GetCurrentThemeName();

			switch (currentTheme)
			{
				case "":
					Theme.ApplyTheme("Themes/Light/LightTheme.xaml");
					break;

				case "LightTheme.xaml":
//					Theme.ApplyTheme("Themes/Dark/DarkTheme.xaml");
					Theme.ClearApplicationThemeToDefault();
					break;

//				default:
//					Theme.ApplyTheme("Themes/Light/LightTheme.xaml");
				break;
			}
		}

		/// <summary>
		/// Check if the theme can be switched.
		/// </summary>
		/// <returns>True always.</returns>
		public bool CanExecute()
		{
			return true;
		}
	}
}