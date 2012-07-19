// <copyright file="ToolbarBase.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-26</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-26</date>
// </editor>
// <summary>Basic <see cref="IToolbar"/> implementation.</summary>

using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Basic <see cref="IToolbar"/> implementation.
	/// </summary>
	[DebuggerDisplay("{Childs.Count}")]
	public class ToolbarBase : NotificationBase, IToolbar
	{
		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolbarBase"/> class.
		/// </summary>
		public ToolbarBase()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolbarBase"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="ToolbarBase"/> instance to copy data from.</param>
		protected ToolbarBase(ToolbarBase another)
		{
			// Deep copy all childs
			var childsCopy = new ObservableCollection<IToolbarItem>();

			foreach (IToolbarItem child in another.Childs)
			{
				var newChild = (IToolbarItem) child.Clone();
				childsCopy.Add(newChild);
			}

			childs = childsCopy;
		}

		#endregion

		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public object Clone()
		{
			return new ToolbarBase(this);
		}

		#endregion

		#region Implementation of IToolbar

		/// <summary>
		/// Child toolbar items.
		/// </summary>
		private ObservableCollection<IToolbarItem> childs;

		/// <summary>
		/// Gets all child toolbar items.
		/// </summary>
		public ObservableCollection<IToolbarItem> Childs
		{
			get
			{
				if (childs == null)
				{
					OnPropertyChanging("Childs");
					childs = new ObservableCollection<IToolbarItem>();
					OnPropertyChanged("Childs");
				}

				return childs;
			}
		}

		#endregion
	}
}