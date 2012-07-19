// <copyright file="IShellPresenter.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-07-04</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-07-04</date>
// </editor>
// <summary>"Application main window" service.</summary>

using Caliburn.PresentationFramework.ApplicationModel;

namespace Nfm.Core.Presenters.Interfaces
{
	/// <summary>
	/// "Application main window" service.
	/// </summary>
	public interface IShellPresenter : IPresenter, ISupportCustomShutdown
	{
		/// <summary>
		/// Show specific presentation model as "modal dialog" overlay on top of currently opened presentation model.
		/// </summary>
		/// <typeparam name="T">Specific <see cref="IPresenter"/>.</typeparam>
		/// <param name="presenter">Specific "modal dialog" to show.</param>
		void ShowDialog<T>(T presenter) where T : IPresenter, ILifecycleNotifier;
	}
}