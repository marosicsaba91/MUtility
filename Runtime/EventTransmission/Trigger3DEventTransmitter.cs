using UnityEngine;

namespace Utility.EventTransmission
{
	public class Trigger3DEventTransmitter : MonoBehaviour
	{
		public delegate void TriggerEvent(Trigger3DEventTransmitter transmitter, Collider collider);

		public event TriggerEvent triggerEnter = null;
		public event TriggerEvent triggerExit = null;
		public event TriggerEvent triggerStay = null;
		void OnTriggerEnter(Collider other) =>
			triggerEnter?.Invoke(this, other);
		void OnTriggerExit(Collider other) =>
			triggerExit?.Invoke(this, other);
		void OnTriggerStay(Collider other) =>
			triggerStay?.Invoke(this, other);
	}
}
