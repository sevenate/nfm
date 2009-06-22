// <copyright file="AddInManager.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-07</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-07</date>
// </editor>
// <summary>Manage all application add-ins.</summary>

using System;
using System.AddIn.Hosting;
using System.Collections.Generic;
using Nfm.HostView;

namespace Nfm.Core
{
	/// <summary>
	/// Manage all application add-ins.
	/// </summary>
	public class AddInManager
	{
		public static void Update()
		{
			string path = Environment.CurrentDirectory;
			AddInStore.Update(path);
		}

		public static void Find()
		{
			string path = Environment.CurrentDirectory;
			IList<AddInToken> tokens = AddInStore.FindAddIns(typeof(ViewNodeHostView), path);

			var addin = tokens[0].Activate<ViewNodeHostView>(AddInSecurityLevel.Internet);
			addin.View();
		}
	}
}