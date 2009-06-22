// <copyright file="KeyboardFocusManagerExtension.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-30</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-30</date>
// </editor>
// <summary>This fake markup extension allow set keyboard focus to specific <see cref="ListBoxItem"/>.</summary>

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Threading;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Controls
{
	/// <summary>
	/// This fake markup extension allow set keyboard focus to specific <see cref="ListBoxItem"/>.
	/// It is intended to be used with ElementUtility.Dummy attached property because it does't provide any real value:
	/// <![CDATA[
	///		<ListBox
	///			ElementUtility.Dummy="{KeyboardFocusManager}" />
	/// ]]>
	/// </summary>
	public class KeyboardFocusManagerExtension : MarkupExtension
	{
		#region Private Data

		/// <summary>
		/// Gets or sets current item index to set focused on.
		/// </summary>
		private int ItemIndex { get; set; }

		/// <summary>
		/// Gets or sets ListBox with items.
		/// </summary>
		private ListBox ListBox { get; set; }

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyboardFocusManagerExtension"/> class.
		/// </summary>
		public KeyboardFocusManagerExtension()
		{
			// Preventing from setting focus by default
			ItemIndex = -1;
		}

		#endregion

		#region Overrides of MarkupExtension

		/// <summary>
		/// Hock specific hanlders to several <see cref="ListBox"/> events
		/// to be able locates specific <see cref="ListBoxItem"/> by index and set keyboard focus to it.
		/// </summary>
		/// <param name="serviceProvider"><see cref="IServiceProvider"/> from XAML.</param>
		/// <returns>Null, because of markup extension fake nature.</returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			// Ignore if in design mode
			if ((bool) DesignerProperties
			           	.IsInDesignModeProperty
			           	.GetMetadata(typeof (DependencyObject)).DefaultValue)
			{
				return null;
			}

			// Get the IProvideValue interface which gives us access to the target property 
			// and object.  Note that MSDN calls this interface "internal" but it's necessary
			// here because we need to know what property we are assigning to.
			var pvt = serviceProvider.GetService(typeof (IProvideValueTarget)) as IProvideValueTarget;

			if (pvt != null)
			{
				// Note: Important check for System.Windows.SharedDp,
				// when using markup extension inside of ControlTemplates and DataTemplates.
				// Details: http://social.msdn.microsoft.com/forums/en-US/wpf/thread/a9ead3d5-a4e4-4f9c-b507-b7a7d530c6a9/
				if (!(pvt.TargetObject is DependencyObject))
				{
					return this;
				}

				ListBox = pvt.TargetObject as ListBox;

				if (ListBox != null)
				{
					AttachDataContextChangedHandler();
				}
			}

			return null;
		}

		#endregion

		/// <summary>
		/// Attach event handler to <see cref="FrameworkElement.DataContextChanged"/> event
		/// of current <see cref="ListBox"/> element.
		/// </summary>
		private void AttachDataContextChangedHandler()
		{
			ListBox.DataContextChanged -= OnDataContextChanged;

			// Wait until the element is initialised
			if (!ListBox.IsInitialized)
			{
				EventHandler callback = null;

				callback = delegate
				           {
				           	ListBox.Initialized -= callback;
				           	AttachDataContextChangedHandler();
				           };

				ListBox.Initialized += callback;
				return;
			}

			ListBox.DataContextChanged += OnDataContextChanged;

			AttachItemGeneratorStatusChangedHandler((IViewModelWithChilds) ListBox.DataContext);
		}

		/// <summary>
		/// <see cref="FrameworkElement.DataContextChanged"/> event handler.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="eventArgs">The event data.</param>
		private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs eventArgs)
		{
			if (eventArgs.NewValue != null)
			{
				AttachItemGeneratorStatusChangedHandler((IViewModelWithChilds) eventArgs.NewValue);
			}
		}

		/// <summary>
		/// Attach event handler to <see cref="ItemContainerGenerator.StatusChanged"/> event
		/// of current <see cref="ListBox"/> element.
		/// </summary>
		/// <param name="viewModel">Corresponding <see cref="IViewModelWithChilds"/> instance with current item index.</param>
		private void AttachItemGeneratorStatusChangedHandler(IViewModelWithChilds viewModel)
		{
			if (viewModel != null)
			{
				ItemIndex = viewModel.CurrentItemIndex;
			}

			ItemContainerGenerator itemGenerator = ListBox.ItemContainerGenerator;
			itemGenerator.StatusChanged += OnItemGeneratorStatusChanged;
		}

		/// <summary>
		/// <see cref="ItemContainerGenerator.StatusChanged"/> event handler.
		/// </summary>
		/// <param name="sender"><see cref="ItemContainerGenerator"/> instance.</param>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains no event data.</param>
		private void OnItemGeneratorStatusChanged(object sender, EventArgs e)
		{
			var generator = (ItemContainerGenerator) sender;

			if (generator.Status == GeneratorStatus.ContainersGenerated)
			{
//				generator.StatusChanged -= OnItemGeneratorStatusChanged;

				// Note: Important workaround of WPF bug in ListBox with group style:
				// This seems to be a bug in WPF, when one has a GroupStyle set.
				// The solution is to punt the access of the ItemGenerator after the rendering process.
				// Details: http://stackoverflow.com/questions/165424/how-does-itemcontainergenerator-containerfromitem-work-with-a-grouped-list

				// Additional from 78: with DispatcherPriority.Input (5) and high keyboard input activity
				// some times delegate was called to late when visual tree already changed and no one items in ListBox exist,
				// but with DispatcherPriority.Loaded (6) this was never happens.
				// Note: DispatcherPriority.Render has value (7).
				Application.Current.Dispatcher.BeginInvoke(
					DispatcherPriority.Loaded, new Action<ItemContainerGenerator>(DelayedAction), generator);
			}
		}

		/// <summary>
		/// Try to set keyboard focus on the first item in <see cref="ItemsControl"/>.
		/// </summary>
		/// <param name="generator"><see cref="ItemsControl"/> container generator to get first item from.</param>
		private void DelayedAction(ItemContainerGenerator generator)
		{
			if (0 <= ItemIndex)
			{
				var item = (IInputElement) generator.ContainerFromIndex(ItemIndex);

				if (item != null)
				{
					generator.StatusChanged -= OnItemGeneratorStatusChanged;

					// Logical focus
					//FocusManager.SetFocusedElement(ListBox, item);

					// Keyboard focus
					item.Focus();
				}
				else
				{
					// Note: With VirtualizingPanel if the ListBoxItem is "virtual", generator will return "null" instead of that item.
					ListBox.ScrollIntoView(ListBox.Items[ItemIndex]);
					Debug.WriteLine(
						string.Format(
							"Warning: ItemContainerGenerator of the ListBox with {0} items can't generate container for virtualized item #{1}.",
							ListBox.Items.Count,
							ItemIndex));
				}
			}
		}
	}
}