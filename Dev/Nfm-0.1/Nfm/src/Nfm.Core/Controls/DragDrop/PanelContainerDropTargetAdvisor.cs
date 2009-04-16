using System.Windows;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Controls.DragDrop
{
	public class PanelContainerDropTargetAdvisor : IDropTargetAdvisor
	{
		private static readonly DataFormat SupportedFormat = DataFormats.GetDataFormat("IPanel");

		#region IDropTargetAdvisor Members

		public bool IsValidDataObject(IDataObject obj)
		{
			return obj.GetDataPresent(SupportedFormat.Name);
		}

		public void OnDropCompleted(IDataObject obj, Point dropPoint)
		{
			var droppedPanel = obj.GetData(SupportedFormat.Name) as IPanel;
			var targetPanelContaiger = (IPanelContainer) (((FrameworkElement) TargetUI).DataContext);

			if (!targetPanelContaiger.Childs.Contains(droppedPanel))
			{
				// Bug: NOT Thread safe operation!
				// Simultaneously add item to it and data binding UI enumerate it.
				targetPanelContaiger.Childs.Add(droppedPanel);
			}
			else
			{
				var oldIndex = targetPanelContaiger.Childs.IndexOf(droppedPanel);
				targetPanelContaiger.Childs.Move(oldIndex, targetPanelContaiger.Childs.Count - 1);
			}
		}

		public UIElement TargetUI { get; set; }

		#endregion

		public PanelContainerDropTargetAdvisor()
		{
			;
		}
	}
}