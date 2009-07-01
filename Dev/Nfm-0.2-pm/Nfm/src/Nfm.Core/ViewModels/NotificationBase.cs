// <copyright file="NotificationBase.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-28</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-28</date>
// </editor>
// <summary>Base class for all view models.</summary>

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Base class for all view models.
	/// </summary>
	public abstract class NotificationBase : IDisposable, INotifyPropertyChanged, INotifyPropertyChanging
	{
		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Forced object distruction.
		/// </summary>
		/// <param name="disposing">"True" for manual calls.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Free other state (managed objects).
			}

			// Free your own state (unmanaged objects).
			// Set large fields to null.
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="NotificationBase" /> class.
		/// </summary>
		~NotificationBase()
		{
			// Simply call Dispose(false).
			Dispose (false);
		}

		#endregion

		#region Implementation of INotifyPropertyChanged

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Check existence of property and fire <see cref="PropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyName">Changed property name.</param>
		protected virtual void OnPropertyChanged(string propertyName)
		{
			VerifyPropertyName(propertyName);

			PropertyChangedEventHandler handler = PropertyChanged;

			if (handler != null)
			{
				var e = new PropertyChangedEventArgs(propertyName);
				handler(this, e);
			}
		}

		#endregion

		#region Implementation of INotifyPropertyChanging

		/// <summary>
		/// Occurs when a property value is changing.
		/// </summary>
		public event PropertyChangingEventHandler PropertyChanging;

		/// <summary>
		/// Check existence of property and fire <see cref="PropertyChanging"/> event.
		/// </summary>
		/// <param name="propertyName">Changed property name.</param>
		protected virtual void OnPropertyChanging(string propertyName)
		{
			VerifyPropertyName(propertyName);

			PropertyChangingEventHandler handler = PropertyChanging;

			if (handler != null)
			{
				var e = new PropertyChangingEventArgs(propertyName);
				handler(this, e);
			}
		}

		#endregion

		#region Common for property changes notification

		/// <summary>
		/// Gets or sets a value indicating whether to throw exception on
		/// <see cref="PropertyChanged"/> or <see cref="PropertyChanging"/> event calls
		/// for non-existent property names.
		/// </summary>
		protected bool ThrowOnInvalidPropertyName { get; set; }

		/// <summary>
		/// Verify existence of property with specified name.
		/// </summary>
		/// <param name="propertyName">Property name to check.</param>
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public void VerifyPropertyName(string propertyName)
		{
			// Verify that the property name matches a real,  
			// public, instance property on this object.
			if (TypeDescriptor.GetProperties(this)[propertyName] == null)
			{
				string msg = "Invalid property name: " + propertyName;

				if (ThrowOnInvalidPropertyName)
				{
					throw new Exception(msg);
				}

				Debug.Fail(msg);
			}
		}

		#endregion

		#region Event firing

		/// <summary>
		/// Check for null and fire specific eventHandler delegate.
		/// </summary>
		/// <param name="eventHandler">Action to check and fire.</param>
		/// <param name="sender">Event sender.</param>
		protected static void OnEvent(EventHandler<EventArgs> eventHandler, object sender)
		{
			if (eventHandler != null)
			{
				eventHandler(sender, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Check for null and raise specific action delegate.
		/// </summary>
		/// <param name="action">Action to check and raise.</param>
		/// <param name="data">Action data.</param>
		protected static void OnAction<T>(Action<T> action, T data)
		{
			if (action != null)
			{
				action(data);
			}
		}

		#endregion
	}
}