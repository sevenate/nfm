using System.Windows;

namespace Nfm.Core.Controls.DragDrop
{
	public interface IDragSourceAdvisor
	{
		UIElement SourceUI { get; set; }

		DragDropEffects SupportedEffects { get; }

		DataObject GetDataObject(UIElement draggedElement);
		UIElement GetVisualFeedback(UIElement draggedElement);
		void FinishDrag(UIElement draggedElement, DragDropEffects finalEffects);
		bool IsDraggable(UIElement element);
	}
}