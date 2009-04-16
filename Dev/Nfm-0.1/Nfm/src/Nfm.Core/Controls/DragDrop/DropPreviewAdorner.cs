using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Nfm.Core.Controls.DragDrop
{
	public class DropPreviewAdorner : Adorner
	{
		private readonly UIElement adornedElement;
		private readonly AdornerLayer adornerLayer;
		private readonly ContentPresenter presenter;
		private double left;
		private double top;

		public DropPreviewAdorner(UIElement feedbackUI, UIElement adornedElt, AdornerLayer layer) : base(adornedElt)
		{
			adornedElement = adornedElt;
			adornerLayer = layer;

			presenter = new ContentPresenter
			            {
			            	Content = feedbackUI,
			            	IsHitTestVisible = false
			            };
		}

		public double Left
		{
			get { return left; }
			set
			{
				left = value;
				UpdatePosition();
			}
		}

		public double Top
		{
			get { return top; }
			set
			{
				top = value;
				UpdatePosition();
			}
		}

		protected override int VisualChildrenCount
		{
			get { return 1; }
		}

		private void UpdatePosition()
		{
			if (adornerLayer != null)
			{
				adornerLayer.Update(adornedElement);
			}
		}

		protected override Size MeasureOverride(Size constraint)
		{
			presenter.Measure(constraint);
			return presenter.DesiredSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			presenter.Arrange(new Rect(finalSize));
			return finalSize;
		}

		protected override Visual GetVisualChild(int index)
		{
			return presenter;
		}

		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			var result = new GeneralTransformGroup();
			
			result.Children.Add(new TranslateTransform(Left, Top));
			result.Children.Add(base.GetDesiredTransform(transform));

			return result;
		}
	}
}