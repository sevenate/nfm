// <copyright file="ListBoxExt.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Alex Shred">
//	<url>http://alexshed.spaces.live.com/blog/cns!71C72270309CE838!149.entry</url>
// 	<date>2008-05-07</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-05-03</date>
// </editor>
// <summary>Provides extended selection support for the <see cref="ListBox"/> and <see cref="ListView"/> controls.</summary>

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Nfm.Core.Controls
{
	/// <summary>
	/// Provides extended selection support for the <see cref="ListBox"/> and <see cref="ListView"/> controls.
	/// </summary>
	public static class ListBoxExt
	{
		#region SelectedItemsSource Attached Property

		/// <summary>
		/// SelectedItemsSource attached property, that contains the values that should be selected.
		/// </summary>
		public static readonly DependencyProperty SelectedItemsSourceProperty =
			DependencyProperty.RegisterAttached(
				"SelectedItemsSource",
				typeof (IList),
				typeof (ListBoxExt),
				new PropertyMetadata(
					null,
					OnSelectedItemsSourceChanged));

		/// <summary>
		/// Sets the <see cref="SelectedItemsSourceProperty"/> attached property value to target dependency object.
		/// </summary>
		/// <param name="element">The ListBox being set.</param>
		/// <param name="value">The items to be selected.</param>
		public static void SetSelectedItemsSource(DependencyObject element, IList value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			element.SetValue(SelectedItemsSourceProperty, value);
		}

		/// <summary>
		/// Gets the <see cref="SelectedItemsSourceProperty"/> attached property value from target dependency object.
		/// </summary>
		/// <param name="element">The ListBox to check.</param>
		/// <returns>Current attached property effective value: <see cref="IList"/> with selected values.</returns>
		public static IList GetSelectedItemsSource(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			return (IList) element.GetValue(SelectedItemsSourceProperty);
		}

		/// <summary>
		/// <see cref="SelectedItemsSourceProperty"/> attached property value changed callback handler.
		/// </summary>
		/// <param name="d">Attached property target.</param>
		/// <param name="e">Provides data for attached property changed event.</param>
		private static void OnSelectedItemsSourceChanged(
			DependencyObject d,
			DependencyPropertyChangedEventArgs e)
		{
			var listBox = d as ListBox;

			if (listBox == null)
			{
				throw new InvalidOperationException(
					"The ListBoxExt.SelectedItemsSource attached " +
					"property can only be applied to ListBox controls.");
			}

			listBox.SelectionChanged -= OnListBoxSelectionChanged;

			if (e.NewValue != null)
			{
				ListenForChanges(listBox);
			}
		}

		#endregion

		#region IsResynching attached property

		/// <summary>
		/// Used to set a flag on the <see cref="ListBox"/> to avoid reentry of SelectionChanged due to a full syncronisation pass.
		/// </summary>
		private static readonly DependencyPropertyKey IsResynchingPropertyKey =
			DependencyProperty.RegisterAttachedReadOnly(
				"IsResynching",
				typeof (bool),
				typeof (ListBoxExt),
				new PropertyMetadata(false));

		#endregion

		#region Implementation

		/// <summary>
		/// Attach <see cref="Selector.SelectionChanged"/> event handler to <see cref="ListBox"/>
		/// and resynchronize selected items in first time.
		/// </summary>
		/// <param name="listBox"><see cref="ListBox"/> to listen for changes.</param>
		private static void ListenForChanges(ListBox listBox)
		{
			// Wait until the element is initialised
			if (!listBox.IsInitialized)
			{
				EventHandler callback = null;

				callback = delegate
				           {
				           	listBox.Initialized -= callback;
				           	ListenForChanges(listBox);
				           };

				listBox.Initialized += callback;
				return;
			}

			listBox.SelectionChanged += OnListBoxSelectionChanged;
			ResynchList(listBox);
		}

		/// <summary>
		/// <see cref="Selector.SelectionChanged"/> routed event handler.
		/// </summary>
		/// <param name="sender"><see cref="ListBox"/> where the event handler is attached.</param>
		/// <param name="e">The event data.</param>
		private static void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var listBox = sender as ListBox;

			if (listBox != null)
			{
				var isResynching = (bool) listBox.GetValue(IsResynchingPropertyKey.DependencyProperty);

				if (isResynching)
				{
					return;
				}

				IList list = GetSelectedItemsSource(listBox);

				if (list != null)
				{
					foreach (object obj in e.RemovedItems)
					{
						if (list.Contains(obj))
						{
							list.Remove(obj);
						}
					}

					foreach (object obj in e.AddedItems)
					{
						if (!list.Contains(obj))
						{
							list.Add(obj);
						}
					}
				}

				// A little modification from Domingos allows for TwoWay binding
				BindingExpression bexp = listBox.GetBindingExpression(SelectedItemsSourceProperty);
				
				if (bexp != null)
				{
					bexp.UpdateSource();
				}
			}
		}

		/// <summary>
		/// Resynchronize selected data items with selected <see cref="ListBox"/> items.
		/// </summary>
		/// <param name="listBox"><see cref="ListBox"/> with selected items.</param>
		private static void ResynchList(ListBox listBox)
		{
			if (listBox != null)
			{
				listBox.SetValue(IsResynchingPropertyKey, true);
				IList list = GetSelectedItemsSource(listBox);

				if (listBox.SelectionMode == SelectionMode.Single)
				{
					listBox.SelectedItem = null;

					if (list != null)
					{
						if (list.Count > 1)
						{
							// There is more than one item selected, but the listbox is in Single selection mode
							throw new InvalidOperationException(
								"ListBox is in Single selection mode, but was given more than one selected value.");
						}

						if (list.Count == 1)
						{
							listBox.SelectedItem = list[0];
						}
					}
				}
				else
				{
					listBox.SelectedItems.Clear();

					if (list != null)
					{
						foreach (object obj in listBox.Items)
						{
							if (list.Contains(obj))
							{
								listBox.SelectedItems.Add(obj);
							}
						}
					}
				}

				listBox.SetValue(IsResynchingPropertyKey, false);
			}
		}

		#endregion
	}
}