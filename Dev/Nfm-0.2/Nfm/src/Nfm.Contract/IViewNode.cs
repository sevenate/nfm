// <copyright file="IViewNode.cs" company="HD">
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
// <summary>Allow extend node content view.</summary>

using System.AddIn.Contract;
using System.AddIn.Pipeline;

namespace Nfm.Contract
{
	/// <summary>
	/// Allow view node content.
	/// </summary>
	[AddInContract]
	public interface IViewNode : IContract
	{
		/// <summary>
		/// Start view node content.
		/// </summary>
		void View();
	}
}