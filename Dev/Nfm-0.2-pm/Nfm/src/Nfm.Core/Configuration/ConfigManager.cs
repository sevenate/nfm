﻿// <copyright file="ConfigManager.cs" company="HD">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Caliburn.Core;
using Caliburn.PresentationFramework;
using Nfm.Core.Commands;
using Nfm.Core.Localization;
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
		#region Constants

		/// <summary>
		/// Default folder with localization xml files.
		/// </summary>
		private const string DefaultLocalizationFolder = "Languages";

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes static members of the <see cref="ConfigManager"/> class.
		/// </summary>
		static ConfigManager()
		{
			LoadModules();
			InitCulture();
		}

		#endregion

		#region Initialization

		/// <summary>
		/// Load root node modules.
		/// </summary>
		private static void LoadModules()
		{
			var firstModule = new LocalFileSystemModule();
			RootNode.Inst.RegisterNode(firstModule, new LocalFileSystemModuleVM(firstModule));

			// Todo: Check existing modules files and check configuration for enabled modules.
		}

		/// <summary>
		/// Initialize current translation and all other available languages.
		/// </summary>
		private static void InitCulture()
		{
			// TODO: make languages folder configuragle
			Translator.LocalizationProvider = new XmlFileLocalizationProvider
			                                  {
			                                  	Folder = DefaultLocalizationFolder
			                                  };

			Translator.RefreshAvailableLocalizations();

			if (Translator.AvailableLocalizations.Count() > 0)
			{
				// prefer natural UI culture
				LocalizationInfo uiCulture = (from info in Translator.AvailableLocalizations
				                              where info.Culture.LCID == Translator.CurrentUICulture.LCID
				                              select info).FirstOrDefault();

				// next try culture from region settings
				LocalizationInfo culture = (from info in Translator.AvailableLocalizations
				                            where info.Culture.LCID == Translator.CurrentCulture.LCID
				                            select info).FirstOrDefault();

				// next try "DefaultCulture" (for example, with custom translation)
				// TODO: make default culture resource string
				LocalizationInfo englishCulture = (from info in Translator.AvailableLocalizations
				                                   where info.Culture.LCID == Translator.DefaultCulture.LCID
				                                   select info).FirstOrDefault();

				Translator.CurrentLocalization = uiCulture ?? culture ?? englishCulture;
			}
			else
			{
				// set fallback culture
				Translator.CurrentLocalization = null;
			}
		}

		#endregion

		#region  Layout Generation

		/// <summary>
		/// Gets panels layout.
		/// </summary>
		/// <returns>Panels layout.</returns>
		public static IPanel GetLayout()
		{
#if DEBUG
			return GetDebugLayout();
#endif
			return GetDefaultLayout();
		}

		/// <summary>
		/// Gets default panels layout.
		/// </summary>
		/// <returns>Default panels layout.</returns>
		private static IPanel GetDefaultLayout()
		{
			var viewModels = new List<IViewModel>();
			var panels = new List<IPanel>();

			string[] logicalDrives = Environment.GetLogicalDrives();

			for (int i = 0; i < logicalDrives.Length; i++)
			{
				string logicalDrive = logicalDrives[i];
				IViewModel node = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\" + logicalDrive);
				node.Refresh();
				viewModels.Add(node);

				if (i == 2)
				{
					// 3 drives enough for 3 panels in horizontal oriented stack container
					break;
				}
			}

			// Make sure that there will be three panels even if the system have one or two logical drives
			switch (viewModels.Count)
			{
				case 0:
					throw new Exception("No one logical drives has been found in the system.");

				case 1:
					viewModels.Add((IViewModel) viewModels[0].Clone());
					viewModels.Add((IViewModel) viewModels[0].Clone());
					break;

				case 2:
					viewModels.Add((IViewModel) viewModels[1].Clone());
					break;
			}


			StackContainer drivesStackContainer = GenerateStackContainer(
				viewModels, "Logical Drives", Orientation.Horizontal, true);

			viewModels.Clear();

			IViewModel personal =
				RootNode.Inst.GetNode(
					@"\{78888951-2516-4e63-AC97-90E9D54351D8}\" + Environment.GetFolderPath(Environment.SpecialFolder.Personal));
			personal.Refresh();

			IViewModel desktop =
				RootNode.Inst.GetNode(
					@"\{78888951-2516-4e63-AC97-90E9D54351D8}\" + Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
			desktop.Refresh();

			IViewModel myMusic =
				RootNode.Inst.GetNode(
					@"\{78888951-2516-4e63-AC97-90E9D54351D8}\" + Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
			myMusic.Refresh();

			IViewModel myPictures =
				RootNode.Inst.GetNode(
					@"\{78888951-2516-4e63-AC97-90E9D54351D8}\" + Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
			myPictures.Refresh();


			viewModels.Add(personal);
			viewModels.Add(desktop);

			TabContainer dataTabContainer = GenerateTabContainer(viewModels, "Personal Data", 0);

			viewModels.Clear();

			viewModels.Add(myMusic);
			viewModels.Add(myPictures);

			StackContainer mediaStackContainer = GenerateStackContainer(viewModels, "Media Content", Orientation.Vertical, true);

			panels.Add(dataTabContainer);
			panels.Add(mediaStackContainer);

			StackContainer secondTabStackContainer = GenerateStackContainer(
				panels, "Special Folders", Orientation.Horizontal, false);

			panels.Clear();

			panels.Add(drivesStackContainer);
			panels.Add(secondTabStackContainer);

			TabContainer mainTabContainer = GenerateTabContainer(panels, "Main Window", 0);

			var topStackContainer = new StackContainer
			                        {
			                        	Header = new PanelHeader
			                        	         {
			                        	         	Text = "Root Stack Container"
			                        	         },
			                        	Orientation = Orientation.Vertical
			                        };

			topStackContainer.Presenters.Add(mainTabContainer);

			return topStackContainer;
		}

		private static TabContainer GenerateTabContainer(IEnumerable<IViewModel> viewModels, string header, int selectedIndex)
		{
			var panels = new List<IPanel>();

			foreach (IViewModel viewModel in viewModels)
			{
				var panel = new PanelBase
				            {
				            	PanelContent = (IPanelContent) viewModel.Clone()
				            };

				panels.Add(panel);
			}

			return GenerateTabContainer(panels, header, selectedIndex);
		}

		private static TabContainer GenerateTabContainer(IList<IPanel> panels, string header, int selectedIndex)
		{
			var panelHeader = new PanelHeader
			                  {
			                  	Text = header
			                  };

			var container = new TabContainer
			                {
			                	Header = panelHeader,
			                };

			for (int i = 0; i < panels.Count; i++)
			{
				var panel = (IPanel) panels[i].Clone();
				if (i == selectedIndex)
				{
					panel.Activate();
				}
				else
				{
					panel.Deactivate();
				}

				container.Presenters.Add(panel);
			}

			return container;
		}

		private static StackContainer GenerateStackContainer(
			IEnumerable<IViewModel> viewModels, string header, Orientation orientation, bool wrapUpEachViewModelInTabContainer)
		{
			var panels = new List<IPanel>();

			foreach (IViewModel viewModel in viewModels)
			{
				var panel = new PanelBase
				            {
				            	PanelContent = (IPanelContent) viewModel.Clone(),
				            };

				panels.Add(panel);
			}

			return GenerateStackContainer(panels, header, orientation, wrapUpEachViewModelInTabContainer);
		}

		private static StackContainer GenerateStackContainer(
			IEnumerable<IPanel> panels, string header, Orientation orientation, bool wrapUpEachPanelInTabContainer)
		{
			var panelHeader = new PanelHeader
			                  {
			                  	Text = header
			                  };

			var stackContainer = new StackContainer
			                     {
			                     	Header = panelHeader,
			                     	Orientation = orientation
			                     };

			foreach (IPanel panel in panels)
			{
				var newPanel = (IPanel) panel.Clone();

				if (wrapUpEachPanelInTabContainer)
				{
					newPanel.Activate();

					var tabContainer = new TabContainer();
					tabContainer.Presenters.Add(newPanel);

					stackContainer.Presenters.Add(tabContainer);
				}
				else
				{
					stackContainer.Presenters.Add(newPanel);
				}
			}

			return stackContainer;
		}

		#endregion

		#region Debug Layout

		/// <summary>
		/// Gets debug panels layout.
		/// </summary>
		/// <returns>Debug panels layout.</returns>
		private static IPanel GetDebugLayout()
		{
			IPanelContainer workLeftTabContainer = GetWorkLeftTabContainer();
			IPanelContainer workMiddleTabContainer = GetWorkMiddleTabContainer();
			IPanelContainer workRightStackContainer = GetWorkRightStackContainer();
			IPanelContainer workStackContainer = GetWorkStackContainer(
				workLeftTabContainer, workMiddleTabContainer, workRightStackContainer);

			IPanel enterPanel = GetEnterPanel();
			enterPanel.Activate();

			IPanelContainer subTabContainer1 = GetEnterTopTabContainer();
			IPanelContainer subTabContainer2 = GetDisksTopTabSubContainer();
			IPanelContainer topTabContainer = GetTopTabContainer(subTabContainer1, subTabContainer2);

			//			workStackContainer.IsSelected = true;
			IPanelContainer mainTabContainer = GetMainTabContainer(enterPanel, workStackContainer, topTabContainer);

			return mainTabContainer; //workLeftTabContainer
		}

		/// <summary>
		/// Get Work Left Tab Container.
		/// </summary>
		/// <returns>Work Left Tab Container.</returns>
		private static IPanelContainer GetWorkLeftTabContainer()
		{
			IViewModel music = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\Music");
			music.Refresh();

			var musicPanel = new PanelBase
			                 {
			                 	PanelContent = (IPanelContent) music,
			                 };
			musicPanel.Activate();

			IViewModel driveD = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\C:\");
			driveD.Refresh();

			var driveDPanel = new PanelBase
			                  {
			                  	PanelContent = (IPanelContent) driveD
			                  };

			var workLeftTabContainer = new TabContainer
			                           {
			                           	Header = new PanelHeader
			                           	         {
			                           	         	Text = "Left Tab Container"
			                           	         }
			                           };
			workLeftTabContainer.Presenters.Add(driveDPanel);
			workLeftTabContainer.Presenters.Add(musicPanel);
			return workLeftTabContainer;
		}

		/// <summary>
		/// Get Work Middle Tab Container.
		/// </summary>
		/// <returns>Work Middle Tab Container.</returns>
		private static IPanelContainer GetWorkMiddleTabContainer()
		{
			IViewModel workMiddle = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\Downloads");
			workMiddle.Refresh();

			var workMiddlePanel = new PanelBase
			                      {
			                      	PanelContent = (IPanelContent) workMiddle
			                      };

//			var workMiddleTabContainer = new StackContainer
			var workMiddleTabContainer = new TabContainer
			                             {
			                             	Header = new PanelHeader
			                             	         {
			                             	         	Text = "Middle Tab Container"
			                             	         },
//											 Orientation = Orientation.Vertical
			                             };
			workMiddleTabContainer.Presenters.Add(workMiddlePanel);
			return workMiddleTabContainer;
		}

		/// <summary>
		/// Get Work Right Stack Container.
		/// </summary>
		/// <returns>Work Right Stack Container.</returns>
		private static IPanelContainer GetWorkRightStackContainer()
		{
			IViewModel workRight1 = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\Games");
			workRight1.Refresh();

			var workRightPanel1 = new PanelBase
			                      {
			                      	PanelContent = (IPanelContent) workRight1
			                      };

			IViewModel workRight2 = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\");
			workRight2.Refresh();

			var workRightPanel2 = new PanelBase
			                      {
			                      	PanelContent = (IPanelContent) workRight2
			                      };

			var workRightPanel1TabContainer = new TabContainer
			                                  {
			                                  	Header = new PanelHeader
			                                  	         {
			                                  	         	Text = "Work Right Top Tab Container"
			                                  	         }
			                                  };
			workRightPanel1TabContainer.Presenters.Add(workRightPanel1);

			var workRightPanel2TabContainer = new TabContainer
			                                  {
			                                  	Header = new PanelHeader
			                                  	         {
			                                  	         	Text = "Work Right Bottom Tab Container"
			                                  	         }
			                                  };
			workRightPanel2TabContainer.Presenters.Add(workRightPanel2);

			var workRightStackContainer = new StackContainer
			                              {
			                              	Header = new PanelHeader
			                              	         {
			                              	         	Text = "Right Stack Container"
			                              	         },
			                              	Orientation = Orientation.Vertical
			                              };
			//			workRightStackContainer.Childs.Add(workRightPanel1);
			//			workRightStackContainer.Childs.Add(workRightPanel2);
			workRightStackContainer.Presenters.Add(workRightPanel1TabContainer);
			workRightStackContainer.Presenters.Add(workRightPanel2TabContainer);
			return workRightStackContainer;
		}

		/// <summary>
		/// Get Work Stack Container.
		/// </summary>
		/// <param name="workLeftTabContainer">Work left tab container.</param>
		/// <param name="workMiddleTabContainer">Work middle tab container.</param>
		/// <param name="workRightStackContainer">Work right stack container.</param>
		/// <returns>Work Stack Container.</returns>
		private static IPanelContainer GetWorkStackContainer(
			IPanel workLeftTabContainer, IPanel workMiddleTabContainer, IPanel workRightStackContainer)
		{
			var workStackContainer = new StackContainer
			                         {
			                         	Header = new PanelHeader
			                         	         {
			                         	         	Text = "Work Stack Container"
			                         	         }
			                         };
			workStackContainer.Presenters.Add(workLeftTabContainer);
			//			workStackContainer.Childs.Add(workMiddlePanel);
			workStackContainer.Presenters.Add(workMiddleTabContainer);
			workStackContainer.Presenters.Add(workRightStackContainer);
			return workStackContainer;
		}

		/// <summary>
		/// Get Enter Panel.
		/// </summary>
		/// <returns>Enter Panel.</returns>
		private static IPanel GetEnterPanel()
		{
			IViewModel enter = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\Videos");
			enter.Refresh();

			if (enter is IViewModelWithChilds)
			{
				var enterWithChilds = (IViewModelWithChilds) enter;

				if (enterWithChilds.Childs.Count > 5)
				{
					enterWithChilds.SelectedItems.Add(enterWithChilds.Childs[1]);
					enterWithChilds.SelectedItems.Add(enterWithChilds.Childs[2]);
					enterWithChilds.SelectedItems.Add(enterWithChilds.Childs[3]);

					enterWithChilds.CurrentItemIndex = 4;
				}
			}

			return new PanelBase
			       {
			       	PanelContent = (IPanelContent) enter
			       };
		}

		/// <summary>
		/// Get Enter Top Tab Container.
		/// </summary>
		/// <returns>Enter Top Tab Container.</returns>
		private static IPanelContainer GetEnterTopTabContainer()
		{
			IViewModel topGames = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\Games");
			topGames.Refresh();

			var topGamesPanel = new PanelBase
			                    {
			                    	PanelContent = (IPanelContent) topGames
			                    };

			IViewModel topMusic = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\Music");
			topMusic.Refresh();

			var topMusicPanel = new PanelBase
			                    {
			                    	PanelContent = (IPanelContent) topMusic,
			                    };
			topMusicPanel.Activate();

			var enterTopTabContainer = new TabContainer
			                           {
			                           	Header = new PanelHeader
			                           	         {
			                           	         	Text = "Enter Container"
			                           	         }
			                           };
			enterTopTabContainer.Presenters.Add(topGamesPanel);
			enterTopTabContainer.Presenters.Add(topMusicPanel);
			return enterTopTabContainer;
		}

		/// <summary>
		/// Get Disks Top Tab Sub Container.
		/// </summary>
		/// <returns>Disks Top Tab Sub Container.</returns>
		private static IPanelContainer GetDisksTopTabSubContainer()
		{
			IViewModel topDriveC = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\C:\");
			topDriveC.Refresh();

			var topDriveCpanel = new PanelBase
			                     {
			                     	PanelContent = (IPanelContent) topDriveC
			                     };

			IViewModel topDriveD = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\");
			topDriveD.Refresh();

			var topDriveDpanel = new PanelBase
			                     {
			                     	PanelContent = (IPanelContent) topDriveD,
			                     };
			topDriveDpanel.Activate();

			var subTabContainer2 = new TabContainer
			                       {
			                       	Header = new PanelHeader
			                       	         {
			                       	         	Text = "Disks Container"
			                       	         },
			                       };
			subTabContainer2.Activate();

			subTabContainer2.Presenters.Add(topDriveCpanel);
			subTabContainer2.Presenters.Add(topDriveDpanel);
			return subTabContainer2;
		}

		/// <summary>
		/// Get Top Tab Container.
		/// </summary>
		/// <param name="subTabContainer1">Sub container 1.</param>
		/// <param name="subTabContainer2">Sub container 2.</param>
		/// <returns>Top Tab Container.</returns>
		private static IPanelContainer GetTopTabContainer(IPanel subTabContainer1, IPanel subTabContainer2)
		{
			var topTabContainer = new TabContainer
			                      {
			                      	Header = new PanelHeader
			                      	         {
			                      	         	Text = "Top Tab Container"
			                      	         },
			                      };
			topTabContainer.Presenters.Add(subTabContainer1);
			topTabContainer.Presenters.Add(subTabContainer2);
			return topTabContainer;
		}

		/// <summary>
		/// Get Main Tab Container.
		/// </summary>
		/// <param name="firstPanel">Work Stack Container.</param>
		/// <param name="secondPanel">Rnter Panel.</param>
		/// <param name="thirdPanel">Top Tab Container.</param>
		/// <returns>Main Tab Container.</returns>
		private static IPanelContainer GetMainTabContainer(IPanel firstPanel, IPanel secondPanel, IPanel thirdPanel)
		{
			var mainTabContainer = new TabContainer
			                       {
			                       	Header = new PanelHeader
			                       	         {
			                       	         	Text = "Main Window"
			                       	         },
			                       };
			mainTabContainer.Presenters.Add(firstPanel);
			mainTabContainer.Presenters.Add(secondPanel);
			mainTabContainer.Presenters.Add(thirdPanel);

			var topStackContainer = new StackContainer
			                        {
			                        	Header = new PanelHeader
			                        	         {
			                        	         	Text = "Root Stack Container"
			                        	         },
			                        	Orientation = Orientation.Vertical
			                        };

			topStackContainer.Presenters.Add(mainTabContainer);

			return topStackContainer;
		}

		#endregion
	}
}