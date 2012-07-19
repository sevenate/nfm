// <copyright file="IPanel.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-29</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-29</date>
// </editor>
// <summary>Represent general info panel.</summary>

using System;
using Caliburn.PresentationFramework.ApplicationModel;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Represent general info panel.
	/// </summary>
	public interface IPanel : IPresenter, ILifecycleNotifier, ICloneable
	{
		/// <summary>
		/// Gets panel header.
		/// </summary>
		IPanelHeader Header { get; }

		/// <summary>
		/// Gets or sets parent <see cref="IPanel"/>.
		/// </summary>
		IPanel Parent { get; set; }
	}
}