// <copyright file="QuestionPresenter.cs" company="HD">
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
// <summary>Common modal dialog.</summary>

using System;
using Caliburn.Core.Metadata;
using Caliburn.PresentationFramework.ApplicationModel;
using Nfm.Core.Models;
using Nfm.Core.Presenters.Interfaces;

namespace Nfm.Core.Presenters
{
	/// <summary>
	/// Common modal dialog.
	/// </summary>
	[PerRequest(typeof(IQuestionPresenter))]
	public class QuestionPresenter : Presenter, IQuestionPresenter
	{
		/// <summary>
		/// Complete callback.
		/// </summary>
		private Action completeCallback;

		/// <summary>
		/// Gets question to show.
		/// </summary>
		public Question Question { get; private set; }

		#region Implementation of IQuestionPresenter

		/// <summary>
		/// Setup question dialog.
		/// </summary>
		/// <param name="question">Question to show.</param>
		/// <param name="completed">Callback on complete.</param>
		public void Setup(Question question, Action completed)
		{
			Question = question;
			completeCallback = completed;
		}

		#endregion

		#region Overrides of PresenterBase

		/// <summary>
		/// Called when [shutdown].
		/// </summary>
		protected override void OnShutdown()
		{
			base.OnShutdown();
			completeCallback();
		}

		#endregion
	}
}