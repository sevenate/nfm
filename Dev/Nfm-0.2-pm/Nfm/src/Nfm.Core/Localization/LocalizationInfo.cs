// <copyright file="LocalizationInfo.cs" company="HD">
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
// <summary>Represent translation and culture information retrieved from language file.</summary>

using System.Globalization;

namespace Nfm.Core.Localization
{
	/// <summary>
	/// Represent translation and culture information retrieved from language file.
	/// </summary>
	public class LocalizationInfo
	{
		#region Properties

		/// <summary>
		/// Gets custom localization friendly name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets localization culture information.
		/// </summary>
		public CultureInfo Culture { get; private set; }

		/// <summary>
		/// Gets version of localization.
		/// </summary>
		public string Version { get; private set; }

		/// <summary>
		/// Gets authors of localization.
		/// </summary>
		public string Author { get; private set; }

		/// <summary>
		/// Gets update url for localization.
		/// </summary>
		public string UpdateUrl { get; private set; }

		/// <summary>
		/// Gets localization file name.
		/// </summary>
		public string FileName { get; private set; }

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizationInfo"/> class.
		/// </summary>
		/// <param name="name">Localization name.</param>
		/// <param name="culture">Culture information.</param>
		/// <param name="version">Localization vertion.</param>
		/// <param name="author">Author of localization.</param>
		/// <param name="updateUrl">Update url this localization.</param>
		/// <param name="fileName">File name with this localization.</param>
		internal LocalizationInfo(
			string name, CultureInfo culture, string version, string author, string updateUrl, string fileName)
		{
			Name = name;
			Culture = culture;
			Version = version;
			Author = author;
			UpdateUrl = updateUrl;
			FileName = fileName;
		}

		#endregion
	}
}