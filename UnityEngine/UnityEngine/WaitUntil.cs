using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Suspends the coroutine execution until the supplied delegate evaluates to true.</para>
	/// </summary>
	public sealed class WaitUntil : CustomYieldInstruction
	{
		private Func<bool> m_Predicate;

		public override bool keepWaiting
		{
			get
			{
				return !this.m_Predicate();
			}
		}

		public WaitUntil(Func<bool> predicate)
		{
			this.m_Predicate = predicate;
		}
	}
}
