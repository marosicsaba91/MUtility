using UnityEngine;

namespace MUtility
{
	public class Collision3DEventBroadcaster : MonoBehaviour
	{
		public delegate void CollisionEvent(Collision3DEventBroadcaster transmitter, Collision collision);

		public event CollisionEvent CollisionEnter = null;
		public event CollisionEvent CollisionExit = null;
		public event CollisionEvent CollisionStay = null;

		void OnCollisionEnter(Collision collision) =>
			CollisionEnter?.Invoke(this, collision);
		void OnCollisionExit(Collision collision) =>
			CollisionExit?.Invoke(this, collision);
		void OnCollisionStay(Collision collision) =>
			CollisionStay?.Invoke(this, collision);
	}
}
