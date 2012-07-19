// <copyright file="IQuestionPresenter.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-07-06</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-07-06</date>
// </editor>
// <summary>Speficy common modal dialog interface.</summary>

using System;
using Caliburn.PresentationFramework.ApplicationModel;
using Nfm.Core.Models;

namespace Nfm.Core.Presenters.Interfaces
{
	/// <summary>
	/// Speficy common question dialog interface.
	/// </summary>
	public interface IQuestionPresenter : IPresenter, ILifecycleNotifier
	{
		/// <summary>
		/// Setup question dialog.
		/// </summary>
		/// <param name="question">Question to show.</param>
		/// <param name="completed">Callback on complete.</param>
		void Setup(Question question, Action completed);
	}
}