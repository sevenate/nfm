// <copyright file="BaseViewModel.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-11</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-11</date>
// </editor>
// <summary>Base view model.</summary>

using System;
using System.ComponentModel;
using System.Linq;
using Caliburn.PresentationFramework.Screens;
using Caliburn.PresentationFramework.ViewModels;

namespace Fab.Client.Main.ViewModels
{
	/// <summary>
	/// Base view model.
	/// </summary>
	public abstract class BaseViewModel : Screen, IDataErrorInfo
	{
		#region Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseViewModel"/> class.
		/// </summary>
		/// <param name="validator">Validator for view model data.</param>
		protected BaseViewModel(IValidator validator)
		{
			this.validator = validator;
		}

		#endregion

		#region Implementation of IDataErrorInfo

		/// <summary>
		/// Caliburn framework validator for view model data.
		/// </summary>
		/// <remarks>
		/// NOTE: You could also achieve validation without implementing the IDataErrorInfo interface by using Caliburn's AOP support.
		/// </remarks>
		private readonly IValidator validator;

		/// <summary>
		/// Gets a message that describes any validation errors for the specified property or column name.
		/// </summary>
		/// <returns>
		/// The validation error on the specified property, or null or <see cref="string.Empty"/> if there are no errors present.
		/// </returns>
		/// <param name="columnName">The name of the property or column to retrieve validation errors for.</param>
		public string this[string columnName]
		{
			get { return string.Join(Environment.NewLine, validator.Validate(this, columnName).Select(x => x.Message).ToArray()); }
		}

		/// <summary>
		/// Gets a message that describes any validation errors for the object.
		/// </summary>
		/// <returns>
		/// The validation error on the object, or null or <see cref="string.Empty"/> if there are no errors present. 
		/// </returns>
		public string Error
		{
			get { return string.Join(Environment.NewLine, validator.Validate(this).Select(x => x.Message).ToArray()); }
		}

		#endregion

		#region Overrides of Object

		// Note: The following overrides insure that all instances of this screen are treated as
		// equal by the screen activation mechanism without forcing a singleton registration
		// in the container.

		/// <summary>
		/// Determines whether the specified <see cref="object"/> is equal to the current <see cref="object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="object"/> to compare with the current <see cref="object"/>.</param>
		/// <exception cref="NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
//		public override bool Equals(object obj)
//		{
//			return obj != null && obj.GetType() == GetType();
//		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="object"/>.
		/// </returns>
//		public override int GetHashCode()
//		{
//			return GetType().GetHashCode();
//		}

		#endregion
	}
}