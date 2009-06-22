// <copyright file="ResourceCache.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Dr. WPF">
//	<url>http://www.drwpf.com/blog/Home/tabid/36/EntryID/10/Default.aspx</url>
// 	<date>2007-10-06</date>
// </author>
// <modifier name="jbe2277">
//	<url2>http://www.codeplex.com/CompositeExtensions/Thread/View.aspx?ThreadId=42919</url2>
// 	<date>2009-02-09</date>
// </modifier>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-01</date>
// </editor>
// <summary>
//  Cache resource dictionaries and ensure that each dictionary was loaded only once.
// </summary>

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace Nfm.Core.Resources
{
	/// <summary>
	/// Cache resource dictionaries and ensure that each dictionary was loaded only once.
	/// </summary>
	public static class ResourceCache
	{
		#region MergedDictionaries

		/// <summary>
		/// Identifies the attached MergedDictionaries property. This property used to load specific resource dictionary
		/// and merge it with any <see cref="FrameworkElement"/> or <see cref="FrameworkContentElement"/> resources.
		/// </summary>
		/// <remarks>
		/// Use "|" to separate multiple resource dictionaries.
		/// </remarks>
		public static readonly DependencyProperty MergedDictionariesProperty =
			DependencyProperty.RegisterAttached(
				"MergedDictionaries",
				typeof (string),
				typeof (ResourceCache),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnMergedDictionariesChanged));

		/// <summary>
		/// Gets the value of the <see cref="MergedDictionariesProperty"/> attached property of the element.
		/// </summary>
		/// <param name="d">The element to read the value from.</param>
		/// <returns>Property value.</returns>
		public static string GetMergedDictionaries(DependencyObject d)
		{
			return (string) d.GetValue(MergedDictionariesProperty);
		}

		/// <summary>
		/// Sets the value of the <see cref="MergedDictionariesProperty"/> attached property of the element.
		/// </summary>
		/// <param name="d">The element to set the value.</param>
		/// <param name="value">The value to set.</param>
		public static void SetMergedDictionaries(DependencyObject d, string value)
		{
			d.SetValue(MergedDictionariesProperty, value);
		}

		/// <summary>
		/// <see cref="MergedDictionariesProperty"/> value changed callback.
		/// </summary>
		/// <param name="d">The element on which the property has changed value.</param>
		/// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this property.</param>
		private static void OnMergedDictionariesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var dictionaries = e.NewValue as string;

			if (!string.IsNullOrEmpty(dictionaries))
			{
				foreach (string dictionaryPath in dictionaries.Split('|'))
				{
					ResourceDictionary dictionary = GetResourceDictionary(new Uri(dictionaryPath, UriKind.Relative));

					if (dictionary != null)
					{
						if (d is FrameworkElement)
						{
							((FrameworkElement) d).Resources.MergedDictionaries.Add(dictionary);
						}
						else if (d is FrameworkContentElement)
						{
							((FrameworkContentElement) d).Resources.MergedDictionaries.Add(dictionary);
						}
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// Store already loaded and cached <see cref="ResourceDictionary"/>.
		/// </summary>
		private static readonly Dictionary<Uri, WeakReference> CachedDictionaries = new Dictionary<Uri, WeakReference>();

		/// <summary>
		/// Load specific <see cref="ResourceDictionary"/> and store it in cache to prevent loading in the future.
		/// </summary>
		/// <param name="source">Path to dictionary.</param>
		/// <returns>Corresponding <see cref="ResourceDictionary"/>.</returns>
		public static ResourceDictionary GetResourceDictionary(Uri source)
		{
			if (source.IsAbsoluteUri && source.Scheme == "pack")
			{
				source = new Uri(source.AbsolutePath, UriKind.Relative);
			}

			ResourceDictionary dictionary;

			if (!CachedDictionaries.ContainsKey(source) || CachedDictionaries[source].Target == null)
			{
				dictionary = (ResourceDictionary) Application.LoadComponent(source);
				CachedDictionaries[source] = new WeakReference(dictionary);
			}
			else
			{
				dictionary = (ResourceDictionary) CachedDictionaries[source].Target;
			}

			return dictionary;
		}

		/// <summary>
		/// Gets the pack URI from an assembly-relative resource path.
		/// </summary>
		/// <param name="resourcePath">The resource path.</param>
		/// <returns>Corresponding pack Uri.</returns>
		public static Uri GetPackUri(string resourcePath)
		{
			return
				new Uri(
					string.Format(
						"pack://application:,,,/{0};Component/{1}",
						Assembly.GetCallingAssembly().GetName().Name,
						resourcePath));
		}
	}
}