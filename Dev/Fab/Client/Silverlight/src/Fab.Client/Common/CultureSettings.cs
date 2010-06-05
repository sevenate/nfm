// <copyright file="CultureSettings.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-06-05</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-06-05</date>
// </editor>
// <summary>Used to get <see cref="XmlLanguage"/> for current OS culture.</summary>

using System.Threading;
using System.Windows.Markup;

namespace Fab.Client.Common
{
	/// <summary>
	/// Used to get <see cref="XmlLanguage"/> for current OS culture.
	/// </summary>
	public class CultureSettings
	{
		/// <summary>
		/// Gets <see cref="XmlLanguage"/> for current OS culture.
		/// Used for input data format binding and StringFormat in binding.
		/// </summary>
		public XmlLanguage Language
		{
			get
			{
				return XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
			}
		}
	}
}