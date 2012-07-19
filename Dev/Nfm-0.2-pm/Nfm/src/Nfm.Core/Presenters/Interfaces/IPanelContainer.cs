// <copyright file="IPanelContainer.cs" company="HD">
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
// <summary>Represent a container for multiple <see cref="IPanel"/>.</summary>

using Caliburn.PresentationFramework.ApplicationModel;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Represent a container for multiple <see cref="IPanel"/>.
	/// </summary>
	public interface IPanelContainer : IPresenterManager, IPanel
	{
	}
}