// <copyright file="XmlFileLocalizationProvider.cs" company="HD">
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
// <summary>Provide localized values from xml translation files.</summary>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Nfm.Core.Localization
{
	/// <summary>
	/// Provide localized values from xml translation files.
	/// </summary>
	public class XmlFileLocalizationProvider : ILocalizationProvider
	{
		#region Fields

		/// <summary>
		/// All available localizations.
		/// </summary>
		private readonly IList<LocalizationInfo> availableLocalizations = new List<LocalizationInfo>();

		/// <summary>
		/// Current loaded xml localization file cache.
		/// </summary>
		private readonly Dictionary<string, string> cache = new Dictionary<string, string>();

		/// <summary>
		/// Current localization.
		/// </summary>
		private LocalizationInfo currentLocalization;

		#endregion

		#region Configuration Properties

		/// <summary>
		/// Gets or sets the path to folder with localizaions xml files.
		/// </summary>
		public string Folder { get; set; }

		#endregion

		#region Implementation of ILocalizationProvider

		/// <summary>
		/// Gets all available localizations info.
		/// </summary>
		/// <returns>List with all supported localizations.</returns>
		public IEnumerable<LocalizationInfo> AvailableLocalizations
		{
			get { return availableLocalizations; }
		}

		/// <summary>
		/// Gets or sets current localization to new value.
		/// </summary>
		public LocalizationInfo CurrentLocalization
		{
			get { return currentLocalization; }
			set
			{
				cache.Clear();

				if (value != null)
				{
					XDocument xDocument = XDocument.Load(value.FileName);
					XElement translation = xDocument.Element("translation");

					if (translation != null)
					{
						foreach (XElement xElement in translation.Elements())
						{
							XAttribute key = xElement.Attribute("key");
							XAttribute localizedValue = xElement.Attribute("value");

							if (key == null)
							{
								throw new Exception(
									"Localization file formant is currupted or not supported: attribute \"key\" on element \"translation\" was not found.");
							}

							if (localizedValue == null)
							{
								throw new Exception(
									"Localization file formant is currupted or not supported: attribute \"value\" on element \"translation\" was not found.");
							}

							cache.Add(key.Value, localizedValue.Value);
						}
					}
					else
					{
						throw new Exception(
							"Localization file formant is currupted or not supported: element \"translation\" was not found.");
					}
				}

				currentLocalization = value;
			}
		}

		/// <summary>
		/// Refresh available localizations.
		/// </summary>
		public void RefreshAvailable()
		{
			availableLocalizations.Clear();

			if (Directory.Exists(Folder))
			{
				string[] localizationFiles = Directory.GetFiles(Folder, "*.xml", SearchOption.AllDirectories);

				foreach (string localizationFile in localizationFiles)
				{
					using (StreamReader streamReader = File.OpenText(localizationFile))
					{
						using (XmlReader xmlReader = XmlReader.Create(streamReader))
						{
							while (xmlReader.Read())
							{
								// move to "translation" content
								if (xmlReader.LocalName == "translation" && xmlReader.NodeType == XmlNodeType.Element)
								{
									availableLocalizations.Add(
										new LocalizationInfo(
											xmlReader["name"],
											new CultureInfo(xmlReader["culture"]),
											xmlReader["version"],
											xmlReader["author"],
											xmlReader["updateurl"],
											localizationFile));

									break;
								}
							}
						}
					}
				}
			}
			else
			{
				CurrentLocalization = null;
			}
		}

		/// <summary>
		/// Gets the localized value for the specified key.
		/// </summary>
		/// <param name="key">The localization value key.</param>
		/// <returns>Localized value.</returns>
		public object GetValue(string key)
		{
			return cache.ContainsKey(key)
			       	? cache[key]
			       	: null;
		}

		#endregion
	}
}