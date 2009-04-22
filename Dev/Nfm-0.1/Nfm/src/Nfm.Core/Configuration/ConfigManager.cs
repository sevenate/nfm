// <copyright file="ConfigManager.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-20</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-20</date>
// </editor>
// <summary>General configuration settings manager.</summary>

using System.Windows.Controls;
using Nfm.Core.Models;
using Nfm.Core.Models.FileSystem;
using Nfm.Core.ViewModels;
using Nfm.Core.ViewModels.FileSystem;

namespace Nfm.Core.Configuration
{
	/// <summary>
	/// General configuration settings manager.
	/// </summary>
	public static class ConfigManager
	{
		/// <summary>
		/// Gets panels layout.
		/// </summary>
		/// <returns>Panels layout.</returns>
		public static IPanel GetLayout()
		{
			LoadModules();
			return GetDefaultLayout();
		}

		/// <summary>
		/// Gets default panels layout.
		/// </summary>
		/// <returns>Default panels layout.</returns>
		private static IPanel GetDefaultLayout()
		{
			#region Work Left Tab Container

			var driveCPanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					Key = @"C:\",
				})
			                  {
			                  	IsSelected = true
			                  };
			driveCPanel.RefreshDetails();
			driveCPanel.RefreshChilds();

			var driveDPanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					Key = @"D:\",
				});
			driveDPanel.RefreshDetails();
			driveDPanel.RefreshChilds();

			var workLeftTabContainer = new TabContainer
			                           {
			                           	Header = "Left Tab Container"
			                           };
			workLeftTabContainer.Childs.Add(driveDPanel);
			workLeftTabContainer.Childs.Add(driveCPanel);

			#endregion

			#region Work Middle Tab

			var workMiddlePanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					Key = @"D:\Downloads",
				});
			workMiddlePanel.RefreshDetails();
			workMiddlePanel.RefreshChilds();

			var workMiddleTabContainer = new TabContainer
			                             {
			                             	Header = "Middle Tab Container"
			                             };
			workMiddleTabContainer.Childs.Add(workMiddlePanel);

			#endregion

			#region Work Right Vertical Stack Container

			var workRightPanel1 = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					Key = @"D:\Games",
				});
			workRightPanel1.RefreshDetails();
			workRightPanel1.RefreshChilds();

			var workRightPanel2 = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					Key = @"D:\Music",
				});
			workRightPanel2.RefreshDetails();
			workRightPanel2.RefreshChilds();

			var workRightPanel1TabContainer = new TabContainer
			                                  {
			                                  	Header = "Work Right Top Tab Container"
			                                  };
			workRightPanel1TabContainer.Childs.Add(workRightPanel1);

			var workRightPanel2TabContainer = new TabContainer
			                                  {
			                                  	Header = "Work Right Bottom Tab Container"
			                                  };
			workRightPanel2TabContainer.Childs.Add(workRightPanel2);

			var workRightStackContainer = new StackContainer
			                              {
			                              	Header = "Right Stack Container",
			                              	Orientation = Orientation.Vertical
			                              };
			//			workRightStackContainer.Childs.Add(workRightPanel1);
			//			workRightStackContainer.Childs.Add(workRightPanel2);
			workRightStackContainer.Childs.Add(workRightPanel1TabContainer);
			workRightStackContainer.Childs.Add(workRightPanel2TabContainer);

			#endregion

			#region Work Horizontal Stack Container

			var workStackContainer = new StackContainer
			                         {
			                         	Header = "Work Stack Container"
			                         };
			workStackContainer.Childs.Add(workLeftTabContainer);
			//			workStackContainer.Childs.Add(workMiddlePanel);
			workStackContainer.Childs.Add(workMiddleTabContainer);
			workStackContainer.Childs.Add(workRightStackContainer);

			#endregion

			#region Enter Panel

			var enterPanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					Key = @"D:\Videos",
				})
			                 {
			                 	IsSelected = true
			                 };
			enterPanel.RefreshDetails();
			enterPanel.RefreshChilds();

			#endregion

			#region Top Tab Container

			var topGamesPanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					Key = @"D:\Games",
				});
			topGamesPanel.RefreshDetails();
			topGamesPanel.RefreshChilds();

			var topMusicPanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					Key = @"D:\Music",
				})
			                    {
			                    	IsSelected = true
			                    };
			topMusicPanel.RefreshDetails();
			topMusicPanel.RefreshChilds();

			var subTabContainer1 = new TabContainer
			                       {
			                       	Header = "Enter Container"
			                       };
			subTabContainer1.Childs.Add(topGamesPanel);
			subTabContainer1.Childs.Add(topMusicPanel);

			var topDriveCpanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					Key = @"C:\",
				});
			topDriveCpanel.RefreshDetails();
			topDriveCpanel.RefreshChilds();

			var topDriveDpanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					Key = @"D:\",
				})
			                     {
			                     	IsSelected = true
			                     };
			topDriveDpanel.RefreshDetails();
			topDriveDpanel.RefreshChilds();

			var subTabContainer2 = new TabContainer
			                       {
			                       	Header = "DisksContainer",
			                       	IsSelected = true
			                       };

			subTabContainer2.Childs.Add(topDriveCpanel);
			subTabContainer2.Childs.Add(topDriveDpanel);

			var topTabContainer = new TabContainer
			                      {
			                      	Header = "Top Tab Container",
			                      };
			topTabContainer.Childs.Add(subTabContainer1);
			topTabContainer.Childs.Add(subTabContainer2);

			#endregion

			#region Main Tab Container

			var mainTabContainer = new TabContainer
			                       {
			                       	Header = "Main Window",
			                       };
			mainTabContainer.Childs.Add(workStackContainer);
			mainTabContainer.Childs.Add(enterPanel);
			mainTabContainer.Childs.Add(topTabContainer);

			#endregion

			return mainTabContainer; //workLeftTabContainer
		}

		/// <summary>
		/// Load root node modules.
		/// </summary>
		private static void LoadModules()
		{
			RootNode.Inst.RegisterNode(new LocalFileSystemModule(RootNode.Inst));
		}
	}
}