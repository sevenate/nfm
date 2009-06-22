// <copyright file="HyperlinkUtility.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Ed Ball">
//	<url>http://code.logos.com/blog/2008/01/hyperlinks_to_the_web_in_wpf.html</url>
// 	<date>2008-01-17</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-29</date>
// </editor>
// <summary><see cref="Hyperlink"/> control helper used to launch default browser.</summary>

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Nfm.Core.Controls
{
	/// <summary>
	/// <see cref="Hyperlink"/> control helper used to launch default browser.
	/// </summary>
	public static class HyperlinkUtility
	{
		#region LaunchDefaultBrowser Dependency Property

		/// <summary>
		/// LaunchDefaultBrowser attached property.
		/// </summary>
		public static readonly DependencyProperty LaunchDefaultBrowserProperty =
			DependencyProperty.RegisterAttached(
				"LaunchDefaultBrowser",
				typeof (bool),
				typeof (HyperlinkUtility),
				new PropertyMetadata(false, OnLaunchDefaultBrowserChanged));

		/// <summary>
		/// Attach <see cref="LaunchDefaultBrowserProperty"/> property to target dependency object.
		/// </summary>
		/// <param name="depObj">Attach target.</param>
		/// <param name="isSet">The new local value.</param>
		public static void SetLaunchDefaultBrowser(DependencyObject depObj, bool isSet)
		{
			depObj.SetValue(LaunchDefaultBrowserProperty, isSet);
		}

		/// <summary>
		/// Detattach <see cref="LaunchDefaultBrowserProperty"/> property from target dependency object.
		/// </summary>
		/// <param name="depObj">Detattach target.</param>
		/// <returns>Returns the current effective value.</returns>
		public static bool GetLaunchDefaultBrowser(DependencyObject depObj)
		{
			return (bool) depObj.GetValue(LaunchDefaultBrowserProperty);
		}

		/// <summary>
		/// <see cref="LaunchDefaultBrowserProperty"/> attached property changed callback.
		/// </summary>
		/// <param name="depObj">Attached property target.</param>
		/// <param name="args">Provides data for attached property changed event.</param>
		private static void OnLaunchDefaultBrowserChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
		{
			if ((bool) args.NewValue)
			{
				ElementUtility.AddHandler(
					depObj, Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(OnHyperlinkRequestNavigateEvent));
			}
			else
			{
				ElementUtility.RemoveHandler(
					depObj, Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(OnHyperlinkRequestNavigateEvent));
			}
		}

		#endregion

		/// <summary>
		/// <see cref="Hyperlink.RequestNavigateEvent"/> handler.
		/// </summary>
		/// <param name="sender">The object where the event handler is attached - <see cref="TextBlock"/> or <see cref="Hyperlink"/> element.</param>
		/// <param name="e">Provides data for the <see cref="Hyperlink.RequestNavigateEvent"/> event.</param>
		private static void OnHyperlinkRequestNavigateEvent(object sender, RequestNavigateEventArgs e)
		{
			// Note: in some circumstances, with some browsers, Process.Start will raise an exception,
			// even though it might actually succeed, so we wrap the call in a try/catch block and hope for the best.
			try
			{
				Mouse.OverrideCursor = Cursors.AppStarting;

				if (e.Uri.ToString().Contains("@"))
				{
					Process.Start("mailto:" + e.Uri);
				}
				else
				{
					Process.Start(e.Uri.ToString());
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				Debugger.Break();
			}
			finally
			{
				Mouse.OverrideCursor = null;
			}

			e.Handled = true;
		}
	}
}