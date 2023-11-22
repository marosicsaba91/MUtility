using UnityEngine;

namespace MUtility
{
	public class Trigger2DEventTransmitter : MonoBehaviour
	{
		public delegate void Trigger2DEvent(Trigger2DEventTransmitter transmitter, Collider2D collider);

		public event Trigger2DEvent TriggerEnter2D = null;
		public event Trigger2DEvent TriggerExit2D = null;
		public event Trigger2DEvent TriggerStay2D = null;

		void OnTriggerEnter2D(Collider2D other) =>
			TriggerEnter2D?.Invoke(this, other);
		void OnTriggerExit2D(Collider2D other) =>
			TriggerExit2D?.Invoke(this, other);
		void OnTriggerStay2D(Collider2D other) =>
			TriggerStay2D?.Invoke(this, other);
	}
}
