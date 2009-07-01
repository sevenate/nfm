// <copyright file="HotKeyManager.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-13</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-13</date>
// </editor>
// <summary>General commands hot keys manager.</summary>

using System.Windows.Input;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// General command hot keys manager.
	/// </summary>
	public static class HotKeyManager
	{
		#region Navigate To Current (Enter)

		/// <summary>
		/// Gets hotkey for "NavigateToCurrentNode" command.
		/// </summary>
		public static Key NavigateToCurrentNodeHotKey
		{
			get { return Key.Enter; }
		}

		/// <summary>
		/// Gets hotkey modifiers for "NavigateToCurrentNode" command.
		/// </summary>
		public static ModifierKeys NavigateToCurrentNodeHotKeyModifiers
		{
			get { return ModifierKeys.None; }
		}

		#endregion

		#region Navigate To Parent (Backspace)

		/// <summary>
		/// Gets hotkey for "NavigateToParentNode" command.
		/// </summary>
		public static Key NavigateToParentHotKey
		{
			get { return Key.Back; }
		}

		/// <summary>
		/// Gets hotkey modifiers for "NavigateToParentNode" command.
		/// </summary>
		public static ModifierKeys NavigateToParentHotKeyModifiers
		{
			get { return ModifierKeys.None; }
		}

		#endregion

		#region Refresh (Ctrl + R)

		/// <summary>
		/// Gets hotkey for "RefreshChildNodes" command.
		/// </summary>
		public static Key RefreshChildsHotKey
		{
			get { return Key.R; }
		}

		/// <summary>
		/// Gets hotkey modifiers for "RefreshChildNodes" command.
		/// </summary>
		public static ModifierKeys RefreshChildsHotKeyModifiers
		{
			get { return ModifierKeys.Control; }
		}

		#endregion

		#region Close Tab (Ctrl + W)

		/// <summary>
		/// Gets hotkey for "ClosePanel" command.
		/// </summary>
		public static Key RequestCloseHotKey
		{
			get { return Key.W; }
		}

		/// <summary>
		/// Gets hotkey modifiers for "ClosePanel" command.
		/// </summary>
		public static ModifierKeys RequestCloseHotKeyModifiers
		{
			get { return ModifierKeys.Control; }
		}

		#endregion

		#region Duplicate Tab (Ctrl + T)

		/// <summary>
		/// Gets hotkey for "DublicateSelectedPanel" command.
		/// </summary>
		public static Key DuplicateHotKey
		{
			get { return Key.T; }
		}

		/// <summary>
		/// Gets hotkey modifiers for "DublicateSelectedPanel" command.
		/// </summary>
		public static ModifierKeys DuplicateHotKeyModifiers
		{
			get { return ModifierKeys.Control; }
		}

		#endregion

		#region Switch Theme (Ctrl + Alt + T)

		/// <summary>
		/// Gets hotkey for "SwitchTheme" command.
		/// </summary>
		public static Key SwitchThemeKey
		{
			get { return Key.T; }
		}

		/// <summary>
		/// Gets hotkey modifiers for "SwitchTheme" command.
		/// </summary>
		public static ModifierKeys SwitchThemeModifiers
		{
			get { return ModifierKeys.Control | ModifierKeys.Alt; }
		}

		#endregion

		#region Swap parent stack container (Ctrl + G)

		/// <summary>
		/// Gets hotkey for "SwapStackContainerOrientation" command.
		/// </summary>
		public static Key SwapStackContainerOrientationKey
		{
			get { return Key.G; }
		}

		/// <summary>
		/// Gets hotkey modifiers for "SwapStackContainerOrientation" command.
		/// </summary>
		public static ModifierKeys SwapStackContainerOrientationModifiers
		{
			get { return ModifierKeys.Control; }
		}

		#endregion

		#region Split tab container (Ctrl + Shift + G)

		/// <summary>
		/// Gets hotkey for "SplitTabContainerCommand" command.
		/// </summary>
		public static Key SplitTabContainerCommandKey
		{
			get { return Key.G; }
		}

		/// <summary>
		/// Gets hotkey modifiers for "SplitTabContainerCommand" command.
		/// </summary>
		public static ModifierKeys SplitTabContainerCommandModifiers
		{
			get { return ModifierKeys.Control | ModifierKeys.Shift; }
		}

		#endregion

		#region Wrap panel with tab container (Ctrl + Alt + G)

		/// <summary>
		/// Gets hotkey for "WrapPanelWithTabContainerCommand" command.
		/// </summary>
		public static Key WrapPanelWithTabContainerCommandKey
		{
			get { return Key.G; }
		}

		/// <summary>
		/// Gets hotkey modifiers for "SplitTabContainerCommand" command.
		/// </summary>
		public static ModifierKeys WrapPanelWithTabContainerCommandModifiers
		{
			get { return ModifierKeys.Control | ModifierKeys.Alt; }
		}

		#endregion

		#region Hide or show panel header command. (Ctrl + Shift + V)

		/// <summary>
		/// Gets hotkey for "WrapPanelWithTabContainerCommand" command.
		/// </summary>
		public static Key TogglePanelHeaderCommandKey
		{
			get { return Key.V; }
		}

		/// <summary>
		/// Gets hotkey modifiers for "SplitTabContainerCommand" command.
		/// </summary>
		public static ModifierKeys TogglePanelHeaderCommandModifiers
		{
			get { return ModifierKeys.Control | ModifierKeys.Shift; }
		}

		#endregion

		#region Change localization command. (Ctrl + Alt + L)

		/// <summary>
		/// Gets hotkey for "ChangeLocalizationCommand" command.
		/// </summary>
		public static Key ChangeLocalizationCommandKey
		{
			get { return Key.L; }
		}

		/// <summary>
		/// Gets hotkey modifiers for "ChangeLocalizationCommand" command.
		/// </summary>
		public static ModifierKeys ChangeLocalizationCommandModifiers
		{
			get { return ModifierKeys.Control | ModifierKeys.Alt; }
		}

		#endregion
	}
}