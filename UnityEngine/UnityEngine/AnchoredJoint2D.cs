using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Parent class for all joints that have anchor points.</para>
	/// </summary>
	public class AnchoredJoint2D : Joint2D
	{
		/// <summary>
		///   <para>The joint's anchor point on the object that has the joint component.</para>
		/// </summary>
		public Vector2 anchor
		{
			get
			{
				Vector2 result;
				this.INTERNAL_get_anchor(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_anchor(ref value);
			}
		}

		/// <summary>
		///   <para>The joint's anchor point on the second object (ie, the one which doesn't have the joint component).</para>
		/// </summary>
		public Vector2 connectedAnchor
		{
			get
			{
				Vector2 result;
				this.INTERNAL_get_connectedAnchor(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_connectedAnchor(ref value);
			}
		}

		/// <summary>
		///   <para>Should the connectedAnchor be calculated automatically?</para>
		/// </summary>
		public extern bool autoConfigureConnectedAnchor
		{
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[WrapperlessIcall]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_anchor(out Vector2 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_anchor(ref Vector2 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_connectedAnchor(out Vector2 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_connectedAnchor(ref Vector2 value);
	}
}
