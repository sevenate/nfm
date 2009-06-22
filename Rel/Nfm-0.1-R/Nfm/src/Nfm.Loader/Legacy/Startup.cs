// <copyright file="Startup.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-08</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-08</date>
// </editor>
// <summary>Application startup object.</summary>

using System;

namespace Nfm.Loader.Legacy
{
	/// <summary>
	/// Application startup object.
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// Entry point.
		/// </summary>
		/// <param name="args">Command line arguments.</param>
		[STAThread]
		public static void Main(string[] args)
		{
			var wrapper = new SingleInstanceApplicationWrapper();
			wrapper.Run(args);
		}
	}
}