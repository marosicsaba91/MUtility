using UnityEngine;

namespace Utility.EventTransmission
{
	public class Collision3DEventTransmitter : MonoBehaviour
	{
		public delegate void CollisionEvent(Collision3DEventTransmitter transmitter, Collision collision);

		public event CollisionEvent collisionEnter = null;
		public event CollisionEvent collisionExit = null;
		public event CollisionEvent collisionStay = null;
		void OnCollisionEnter(Collision collision) =>
			collisionEnter?.Invoke(this, collision);
		void OnCollisionExit(Collision collision) =>
			collisionExit?.Invoke(this, collision);
		void OnCollisionStay(Collision collision) =>
			collisionStay?.Invoke(this, collision);
	}
}
