// <copyright file="IPanelContent.cs" company="HD">
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
// <summary>Represent general View Model for <see cref="INode"/>.</summary>

using System;
using Nfm.Core.Models;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Represent general View Model for <see cref="INode"/>.
	/// </summary>
	public interface IPanelContent : ICloneable
	{
		/// <summary>
		/// Gets or sets parent host panel.
		/// </summary>
		IPanelContentHost Host { get; set; }
	}
}