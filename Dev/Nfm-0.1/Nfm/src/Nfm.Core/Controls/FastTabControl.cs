// <copyright file="FastTabControl.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Eric Burke">
//	<url>http://eric.burke.name/dotnetmania/2009/04/26/22.09.28</url>
//	<url2>http://groups.google.com/group/wpf-disciples/browse_thread/thread/c0c65d28612da0e6/a5af73f63716e1a8</url2>
// 	<date>2009-04-26</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-05-02</date>
// </editor>
// <summary>
// <see cref="TabControl"/> with performance tweaks:
// tab content is created when corresponding tab item is selected in the first time
// and collapse when selected tab index changed to another tab; when selected index back to collapsed tab - its just expand
// instead of destroying and recreate tab content each time when selected tab changed.
// </summary>

using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Nfm.Core.Controls
{
	/// <summary>
	/// <see cref="TabControl"/> with performance tweaks:
	/// tab content is created when corresponding tab item is selected in the first time
	/// and collapse when selected tab index changed to another tab; when selected index back to collapsed tab - its just expand
	/// instead of destroying and recreate tab content each time when selected tab changed.
	/// </summary>
	[TemplatePart(Name = "PART_ItemsHolder", Type = typeof (Panel))]
	public class FastTabControl : TabControl
	{
		/// <summary>
		/// <see cref="Panel"/> with tab items.
		/// </summary>
		private Panel itemsHolder;

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="FastTabControl"/> class.
		/// </summary>
		public FastTabControl()
		{
			// this is necessary so that we get the initial databound selected item
			ItemContainerGenerator.StatusChanged += ItemContainerGeneratorStatusChanged;
		}

		/// <summary>
		/// Initializes static members of the <see cref="FastTabControl"/> class.
		/// </summary>
		static FastTabControl()
		{
			// TODO: Consiter using this default static ctor with such body:
//			DefaultStyleKeyProperty.OverrideMetadata(
//				typeof (FastTabControl), new FrameworkPropertyMetadata(typeof (FastTabControl)));
		}

		#endregion

		#region Overrides from TabControl

		/// <summary>
		/// Get the ItemsHolder and generate any children.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			itemsHolder = GetTemplateChild("PART_ItemsHolder") as Panel;
			UpdateSelectedItem();
		}

		/// <summary>
		/// When the items change we remove any generated panel children and add any new ones as necessary.
		/// </summary>
		/// <param name="e">Provides data for the <see cref="INotifyCollectionChanged.CollectionChanged"/> event.</param>
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);

			if (itemsHolder == null)
			{
				return;
			}

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Reset:
					itemsHolder.Children.Clear();
					break;

				case NotifyCollectionChangedAction.Add:
				case NotifyCollectionChangedAction.Remove:
					if (e.OldItems != null)
					{
						foreach (object item in e.OldItems)
						{
							ContentPresenter cp = FindChildContentPresenter(item);
							if (cp != null)
							{
								itemsHolder.Children.Remove(cp);
							}
						}
					}

					// don't do anything with new items because we don't want to
					// create visuals that aren't being shown

					UpdateSelectedItem();
					break;

				case NotifyCollectionChangedAction.Replace:
					break;
					// TODO: add TabControl.Items "replace" action.
//					throw new NotImplementedException("Replace not implemented yet.");
			}
		}

		/// <summary>
		/// Update the visible child in the ItemsHolder.
		/// </summary>
		/// <param name="e">Provides data for the <see cref="Selector.SelectionChanged"/> event.</param>
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);
			UpdateSelectedItem();
		}

		#endregion

		#region Private Implementation

		/// <summary>
		/// Copied from <see cref="TabControl"/>; wish it were protected in that class instead of private.
		/// </summary>
		/// <returns>Selected <see cref="TabItem"/>.</returns>
		protected TabItem GetSelectedTabItem()
		{
			object selectedItem = SelectedItem;

			if (selectedItem == null)
			{
				return null;
			}

			return selectedItem as TabItem ?? ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;
		}

		/// <summary>
		/// Generate a <see cref="ContentPresenter"/> for the selected item.
		/// </summary>
		private void UpdateSelectedItem()
		{
			if (itemsHolder == null)
			{
				return;
			}

			// generate a ContentPresenter if necessary
			TabItem item = GetSelectedTabItem();

			if (item != null)
			{
				CreateChildContentPresenter(item);
			}

			// show the right child
			foreach (ContentPresenter child in itemsHolder.Children)
			{
				child.Visibility = ((TabItem) child.Tag).IsSelected
				                   	? Visibility.Visible
				                   	: Visibility.Collapsed;
			}
		}

		/// <summary>
		/// Create the child <see cref="ContentPresenter"/> for the given item.
		/// </summary>
		/// <param name="item"><see cref="TabItem"/> or data item.</param>
		/// <returns>Generated <see cref="ContentPresenter"/>.</returns>
		private ContentPresenter CreateChildContentPresenter(object item)
		{
			if (item == null)
			{
				return null;
			}

			ContentPresenter cp = FindChildContentPresenter(item);

			if (cp != null)
			{
				return cp;
			}

			// the actual child to be added.  cp.Tag is a reference to the TabItem
			cp = new ContentPresenter
			     {
			     	Content = (item is TabItem)
			     	          	? (item as TabItem).Content
			     	          	: item,
			     	ContentTemplate = SelectedContentTemplate,
			     	ContentTemplateSelector = SelectedContentTemplateSelector,
			     	ContentStringFormat = SelectedContentStringFormat,
			     	Visibility = Visibility.Collapsed,
			     	Tag = (item is TabItem)
			     	      	? item
			     	      	: ItemContainerGenerator.ContainerFromItem(item)
			     };

			itemsHolder.Children.Add(cp);

			return cp;
		}

		/// <summary>
		/// If containers are done, generate the selected item.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains no event data.</param>
		private void ItemContainerGeneratorStatusChanged(object sender, EventArgs e)
		{
			if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				ItemContainerGenerator.StatusChanged -= ItemContainerGeneratorStatusChanged;
				UpdateSelectedItem();
			}
		}

		/// <summary>
		/// Find the <see cref="ContentPresenter"/> for the given data item.
		/// </summary>
		/// <param name="item"><see cref="TabItem"/> or data item.</param>
		/// <returns>Found <see cref="ContentPresenter"/> or null otherwise.</returns>
		private ContentPresenter FindChildContentPresenter(object item)
		{
			if (item is TabItem)
			{
				item = (item as TabItem).Content;
			}

			if (item == null)
			{
				return null;
			}

			if (itemsHolder == null)
			{
				return null;
			}

			foreach (ContentPresenter cp in itemsHolder.Children)
			{
				if (cp.Content == item)
				{
					return cp;
				}
			}

			return null;
		}

		#endregion
	}
}