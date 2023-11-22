using UnityEngine;

namespace MUtility
{
	public class Trigger3DEventBroadcaster : MonoBehaviour
	{
		public delegate void TriggerEvent(Trigger3DEventBroadcaster transmitter, Collider collider);

		public event TriggerEvent TriggerEnter = null;
		public event TriggerEvent TriggerExit = null;
		public event TriggerEvent TriggerStay = null;

		void OnTriggerEnter(Collider other) =>
			TriggerEnter?.Invoke(this, other);
		void OnTriggerExit(Collider other) =>
			TriggerExit?.Invoke(this, other);
		void OnTriggerStay(Collider other) =>
			TriggerStay?.Invoke(this, other);
	}
}
