using System.Windows;

namespace Nfm.Core.Controls.DragDrop
{
	public interface IDropTargetAdvisor
	{
		UIElement TargetUI { get; set; }

		bool IsValidDataObject(IDataObject obj);
		void OnDropCompleted(IDataObject obj, Point dropPoint);
	}
}