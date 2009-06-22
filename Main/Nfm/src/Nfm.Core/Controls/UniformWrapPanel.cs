// <copyright file="UniformWrapPanel.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Alex Shred">
//	<url>http://alexshed.spaces.live.com/blog/cns!71C72270309CE838!168.entry</url>
// 	<date>2008-05-07</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-05-03</date>
// </editor>
// <summary>
//  A UniformWrapPanel behaves similar to an horizontal WrapPanel that places the items in a
//  "vertical first" approach and tries to keep all columns with the same number of items.
// </summary>

using System;
using System.Windows;
using System.Windows.Controls;

namespace Nfm.Core.Controls
{
	/// <summary>
	/// A UniformWrapPanel behaves similar to an horizontal WrapPanel that places the items in a 
	/// "vertical first" approach and tries to keep all columns with the same number of items.
	/// </summary>
	/// <remarks>
	/// If given Infinite width, the UniformWrapPanel will layout in a single column
	/// to avoid an horizontal scroll bar.
	/// </remarks>
	/// Bug: There's a slight problem, it returns invalid measurements when there are 0 items,
	/// makes the WPF designer freak out.
	public class UniformWrapPanel : Panel
	{
		#region Layout Related Fields 

		// The total columns used by the panel
		private int columns;

		// The total rows used by the panel

		// The number of columns that items in all the rows. Remaining columns do not
		// have an item in the last row.
		private int fullColumns;

		// The calculated width of the items

		// The calculated height of the items
		private double itemHeight;
		private double itemWidth;
		private int rows;

		#endregion

		#region Measure and Arrange Methods

		protected override Size MeasureOverride(Size constraint)
		{
			itemWidth = 0;
			itemHeight = 0;

			double width = constraint.Width;
			double height = constraint.Height;

			UIElementCollection internalChildren = InternalChildren;
			int count = internalChildren.Count;
			int current = 0;
			while (current < count)
			{
				UIElement element = internalChildren[current];
				if (element != null)
				{
					element.Measure(constraint);
					if (itemWidth < element.DesiredSize.Width)
					{
						itemWidth = element.DesiredSize.Width;
					}
					if (itemHeight < element.DesiredSize.Height)
					{
						itemHeight = element.DesiredSize.Height;
					}
				}
				current++;
			}

			if (count > 0)
			{
				columns = 1;
				if (!double.IsNaN(constraint.Width) && (itemWidth < constraint.Width))
				{
					columns = (int) Math.Floor(constraint.Width/itemWidth);
				}

				rows = (int) Math.Ceiling((double) count/columns);
				fullColumns = count - (columns*(rows - 1));

				if (!double.IsPositiveInfinity(width))
				{
					width = Math.Max(constraint.Width, columns*itemWidth);
				}

				if (double.IsPositiveInfinity(height))
				{
					height = rows*itemHeight;
				}
				else
				{
					height = Math.Max(constraint.Height, rows*itemHeight);
				}
			}

			return new Size(width, height);
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			int currentItem = 0;
			int currentRow = 0;
			int currentColumn = 0;

			UIElementCollection internalChildren = InternalChildren;
			int count = internalChildren.Count;
			while (currentItem < count)
			{
				UIElement element = internalChildren[currentItem];
				if (element != null)
				{
					var childArea = new Rect(
						currentColumn*itemWidth,
						currentRow*itemHeight,
						itemWidth,
						itemHeight);

					element.Arrange(childArea);

					currentRow++;

					if ((currentRow >= rows) ||
					    ((currentRow >= rows - 1) && (currentColumn >= fullColumns)))
					{
						currentRow = 0;
						currentColumn++;
					}
				}
				currentItem++;
			}
			return finalSize;
		}

		#endregion
	}
}