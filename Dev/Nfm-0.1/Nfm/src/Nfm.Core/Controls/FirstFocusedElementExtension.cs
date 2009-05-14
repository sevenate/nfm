// <copyright file="FirstFocusedElementExtension.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Mark Smith">
//	<url>http://www.julmar.com/blog/mark/PermaLink,guid,507386bd-a72e-455e-b345-315e0dcf35e9.aspx</url>
// 	<date>2008-09-12</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-08</date>
// </editor>
// <summary>
//	This markup extension locates the first focusable child and returns it.
//	It is intended to be used with FocusManager.FocusedElement in XAML markup.
// </summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Nfm.Core.Controls
{
	/// <summary>
	/// This markup extension locates the first focusable child and returns it.
	/// It is intended to be used with FocusManager.FocusedElement:
	/// <![CDATA[
	///		<Window ... FocusManager.FocusedElement={ctrl:FirstFocusedElement} />
	/// ]]>
	/// </summary>
	public class FirstFocusedElementExtension : MarkupExtension
	{
		#region Const

		/// <summary>
		/// Default focusable candidate filter.
		/// </summary>
		private Func<IInputElement, bool> defaultFilder = element => true;

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="FirstFocusedElementExtension"/> class.
		/// </summary>
		public FirstFocusedElementExtension()
		{
			OneTime = true;
		}

		#endregion

		#region Public Configuration Parameters

		/// <summary>
		/// Gets or sets a value indicating whether there is a need to unhook the handler after it has set focus to the element the first time
		/// </summary>
		public bool OneTime { get; set; }

		#endregion

		#region MarkupExtension Overrides

		/// <summary>
		/// This method locates the first focusable + visible element we can change focus to.
		/// </summary>
		/// <param name="serviceProvider"><see cref="IServiceProvider"/> from XAML.</param>
		/// <returns>Focusable Element or null.</returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			// Ignore if in design mode
			if ((bool) DesignerProperties
			           	.IsInDesignModeProperty
			           	.GetMetadata(typeof (DependencyObject)).DefaultValue)
			{
				return null;
			}

			// Get the IProvideValue interface which gives us access to the target property 
			// and object.  Note that MSDN calls this interface "internal" but it's necessary
			// here because we need to know what property we are assigning to.
			var pvt = serviceProvider.GetService(typeof (IProvideValueTarget)) as IProvideValueTarget;
			
			if (pvt != null)
			{
				// Note: Important check for System.Windows.SharedDp,
				// when using markup extension inside of ControlTemplates and DataTemplates.
				// Details: http://social.msdn.microsoft.com/forums/en-US/wpf/thread/a9ead3d5-a4e4-4f9c-b507-b7a7d530c6a9/
				if (!(pvt.TargetObject is DependencyObject))
				{
					return this;
				}

				var fe = pvt.TargetObject as FrameworkElement;
				object targetProperty = pvt.TargetProperty;
				
				if (fe != null)
				{
					// If the element isn't loaded yet, then wait for it.
					if (!fe.IsLoaded)
					{
						RoutedEventHandler deferredFocusHookup = null;
						deferredFocusHookup = delegate
						                      {
						                      	// Ignore if the element is now loaded but not
						                      	// visible -- this happens for things like TabItem.
						                      	// Instead, we'll wait until the item becomes visible and
						                      	// then set focus.
						                      	if (fe.IsVisible == false)
						                      	{
						                      		return;
						                      	}

						                      	// Look for the first focusable leaf child and set the property
												IInputElement ie = GetLeafFocusableChild(fe, defaultFilder);
						                      	
												  if (targetProperty is DependencyProperty)
						                      	{
						                      		// Specific case where we are setting focused element.
						                      		// We really need to set this property onto the focus scope, 
						                      		// so we'll use UIElement.Focus() which will do exactly that.
						                      		if (targetProperty == FocusManager.FocusedElementProperty)
						                      		{
						                      			ie.Focus();
						                      		}
						                      		else
						                      		{
						                      			// Being assigned to some other property - just assign it.
						                      			fe.SetValue((DependencyProperty) targetProperty, ie);
						                      		}
						                      	}
						                      	else if (targetProperty is PropertyInfo)
						                      	{
						                      		// Simple property assignment through reflection.
						                      		var pi = (PropertyInfo) targetProperty;
						                      		pi.SetValue(fe, ie, null);
						                      	}

						                      	// Unhook the handler if we are supposed to.
						                      	if (OneTime)
						                      	{
						                      		fe.Loaded -= deferredFocusHookup;
						                      	}
						                      };

						// Wait for the element to load
						fe.Loaded += deferredFocusHookup;
					}
					else
					{
						return GetLeafFocusableChild(fe, defaultFilder);
					}
				}
			}

			return null;
		}

		#endregion

		#region Private Helpers

		/// <summary>
		/// Locate the first real focusable child.  We keep going down
		/// the visual tree until we hit a leaf node.
		/// </summary>
		/// <param name="fe">Focusable element.</param>
		/// <param name="isSuitable">Custom element filter.</param>
		/// <returns>Leaf focusable element.</returns>
		private static IInputElement GetLeafFocusableChild(IInputElement fe, Func<IInputElement, bool> isSuitable)
		{
			IInputElement ie = GetFirstFocusableChild(fe, isSuitable);
			IInputElement final = ie;
			
			while (final != null)
			{
				ie = final;
				final = GetFirstFocusableChild(final, isSuitable);
			}

			return ie;
		}

		/// <summary>
		/// This searches the Visual Tree looking for a valid child which can have focus.
		/// </summary>
		/// <param name="fe">Focusable element.</param>
		/// <param name="isSuitable">Custom element filter.</param>
		/// <returns>First focusable element.</returns>
		private static IInputElement GetFirstFocusableChild(IInputElement fe, Func<IInputElement, bool> isSuitable)
		{
			var dpo = fe as DependencyObject;
			return dpo == null
			       	? null
			       	: (from vc in EnumerateVisualTree(dpo, c => !FocusManager.GetIsFocusScope(c))
			       	   let iic = vc as IInputElement
			       	   where iic != null && iic.Focusable && iic.IsEnabled &&
			       	         (!(iic is FrameworkElement) || ((FrameworkElement) iic).IsVisible) &&
			       	         isSuitable(iic)
			       	   select iic).FirstOrDefault();
		}

		/// <summary>
		/// A simple iterator method to expose the visual tree to LINQ
		/// </summary>
		/// <param name="start">Root visual tree element.</param>
		/// <param name="eval">Evaluation predicate.</param>
		/// <typeparam name="T">Dependency object.</typeparam>
		/// <returns>Iterator for matching elements in visual tree.</returns>
		private static IEnumerable<T> EnumerateVisualTree<T>(T start, Predicate<T> eval) where T : DependencyObject
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(start); i++)
			{
				var child = VisualTreeHelper.GetChild(start, i) as T;

				if (child != null && (eval != null && eval(child)))
				{
					yield return child;

					foreach (T childOfChild in EnumerateVisualTree(child, eval))
					{
						yield return childOfChild;
					}
				}
			}
		}

		#endregion
	}
}