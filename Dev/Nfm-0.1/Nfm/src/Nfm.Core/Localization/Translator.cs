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
using System.Globalization;
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
		/// <summary>
		/// Gets or sets current application culture.
		/// </summary>
		public static CultureInfo Culture
		{
			get { return Thread.CurrentThread.CurrentUICulture; }
			set
			{
				Thread.CurrentThread.CurrentUICulture = value;
				if (CultureChanged != null)
				{
					CultureChanged(null, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Raised when current application culture was changed.
		/// </summary>
		internal static event EventHandler CultureChanged;
	}
}