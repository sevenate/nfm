// <copyright file="ViewNodeAddInView.cs" company="HD">
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
// <summary>Add-in view for IViewNode contract.</summary>

using System.AddIn.Pipeline;

namespace Nfm.AddInView
{
	/// <summary>
	/// Add-in view for IViewNode contract.
	/// </summary>
	[AddInBase]
	public abstract class ViewNodeAddInView
	{
		/// <summary>
		/// Start view node content.
		/// </summary>
		public abstract void ViewNode();
	}
}