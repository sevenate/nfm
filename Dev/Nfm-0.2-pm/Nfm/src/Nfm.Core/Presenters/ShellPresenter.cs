// <copyright file="ShellPresenter.cs" company="HD">
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
// <summary>Application main window presenter.</summary>

using System;
using Caliburn.Core.Metadata;
using Caliburn.PresentationFramework.ApplicationModel;
using Microsoft.Practices.ServiceLocation;
using Nfm.Core.Models;
using Nfm.Core.Presenters.Interfaces;

namespace Nfm.Core.Presenters
{
	/// <summary>
	/// Application main window presenter.
	/// </summary>
	[Singleton(typeof(IShellPresenter))]
	public class ShellPresenter : Presenter, IShellPresenter
	{
		#region Fields

		/// <summary>
		/// Service locator.
		/// </summary>
		private readonly IServiceLocator serviceLocator;

		/// <summary>
		/// Modal dialog application model.
		/// </summary>
		private IPresenter dialogModel;

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ShellPresenter"/> class.
		/// </summary>
		/// <param name="serviceLocator">Service locator to use.</param>
		public ShellPresenter(IServiceLocator serviceLocator)
		{
			this.serviceLocator = serviceLocator;
		}

		#endregion

		/// <summary>
		/// Gets or sets dialog presentation model.
		/// </summary>
		public IPresenter DialogModel
		{
			get { return dialogModel; }
			set
			{
				dialogModel = value;
				NotifyOfPropertyChange("DialogModel");
			}
		}

		#region Implementation of IShellPresenter

		/// <summary>
		/// Show specific presentation model as "modal dialog" overlay on top of currently opened presentation model.
		/// </summary>
		/// <typeparam name="T">Specific <see cref="IPresenter"/>.</typeparam>
		/// <param name="presenter">Specific "modal dialog" to show.</param>
		public void ShowDialog<T>(T presenter) where T : IPresenter, ILifecycleNotifier
		{
			presenter.WasShutdown += delegate { DialogModel = null; };
			DialogModel = presenter;
		}

		#endregion

		/// <summary>
		/// Inheritors should override this method if they intend to handle advanced shutdown scenarios.
		/// </summary>
		/// <param name="model">The model.</param><param name="completed">Called when the shutdown model is finished executing.</param>
		protected override void ExecuteShutdownModel(ISubordinate model, Action completed)
		{
			var dialogPresenter = serviceLocator.GetInstance<IQuestionPresenter>();

			Question question = model as Question
								?? new Question(this, Answer.No, "Shutdown application?", Answer.Yes, Answer.No);

			dialogPresenter.Setup(question, completed);
			ShowDialog(dialogPresenter);
		}

		#region Overrides of PresenterBase

		/// <summary>
		/// Determines whether this instance can shutdown.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can shutdown; otherwise, <c>false</c>.
		/// </returns>
		public override bool CanShutdown()
		{
			// TODO: Make exit confirmation configurable!
			return false;
		}

		#endregion

		#region Implementation of ISupportCustomShutdown

		/// <summary>
		/// Creates the shutdown model.
		/// </summary>
		/// <returns/>
		public ISubordinate CreateShutdownModel()
		{
			return new Question(this, Answer.No, "Are you sure to exit?", Answer.Yes, Answer.No);
		}

		/// <summary>
		/// Determines whether this instance can shutdown based on the evaluated shutdown model.
		/// </summary>
		/// <param name="shutdownModel">The shutdown model.</param>
		/// <returns>
		/// <c>true</c> if this instance can shutdown; otherwise, <c>false</c>.
		/// </returns>
		public bool CanShutdown(ISubordinate shutdownModel)
		{
			var question = (Question)shutdownModel;
			return question.Answer == Answer.Yes;
		}

		#endregion
	}
}