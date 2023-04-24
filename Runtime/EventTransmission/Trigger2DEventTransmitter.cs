using UnityEngine;

namespace Utility.EventTransmission
{
	public class Trigger2DEventTransmitter : MonoBehaviour
	{
		public delegate void Trigger2DEvent(Trigger2DEventTransmitter transmitter, Collider2D collider);

		public event Trigger2DEvent triggerEnter2D = null;
		public event Trigger2DEvent triggerExit2D = null;
		public event Trigger2DEvent triggerStay2D = null;

		void OnTriggerEnter2D(Collider2D other) =>
			triggerEnter2D?.Invoke(this, other);
		void OnTriggerExit2D(Collider2D other) =>
			triggerExit2D?.Invoke(this, other);
		void OnTriggerStay2D(Collider2D other) =>
			triggerStay2D?.Invoke(this, other);
	}
}
