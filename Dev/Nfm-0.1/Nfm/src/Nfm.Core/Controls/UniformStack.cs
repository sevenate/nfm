// <copyright file="UniformStack.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Filipe Fortes">
//	<url>http://fortes.com/2007/05/uniformpanel/</url>
// 	<date>2007-05-07</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-03-14</date>
// </editor>
// <summary>Layout panel which is a cross beween <see cref="UniformGrid"/> and <see cref="StackPanel"/>.</summary>

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Nfm.Core.Controls
{
	//TODO: add comments and make refactoring

	/// <summary>
	/// Layout panel which is a cross beween <see cref="UniformGrid"/> and <see cref="StackPanel"/>.
	/// </summary>
	public class UniformStack : Panel
	{
		#region Fields

		private readonly List<UIElement> fixedChildren = new List<UIElement>();
		private readonly List<UIElement> stretchyChildren = new List<UIElement>();
		private double fixedVHeight;
		private DependencyProperty vHeightProperty = HeightProperty;
		private DependencyProperty vWidthProperty = WidthProperty;

		#endregion

		#region Dependency Properties

		public static readonly DependencyProperty IsExpanderDetectionEnabledProperty =
			DependencyProperty.Register(
				"IsExpanderDetectionEnabled",
				typeof (bool),
				typeof (UniformStack),
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
			"Orientation",
			typeof (Orientation),
			typeof (UniformStack),
			new FrameworkPropertyMetadata(
				Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure, OnOrientationPropertyChanged));

		public Orientation Orientation
		{
			get { return (Orientation) GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		/* Update our virtual properties */

		public bool IsExpanderDetectionEnabled
		{
			get { return (bool) GetValue(IsExpanderDetectionEnabledProperty); }
			set { SetValue(IsExpanderDetectionEnabledProperty, value); }
		}

		private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var current = d as UniformStack;

			if (d == null)
			{
				return; // Shouldn't happen
			}

			bool isVertical = ((Orientation) e.NewValue == Orientation.Vertical);

			current.vWidthProperty = (isVertical) ? WidthProperty : HeightProperty;
			current.vHeightProperty = (isVertical) ? HeightProperty : WidthProperty;
		}

		#endregion

		#region Layout Overrides

		protected override Size MeasureOverride(Size availableSize)
		{
			double maxVWidth = 0;
			double totalVHeight = 0;
			Size vAvailableSize = VirtualSize(availableSize);
			fixedVHeight = 0;

			stretchyChildren.Clear();
			fixedChildren.Clear();

			foreach (UIElement child in Children)
			{
				if (HasFixedHeight(child))
				{
					fixedChildren.Add(child);

					// Measure the child with the incoming width and infinite height (since it will size to content)
					child.Measure(VirtualSize(new Size(vAvailableSize.Width, Double.PositiveInfinity)));
					maxVWidth = Math.Max(maxVWidth, VirtualSize(child.DesiredSize).Width);
					fixedVHeight += VirtualSize(child.DesiredSize).Height;
				}
				else
				{
					stretchyChildren.Add(child);

					// Hold off on Measuring until we know how much space is left over
				}
			}

			totalVHeight = fixedVHeight;

			if (stretchyChildren.Count > 0)
			{
				// Measure the stretchy children now 
				vAvailableSize.Height = Math.Max(0, vAvailableSize.Height - fixedVHeight); // Don't want to give negative numbers
				Size UniformMeasureSize = VirtualSize(new Size(vAvailableSize.Width, vAvailableSize.Height/stretchyChildren.Count));

				foreach (UIElement child in stretchyChildren)
				{
					child.Measure(UniformMeasureSize);
					totalVHeight += VirtualSize(child.DesiredSize).Height;
					maxVWidth = Math.Max(maxVWidth, VirtualSize(child.DesiredSize).Width);
				}
			}

			return VirtualSize(new Size(maxVWidth, totalVHeight));
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			double stretchyVHeight = Math.Max(0, (VirtualSize(finalSize).Height - fixedVHeight)/stretchyChildren.Count);
			// No negative numbers
			var stretchyArrangeVSize = new Size(VirtualSize(finalSize).Width, stretchyVHeight);

			double currVHeightOffset = 0;

			foreach (UIElement child in Children)
			{
				if (HasFixedHeight(child))
				{
					double childVHeight = VirtualSize(child.DesiredSize).Height;
					child.Arrange(
						VirtualRect(new Rect(new Point(0, currVHeightOffset), new Size(stretchyArrangeVSize.Width, childVHeight))));
					currVHeightOffset += childVHeight;
				}
				else
				{
					child.Arrange(VirtualRect(new Rect(new Point(0, currVHeightOffset), stretchyArrangeVSize)));
					currVHeightOffset += stretchyVHeight;
				}
			}

			return finalSize;
		}

		#endregion

		/* In order to share code paths, we use the notion of "virtual" widths and heights */

		#region Private Helpers

		/* Swaps width and height *only* if we're in virtual mode. Can be used to virtualize and de-virtualize a size */

		private Size VirtualSize(Size incoming)
		{
			if (Orientation == Orientation.Vertical)
			{
				return incoming;
			}

			return new Size(incoming.Height, incoming.Width);
		}

		private Rect VirtualRect(Rect incoming)
		{
			if (Orientation == Orientation.Vertical)
			{
				return incoming;
			}

			return new Rect(new Point(incoming.Top, incoming.Left), VirtualSize(incoming.Size));
		}

		/* Returns true if the element has a fixed height and should not be equi-sized */

		private bool HasFixedHeight(UIElement child)
		{
			if (!Double.IsNaN((double) child.GetValue(vHeightProperty)))
			{
				return true;
			}

			if (IsExpanderDetectionEnabled && !IsExpanded(child))
			{
				return true;
			}

			return false;
		}

		private bool IsExpanded(UIElement child)
		{
			Expander exp = FindExpander(child);

			if (exp == null)
			{
				return true; // No expander == expanded, I guess
			}

			return exp.IsExpanded;
		}

		private static Expander FindExpander(UIElement child)
		{
			if (child is Expander)
			{
				return (Expander) child;
			}

			// There's definitely a better way to do this, it shall wait for the next version
			// (this is why detection is off by default)
			while (VisualTreeHelper.GetChildrenCount(child) == 1)
			{
				// We go more than one level because you can have ContentPresenters, etc in between
				child = VisualTreeHelper.GetChild(child, 0) as UIElement;
				if (child is Expander)
				{
					return (Expander) child;
				}
			}

			return null;
		}

		#endregion
	}
}