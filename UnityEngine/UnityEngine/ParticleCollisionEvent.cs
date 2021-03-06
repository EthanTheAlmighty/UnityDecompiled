using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Information about a particle collision.</para>
	/// </summary>
	public struct ParticleCollisionEvent
	{
		private Vector3 m_Intersection;

		private Vector3 m_Normal;

		private Vector3 m_Velocity;

		private int m_ColliderInstanceID;

		/// <summary>
		///   <para>Intersection point of the collision in world coordinates.</para>
		/// </summary>
		public Vector3 intersection
		{
			get
			{
				return this.m_Intersection;
			}
		}

		/// <summary>
		///   <para>Geometry normal at the intersection point of the collision.</para>
		/// </summary>
		public Vector3 normal
		{
			get
			{
				return this.m_Normal;
			}
		}

		/// <summary>
		///   <para>Incident velocity at the intersection point of the collision.</para>
		/// </summary>
		public Vector3 velocity
		{
			get
			{
				return this.m_Velocity;
			}
		}

		/// <summary>
		///   <para>The Collider for the GameObject struck by the particles.</para>
		/// </summary>
		[Obsolete("collider property is deprecated. Use colliderComponent instead, which supports Collider and Collider2D components.")]
		public Collider collider
		{
			get
			{
				return ParticleCollisionEvent.InstanceIDToCollider(this.m_ColliderInstanceID);
			}
		}

		/// <summary>
		///   <para>The Collider or Collider2D for the GameObject struck by the particles.</para>
		/// </summary>
		public Component colliderComponent
		{
			get
			{
				return ParticleCollisionEvent.InstanceIDToColliderComponent(this.m_ColliderInstanceID);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Collider InstanceIDToCollider(int instanceID);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Component InstanceIDToColliderComponent(int instanceID);
	}
}
