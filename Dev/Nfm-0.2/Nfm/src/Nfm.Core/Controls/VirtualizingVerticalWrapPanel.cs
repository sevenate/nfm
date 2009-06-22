// <copyright file="VirtualizingVerticalWrapPanel.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Mike Taulty">
//	<url>http://mtaulty.com/CommunityServer/blogs/mike_taultys_blog/archive/2006/06/17/5856.aspx</url>
// 	<date>2006-06-17</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-05-04</date>
// </editor>
// <summary>
//  Provides extended support for the ListBox and ListView controls.
// </summary>

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Nfm.Core.Controls
{
	public class VirtualizingVerticalWrapPanel : VirtualizingPanel, IScrollInfo
	{
		private readonly Size maxSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
		private bool canVScroll;
		private Size extent;
		private double itemHeight;
		private double itemWidth;
		private TranslateTransform renderTransform;
		private ScrollViewer scrollOwner;
		private double verticalOffset;
		private Size viewport;

		public VirtualizingVerticalWrapPanel()
		{
			renderTransform = new TranslateTransform();
			RenderTransform = renderTransform;
		}

		private int ItemsPerLine
		{
			get { return ((int) (viewport.Width/itemWidth)); }
		}

		private int FirstItemVisibleInViewport
		{
			get
			{
				var firstItem =
					(int) ((verticalOffset/itemHeight)*ItemsPerLine);

				return (firstItem);
			}
		}

		private int LastItemVisibleInViewport
		{
			get
			{
				double lines = (viewport.Height/itemHeight);
				double items = lines*ItemsPerLine;

				int lastItem = FirstItemVisibleInViewport +
				               (int) Math.Ceiling(items);

				lastItem--;

				lastItem = Math.Min(lastItem, ItemCount - 1);

				return (lastItem);
			}
		}

		private int ItemCount
		{
			get
			{
				int count = 0;

				ItemsControl itemsControl = ItemsControl.GetItemsOwner(this);

				if ((itemsControl != null) && (itemsControl.Items != null))
				{
					count = itemsControl.Items.Count;
				}
				return (count);
			}
		}

		#region IScrollInfo Members

		public bool CanHorizontallyScroll
		{
			get { return (false); }
			set { }
		}

		public bool CanVerticallyScroll
		{
			get { return (canVScroll); }
			set { canVScroll = value; }
		}

		public double ExtentHeight
		{
			get { return (extent.Height); }
		}

		public double ExtentWidth
		{
			get { return (extent.Width); }
		}

		public double HorizontalOffset
		{
			get { return (0.0d); }
		}

		public void LineDown()
		{
			SetVerticalOffset(verticalOffset + 1);
		}

		public void LineLeft()
		{
		}

		public void LineRight()
		{
		}

		public void LineUp()
		{
			SetVerticalOffset(verticalOffset - 1);
		}

		public Rect MakeVisible(
			Visual visual, Rect rectangle)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void MouseWheelDown()
		{
			SetVerticalOffset(verticalOffset + itemHeight);
		}

		public void MouseWheelLeft()
		{
		}

		public void MouseWheelRight()
		{
		}

		public void MouseWheelUp()
		{
			SetVerticalOffset(verticalOffset - itemHeight);
		}

		public void PageDown()
		{
			SetVerticalOffset(verticalOffset + itemHeight);
		}

		public void PageLeft()
		{
		}

		public void PageRight()
		{
		}

		public void PageUp()
		{
			SetVerticalOffset(verticalOffset - itemHeight);
		}

		public ScrollViewer ScrollOwner
		{
			get { return (scrollOwner); }
			set { scrollOwner = value; }
		}

		public void SetHorizontalOffset(double offset)
		{
			// We don't horizontally scroll
		}

		public void SetVerticalOffset(double offset)
		{
			offset = Math.Max(0.0d, offset);

			offset = Math.Min(
				extent.Height - viewport.Height,
				offset);

			verticalOffset = offset;

			renderTransform.Y = 0 - verticalOffset;

			RealizeVisibleItems();

			if (scrollOwner != null)
			{
				scrollOwner.InvalidateScrollInfo();
			}
		}

		public double VerticalOffset
		{
			get { return (verticalOffset); }
		}

		public double ViewportHeight
		{
			get { return (viewport.Height); }
		}

		public double ViewportWidth
		{
			get { return (viewport.Width); }
		}

		#endregion

		protected override Size MeasureOverride(
			Size constraint)
		{
			// How big an area have we got to play with?
			bool invalidate = (viewport != constraint);
			viewport = constraint;

			// How big would things like to be? Note - this is "rough"
			// because it changes when we animate the current picture.
			DetermineStandardItemSize();

			// How big is our entire area then?
			int itemsPerLine = ItemsPerLine;

			var newExtent = new Size(
				constraint.Width,
				Math.Ceiling(ItemCount/(double) itemsPerLine)*itemHeight);

			invalidate = (invalidate) || (newExtent != extent);
			extent = newExtent;

			RealizeVisibleItems();

			foreach (UIElement element in InternalChildren)
			{
				element.Measure(maxSize);
			}

			if (invalidate && (scrollOwner != null))
			{
				scrollOwner.InvalidateScrollInfo();
			}
			return (constraint);
		}

		private void RealizeVisibleItems()
		{
			IItemContainerGenerator generator = ItemContainerGenerator;

			int i = 0;

			for (i = FirstItemVisibleInViewport;
			     i <= LastItemVisibleInViewport;
			     i++)
			{
				GeneratorPosition pos = generator.GeneratorPositionFromIndex(i);

				// Should be visible
				if (pos.Offset != 0)
				{
					// Virtualised so we need to fix.
					using (generator.StartAt(pos, GeneratorDirection.Forward))
					{
						var element = generator.GenerateNext() as UIElement;
						generator.PrepareItemContainer(element);

						pos = generator.GeneratorPositionFromIndex(i);

						InsertInternalChild(pos.Index, element);
					}
				}
			}
			i = FirstItemVisibleInViewport - 1;

			while (i >= 0)
			{
				GeneratorPosition pos = generator.GeneratorPositionFromIndex(i);

				if (pos.Offset == 0)
				{
					RemoveInternalChildRange(pos.Index, 1);
					generator.Remove(pos, 1);
				}
				else
				{
					break;
				}
				i--;
			}

			i = LastItemVisibleInViewport + 1;

			while (i < ItemCount)
			{
				GeneratorPosition pos = generator.GeneratorPositionFromIndex(i);

				if (pos.Offset == 0)
				{
					RemoveInternalChildRange(pos.Index, 1);
					generator.Remove(pos, 1);
				}
				else
				{
					break;
				}
				i++;
			}
		}

		private void RealizeFirstItem()
		{
			IItemContainerGenerator generator = ItemContainerGenerator;
			GeneratorPosition pos = generator.GeneratorPositionFromIndex(0);

			using (generator.StartAt(pos, GeneratorDirection.Forward))
			{
				var element = generator.GenerateNext() as UIElement;

				generator.PrepareItemContainer(element);

				AddInternalChild(element);
			}
		}

		private void DetermineStandardItemSize()
		{
			if ((itemWidth == 0) || (itemHeight == 0))
			{
				if (InternalChildren.Count == 0)
				{
					RealizeFirstItem();
				}
				UIElement element = InternalChildren[0];
				element.Measure(maxSize);

				itemWidth = element.DesiredSize.Width;
				itemHeight = element.DesiredSize.Height;
			}
		}

		protected override Size ArrangeOverride(
			Size finalSize)
		{
			double xPos = 0.0d;
			double yPos = verticalOffset;
			double maxHeightOnLine = 0.0d;

			foreach (UIElement element in InternalChildren)
			{
				var itemSize = new Rect(element.DesiredSize);

				if ((xPos + itemSize.Width) > finalSize.Width)
				{
					xPos = 0.0d;
					yPos = yPos + maxHeightOnLine;
					maxHeightOnLine = 0.0d;
				}
				element.Arrange(
					new Rect(
						xPos,
						yPos,
						itemSize.Width,
						itemSize.Height));

				maxHeightOnLine = Math.Max(maxHeightOnLine, itemSize.Height);

				xPos += itemSize.Width;
			}
			return (finalSize);
		}
	}
}