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
	[DebuggerDisplay("{GetType().Name}:"
					+ " {Header.Text}"
					+ " {IsSelected}"
					+ " {Childs.Count}"
					+ " Active={Active != null ? Active.Header.Text : null}"
					+ " Parent={Parent != null ? Parent.Header.Text : null}")]
	public class PanelContainerBase : NotificationBase, IPanelContainer
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
		/// Indicating whether a panel is selected.
		/// </summary>
		private bool isSelected;

		/// <summary>
		/// Gets or sets panel header.
		/// </summary>
		public IPanelHeader Header { get; set; }

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
		/// Gets or sets a value indicating whether a panel is selected.
		/// </summary>
		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				OnPropertyChanging("IsSelected");
				OnAction(SelectionChanging, this);
				isSelected = value;
				OnAction(SelectionChanged, this);
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
		/// Rased before panel is closed.
		/// </summary>
		public event EventHandler<EventArgs> Closing;

		/// <summary>
		/// Rased after panel is closed.
		/// </summary>
		public event EventHandler<EventArgs> Closed;

		/// <summary>
		/// Rased before panel is selected.
		/// </summary>
		public event Action<IPanel> SelectionChanging;

		/// <summary>
		/// Rased after panel is selected.
		/// </summary>
		public event Action<IPanel> SelectionChanged;

		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public virtual object Clone()
		{
			return new PanelContainerBase(this);
		}

		#endregion

		#endregion

		#region Implementation of IPanelContainer

		/// <summary>
		/// Active child panel.
		/// </summary>
		private IPanel active;

		/// <summary>
		/// Child panels.
		/// </summary>
		private ObservableCollection<IPanel> childs;

		/// <summary>
		/// Specifing, when container is in forced closing state.
		/// </summary>
		private bool isClosing;

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
		/// Gets or sets active child panel.
		/// </summary>
		public IPanel Active
		{
			get { return active; }
			set
			{
				if (Childs.Contains(value))
				{
					OnPropertyChanging("Active");
					active = value;

					if (!active.IsSelected)
					{
						active.IsSelected = true;
					}

					OnPropertyChanged("Active");
				}
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
		protected void OnChildsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			// Note: NotifyCollectionChangedAction.Reset
			//    change types should not occur for ObservableCollection<IPanel>.

			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				// Todo: remove debug fail.
				Debug.Fail("NotifyCollectionChangedAction.Reset");
			}

			if (e.NewItems != null
			    && e.NewItems.Count > 0
			    && (e.Action == NotifyCollectionChangedAction.Add
			        || e.Action == NotifyCollectionChangedAction.Replace))
			{
				foreach (IPanel panel in e.NewItems)
				{
					// Take ownership of this child.
					panel.Parent = this;
					panel.Closed += OnChildClose;
					panel.SelectionChanged += OnChildSelectionChanged;

					// Initialize active panel with last added selected panel
					if (panel.IsSelected)
					{
						Active = panel;
					}
				}

				// Force selection of last added panel to initialize active panel
				if (Active == null)
				{
					((IPanel) e.NewItems[e.NewItems.Count - 1]).IsSelected = true;
				}
			}

			if (e.OldItems != null
			    && e.OldItems.Count > 0
			    && (e.Action == NotifyCollectionChangedAction.Remove
			        || e.Action == NotifyCollectionChangedAction.Replace))
			{
				foreach (IPanel panel in e.OldItems)
				{
					// Important: do NOT remove parent from NOT owned childs!
					// When child will be added at the same time to other collection,
					// that new collection will take ownership on this child.
					if (panel.Parent == this)
					{
						panel.Parent = null;
					}

					// But event handler still need to be removed.
					panel.Closed -= OnChildClose;
					panel.SelectionChanged -= OnChildSelectionChanged;
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
			var panel = sender as IPanel;

			if (Childs.Contains(panel))
			{
				int panelIndex = Childs.IndexOf(panel);
				Childs.Remove(panel);

				if (Active == panel && Childs.Count > 0)
				{
					// Todo: Make this configurable - after child panel is closed, make next OR prev child panel active.
					int newActiveIndex = panelIndex < Childs.Count
					                     	? panelIndex
					                     	: Childs.Count - 1;
					Active = Childs[newActiveIndex];
				}
			}

			if (Childs.Count == 0 && !isClosing)
			{
				active = null;
				RequestClose();
			}
		}

		/// <summary>
		/// Child <see cref="IPanel.SelectionChanged"/> event handler.
		/// </summary>
		/// <param name="panel">Event sender.</param>
		private void OnChildSelectionChanged(IPanel panel)
		{
			if (panel.IsSelected && panel != Active)
			{
				if (Active != null && Active.Parent == this)
				{
					Active.IsSelected = false;
				}

				Active = panel;
			}
		}

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="PanelContainerBase"/> class.
		/// </summary>
		public PanelContainerBase()
		{
			Header = new PanelHeader();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PanelContainerBase"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="PanelContainerBase"/> instance to copy data from.</param>
		protected PanelContainerBase(PanelContainerBase another)
		{
			Header = (IPanelHeader)another.Header.Clone();
			isSelected = another.isSelected;

			// Detach from parent panel
			//	Parent = null;

			// Remove original subscribiters
			//	Closing = null;	// -= Closing;
			//	Closed = null;	// -= Closed;

			// Deep copy all childs
			var childsCopy = new ObservableCollection<IPanel>();

			// Important: handler below must belong to "result" object and NOT to "this" object!
			// See defect #18.
			childsCopy.CollectionChanged += OnChildsChanged;

			foreach (IPanel child in another.childs)
			{
				var newChild = (IPanel) child.Clone();
				childsCopy.Add(newChild);

				if (child == another.Active)
				{
					active = newChild;
				}
			}

			childs = childsCopy;
		}

		#endregion
	}
}