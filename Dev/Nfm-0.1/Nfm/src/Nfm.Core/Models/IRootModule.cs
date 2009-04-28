// <copyright file="IRootModule.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-14</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-14</date>
// </editor>
// <summary>General root modul interface.</summary>

namespace Nfm.Core.Models
{
	/// <summary>
	/// General root modul interface.
	/// </summary>
	public interface IRootModule
	{
		/// <summary>
		/// Gets module name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets unique module identification key.
		/// </summary>
		string Key { get; }
	}
}