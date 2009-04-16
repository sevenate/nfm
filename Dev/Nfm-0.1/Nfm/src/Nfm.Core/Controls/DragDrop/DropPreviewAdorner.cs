// <copyright file="DropPreviewAdorner.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Pavan Podila">
//	<url>http://pavanpodila.spaces.live.com/blog/cns!9C9E888164859398!199.entry</url>
// 	<date>2006-11-20</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-16</date>
// </editor>
// <summary>Drag item preview adorner.</summary>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Nfm.Core.Controls.DragDrop
{
	/// <summary>
	/// Drag item preview adorner.
	/// </summary>
	public class DropPreviewAdorner : Adorner
	{
		#region Fields

		/// <summary>
		/// The element to bind the adorner to.
		/// </summary>
		private readonly UIElement adornedElement;

		/// <summary>
		/// A surface for rendering adorners.
		/// </summary>
		private readonly AdornerLayer adornerLayer;

		/// <summary>
		/// <see cref="ContentPresenter"/> what host additional drag item visual feedback element.
		/// </summary>
		private readonly ContentPresenter presenter;

		/// <summary>
		/// Offset from adorder layer left edge.
		/// </summary>
		private double left;
		
		/// <summary>
		/// Offset from adorder layer top edge.
		/// </summary>
		private double top;

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="DropPreviewAdorner"/> class.
		/// </summary>
		/// <param name="adornedElement">The element to bind the adorner to.</param>
		/// <param name="feedbackUIElement">Additional drag item visual feedback element.</param>
		/// <param name="adornerLayer">A surface for rendering adorners.</param>
		public DropPreviewAdorner(UIElement adornedElement, UIElement feedbackUIElement, AdornerLayer adornerLayer)
			: base(adornedElement)
		{
			this.adornedElement = adornedElement;
			this.adornerLayer = adornerLayer;

			presenter = new ContentPresenter
			            {
			            	Content = feedbackUIElement,
			            	IsHitTestVisible = false
			            };
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets offset from adorder layer left edge.
		/// </summary>
		public double Left
		{
			private get { return left; }
			set
			{
				left = value;
				UpdatePosition();
			}
		}

		/// <summary>
		/// Gets or sets offset from adorder layer top edge.
		/// </summary>
		public double Top
		{
			private get { return top; }
			set
			{
				top = value;
				UpdatePosition();
			}
		}

		#endregion

		/// <summary>
		/// Updates the layout and redraws all of the adorners in the adorner layer
		/// that are bound to the specified <see cref="System.Windows.UIElement"/>.
		/// </summary>
		private void UpdatePosition()
		{
			if (adornerLayer != null)
			{
				adornerLayer.Update(adornedElement);
			}
		}

		#region Adorner Overrides

		/// <summary>
		/// Gets the number of visual child elements within this element.
		/// </summary>
		protected override int VisualChildrenCount
		{
			get { return 1; }
		}

		/// <summary>
		/// Implements any custom measuring behavior for the adorner.
		/// </summary>
		/// <param name="constraint">A size to constrain the adorner to.</param>
		/// <returns>
		/// A <see cref="System.Windows.Size"/> object representing the amount
		/// of layout space needed by the adorner.
		/// </returns>
		protected override Size MeasureOverride(Size constraint)
		{
			presenter.Measure(constraint);
			return presenter.DesiredSize;
		}

		/// <summary>
		/// When overridden in a derived class,
		/// positions child elements and
		/// determines a size for a <see cref="System.Windows.FrameworkElement"/> derived class.
		/// </summary>
		/// <param name="finalSize">
		/// The final area within the parent that this element
		/// should use to arrange itself and its children.
		/// </param>
		/// <returns>The actual size used.</returns>
		protected override Size ArrangeOverride(Size finalSize)
		{
			presenter.Arrange(new Rect(finalSize));
			return finalSize;
		}

		/// <summary>
		/// Overrides <see cref="System.Windows.Media.Visual.GetVisualChild(System.Int32)"/>,
		/// and returns a child at the specified index from a collection of child elements.
		/// </summary>
		/// <param name="index">The zero-based index of the requested child element in the collection.</param>
		/// <returns>
		/// The requested child element. This should not return null;
		/// if the provided index is out of range, an exception is thrown.
		/// </returns>
		protected override Visual GetVisualChild(int index)
		{
			return presenter;
		}

		/// <summary>
		/// Returns a <see cref="System.Windows.Media.Transform"/> for the adorner,
		/// based on the transform that is currently applied to the adorned element.
		/// </summary>
		/// <param name="transform">The transform that is currently applied to the adorned element.</param>
		/// <returns>A transform to apply to the adorner.</returns>
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			var result = new GeneralTransformGroup();

			result.Children.Add(new TranslateTransform(Left, Top));
			result.Children.Add(base.GetDesiredTransform(transform));

			return result;
		}

		#endregion
	}
}