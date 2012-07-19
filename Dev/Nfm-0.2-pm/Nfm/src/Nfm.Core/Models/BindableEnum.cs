// <copyright file="BindableEnum.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-07-06</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-07-06</date>
// </editor>
// <summary>Represent bindable enum wrapper.</summary>

namespace Nfm.Core.Models
{
	/// <summary>
	/// Represent bindable enum wrapper.
	/// </summary>
	public class BindableEnum
	{
		#region Bindable Properties

		/// <summary>
		/// Gets or sets enum underlying value.
		/// </summary>
		public int UnderlyingValue { get; set; }

		/// <summary>
		/// Gets or sets friendly display name.
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets enum value.
		/// </summary>
		public object Value { get; set; }

		#endregion

		#region Overrides of Object

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="BindableEnum"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="BindableEnum"/>.</returns>
		public override string ToString()
		{
			return DisplayName;
		}

		/// <summary>
		/// Determines whether the specified <see cref="BindableEnum"/> is equal to the current <see cref="BindableEnum"/>.
		/// </summary>
		/// <param name="obj">The <see cref="BindableEnum"/> to compare with the current <see cref="BindableEnum"/>.</param>
		/// <returns>true if the specified <see cref="BindableEnum"/> is equal to the current <see cref="BindableEnum"/>; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			var otherBindable = obj as BindableEnum;

			if (otherBindable != null)
			{
				return UnderlyingValue == otherBindable.UnderlyingValue;
			}

			if (obj is int)
			{
				return UnderlyingValue.Equals((int) obj);
			}

			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current <see cref="BindableEnum"/>.</returns>
		public override int GetHashCode()
		{
			return UnderlyingValue.GetHashCode();
		}

		#endregion
	}
}