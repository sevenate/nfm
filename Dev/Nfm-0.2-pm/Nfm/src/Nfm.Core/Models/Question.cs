// <copyright file="Question.cs" company="HD">
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
// <summary>Application question to user, which require immediate answer (reaction from user side).</summary>

using System.Linq;
using Caliburn.Core;
using Caliburn.PresentationFramework.ApplicationModel;

namespace Nfm.Core.Models
{
	/// <summary>
	/// Application question to user, which require immediate answer (reaction from user side).
	/// </summary>
	public class Question : PropertyChangedBase, ISubordinate
	{
		#region Fields

		/// <summary>
		/// Selected answer to question.
		/// </summary>
		private Answer answer;

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="Question"/> class.
		/// </summary>
		/// <param name="master"><see cref="ISubordinate.Master"/> of this subordinate.</param>
		/// <param name="text">Question text.</param>
		public Question(IPresenter master, string text)
			: this(master, Answer.No, text, Answer.Yes, Answer.No)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Question"/> class.
		/// </summary>
		/// <param name="master"><see cref="ISubordinate.Master"/> of this subordinate.</param>
		/// <param name="text">Question text.</param>
		/// <param name="possibleAnswers">All possible answers to question.</param>
		public Question(IPresenter master, string text, params Answer[] possibleAnswers)
			: this(master, possibleAnswers.Length > 0 ? possibleAnswers[0] : Answer.No, text, possibleAnswers)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Question"/> class.
		/// </summary>
		/// <param name="master"><see cref="ISubordinate.Master"/> of this subordinate.</param>
		/// <param name="defaultAnswer">Default answer.</param>
		/// <param name="text">Question text.</param>
		/// <param name="possibleAnswers">All possible answers to question.</param>
		public Question(IPresenter master, Answer defaultAnswer, string text, params Answer[] possibleAnswers)
		{
			Master = master;
			Text = text;
			PossibleAnswers = new BindableEnumCollection<Answer>(possibleAnswers);
			answer = possibleAnswers.Contains(defaultAnswer) ? defaultAnswer : Answer.No;
		}

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Gets question text.
		/// </summary>
		public string Text { get; private set; }

		/// <summary>
		/// Gets or sets all possible answers to question.
		/// </summary>
		public BindableEnumCollection<Answer> PossibleAnswers { get; set; }

		/// <summary>
		/// Gets or sets selected answer to question.
		/// </summary>
		public Answer Answer
		{
			get { return answer; }
			set
			{
				answer = value;
				NotifyOfPropertyChange("Answer");
			}
		}

		#endregion

		#region Implementation of ISubordinate

		/// <summary>
		/// Gets the <see cref="T:Caliburn.PresentationFramework.ApplicationModel.IPresenter"/> that owns this instance.
		/// </summary>
		/// <value>
		/// The master.
		/// </value>
		public IPresenter Master { get; private set; }

		#endregion
	}
}