// <copyright file="IPanelContentHost.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-24</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-24</date>
// </editor>
// <summary>Panel with content.</summary>

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Panel with content.
	/// </summary>
	public interface IPanelContentHost : IPanel
	{
		/// <summary>
		/// Gets or sets panel content.
		/// </summary>
		IPanelContent PanelContent { get; set; }
	}
}