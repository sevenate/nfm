using System;
using System.Linq;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.PresentationFramework.Screens;
using Caliburn.PresentationFramework.RoutedMessaging;
using Caliburn.ShellFramework.History;
using Caliburn.ShellFramework.Questions;
using Caliburn.ShellFramework.Results;
using Fab.Client.Models;

namespace Fab.Client.Shell.ViewModels
{
	public class ShellViewModel : ScreenConductor<IScreen>, IShell
	{
		private readonly IHistoryCoordinator historyCoordinator;
		private readonly IObservableCollection<IPart> parts;

		public ShellViewModel(IHistoryCoordinator historyCoordinator, IPart[] parts)
		{
			this.historyCoordinator = historyCoordinator;
			this.parts = new BindableCollection<IPart>(parts);
		}

		public IObservableCollection<IPart> Parts
		{
			get { return parts; }
		}

		protected override void OnInitialize()
		{
			historyCoordinator.Start(
				config =>
				{
					config.Host = this;
					config.HistoryKey = "Page";
					config.ScreenNotFound = HandleScreenNotFound;
				});

			base.OnInitialize();
		}

		protected override void OnActivate()
		{
			Parts.First().Enter().Execute();
			base.OnActivate();
		}

		private void HandleScreenNotFound(string historyKey)
		{
			if (string.IsNullOrEmpty(historyKey))
			{
				return;
			}

			var item = Parts.FirstOrDefault(x => x.DisplayName == historyKey);

			if (item != null)
			{
				item.Enter().Execute();
			}
			else
			{
				Show.MessageBox("Invalid Query String Parameter: " + historyKey).Execute();
			}
		}

		protected override void ExecuteShutdownModel(ISubordinate model, Action completed)
		{
			model.Execute(completed);
		}
	}
}