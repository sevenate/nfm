// <copyright file="PanelContainerBase.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-10</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-10</date>
// </editor>
// <summary>Base <see cref="IPanelContainer"/> with childs close notifications.</summary>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Base <see cref="IPanelContainer"/> with childs close notifications.
	/// </summary>
	[DebuggerDisplay(
			@"Header = {Header}"
		+ @", Type = {GetType()}"
		+ @", Childs = {Childs.Count}")]
	public abstract class PanelContainerBase : NotificationBase, IPanelContainer
	{
		#region Implementation of IDisposable

		/// <summary>
		/// Forced object distruction.
		/// </summary>
		/// <param name="disposing">"True" for manual calls.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Release managed resources.
				Close(true);
			}

			// Release unmanaged resources.
			// Set large fields to null.
			// Call Dispose on your base class.
			base.Dispose(disposing);

			// The derived class does not have a Finalize method
			// or a Dispose method with parameters because it inherits
			// them from the base class.
		}

		#endregion

		#region Implementation of IPanel

		/// <summary>
		/// Gets or sets panel header: string text or complex content.
		/// </summary>
		public object Header { get; set; }

		/// <summary>
		/// Gets a value indicating whether a panel can be closed.
		/// </summary>
		public bool CanClose
		{
			get
			{
				foreach (IPanel child in Childs)
				{
					if (!child.CanClose)
					{
						return false;
					}
				}

				return true;
			}
		}

		/// <summary>
		/// Indicating whether a panel is selected.
		/// </summary>
		private bool isSelected;

		/// <summary>
		/// Gets or sets a value indicating whether a panel is selected.
		/// </summary>
		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				OnPropertyChanging("IsSelected");
				isSelected = value;
				OnPropertyChanged("IsSelected");
			}
		}

		/// <summary>
		/// Gets or sets parent <see cref="IPanel"/>.
		/// </summary>
		public IPanel Parent { get; set; }

		/// <summary>
		/// Request close action for panel.
		/// </summary>
		public void RequestClose()
		{
			isClosing = true;
			OnEvent(Closing, this);

			if (Childs.Count > 0)
			{
				Close(false);
			}

			isClosing = false;
			if (Childs.Count == 0)
			{
				OnEvent(Closed, this);
			}
			else
			{
				throw new Exception("Some child panels can not be closed.");
			}
		}

		/// <summary>
		/// Fire when panel is closed.
		/// </summary>
		public virtual event EventHandler<EventArgs> Closed;

		/// <summary>
		/// Fire when panel is intended to close.
		/// </summary>
		public virtual event EventHandler<EventArgs> Closing;

		#endregion

		#region Implementation of IPanelContainer

		/// <summary>
		/// Specifing, when container is in forced closing state.
		/// </summary>
		private bool isClosing;

		/// <summary>
		/// Child panels.
		/// </summary>
		private ObservableCollection<IPanel> childs;

		/// <summary>
		/// Gets all child panels.
		/// </summary>
		public ObservableCollection<IPanel> Childs
		{
			get
			{
				if (childs == null)
				{
					OnPropertyChanging("Childs");
					childs = new ObservableCollection<IPanel>();
					childs.CollectionChanged += OnChildsChanged;
					OnPropertyChanged("Childs");
				}

				return childs;
			}
		}

		/// <summary>
		/// Close panel and all childs panels.
		/// </summary>
		/// <param name="force">"True" to force close action.</param>
		private void Close(bool force)
		{
			IList<IPanel> closingPanels = new List<IPanel>(Childs);

			foreach (IPanel panel in closingPanels)
			{
				if (panel != null && (force || panel.CanClose))
				{
					panel.RequestClose();
				}
			}

			closingPanels.Clear();
		}

		/// <summary>
		/// Childs panels <see cref="ObservableCollection{T}.CollectionChanged"/> event handler.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event params.</param>
		private void OnChildsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null && e.NewItems.Count != 0)
			{
				foreach (IPanel panel in e.NewItems)
				{
					panel.Closed += OnChildClose;
				}
			}

			if (e.OldItems != null && e.OldItems.Count != 0)
			{
				foreach (IPanel panel in e.OldItems)
				{
					panel.Closed -= OnChildClose;
				}
			}
		}

		/// <summary>
		/// Child <see cref="IPanel.Closed"/> event handler.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event params.</param>
		private void OnChildClose(object sender, EventArgs e)
		{
			Childs.Remove(sender as IPanel);

			if (Childs.Count == 0 && !isClosing)
			{
				RequestClose();
			}
		}

		#endregion
	}
}