// <copyright file="SharedResourceDictionary.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Christian Moser">
// 	<url>http://www.wpftutorial.net/MergedDictionaryPerformance.html</url>
// 	<date>2009-03-25</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-03-25</date>
// </editor>
// <summary>
// The shared resource dictionary is a specialized resource dictionary
// that loads it content only once. If a second instance with the same source
// is created, it only merges the resources from the cache.
// </summary>

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;

[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation",
	"WPFTutorial.Utils")]

namespace Nfm.Core.Resources
{
	/// <summary>
	/// The shared resource dictionary is a specialized resource dictionary
	/// that loads it content only once. If a second instance with the same source
	/// is created, it only merges the resources from the cache.
	/// </summary>
	public class SharedResourceDictionary : ResourceDictionary
	{
		/// <summary>
		/// Internal cache of loaded dictionaries 
		/// </summary>
		private static readonly Dictionary<Uri, ResourceDictionary> sharedDictionaries =
			new Dictionary<Uri, ResourceDictionary>();

		/// <summary>
		/// Local member of the source uri
		/// </summary>
		private Uri sourceUri;

		/// <summary>
		/// Gets or sets the uniform resource identifier (URI) to load resources from.
		/// </summary>
		public new Uri Source
		{
			get { return sourceUri; }
			set
			{
				sourceUri = value;

				if (!sharedDictionaries.ContainsKey(value))
				{
					// If the dictionary is not yet loaded, load it by setting
					// the source of the base class
					base.Source = value;

					// add it to the cache
					sharedDictionaries.Add(value, this);
				}
				else
				{
					// If the dictionary is already loaded, get it from the cache
					MergedDictionaries.Add(sharedDictionaries[value]);
				}
			}
		}
	}
}