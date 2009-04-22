// <copyright file="INodePanel.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-22</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-22</date>
// </editor>
// <summary>Represent panel with <see cref="INode"/>.</summary>

using Nfm.Core.Models;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Represent panel with <see cref="INode"/>.
	/// </summary>
	public interface INodePanel : IPanel
	{
		/// <summary>
		/// Gets specific <see cref="INode"/>.
		/// </summary>
		INode Node { get; }
	}
}