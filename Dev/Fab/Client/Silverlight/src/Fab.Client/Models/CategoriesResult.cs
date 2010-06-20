// <copyright file="CategoriesResult.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="alevshoff@hd.com" date="2010-04-12" />
// <summary>Load all categories for user result.</summary>

using System;
using Caliburn.PresentationFramework.RoutedMessaging;
using Fab.Client.MoneyServiceReference;

namespace Fab.Client.Models
{
	/// <summary>
	/// Load all categories for user result.
	/// </summary>
	public class CategoriesResult : IResult
	{
		private readonly Guid userId;

		public CategoriesResult(Guid userId)
		{
			this.userId = userId;
		}

		public Guid UserId
		{
			get { return userId; }
		}
		
		public Category[] Categories { get; set; }

		public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

		public void Execute(ResultExecutionContext context)
		{
			var proxy = new MoneyServiceClient();

			proxy.GetAllCategoriesCompleted += (s, e) =>
			{
				if (e.Error != null)
				{
					Caliburn.PresentationFramework.Invocation.Execute.OnUIThread(
						() => Completed(this, new ResultCompletionEventArgs { Error = e.Error }));
				}
				else
				{
					Categories = e.Result;
					Caliburn.PresentationFramework.Invocation.Execute.OnUIThread(() => Completed(this, new ResultCompletionEventArgs()));
				}
			};

			proxy.GetAllCategoriesAsync(userId: userId);
		}
	}
}