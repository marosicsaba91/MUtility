using UnityEngine;

namespace Utility.EventTransmission
{
	public class Collision2DEventTransmitter : MonoBehaviour
	{
		public delegate void Collision2DEvent(Collision2DEventTransmitter transmitter, Collision2D collision);

		public event Collision2DEvent collisionEnter2D = null;
		public event Collision2DEvent collisionExit2D = null;
		public event Collision2DEvent collisionStay2D = null;

		void OnCollisionEnter2D(Collision2D collision) =>
			collisionEnter2D?.Invoke(this, collision);
		void OnCollisionExit2D(Collision2D collision) =>
			collisionExit2D?.Invoke(this, collision);
		void OnCollisionStay2D(Collision2D collision) =>
			collisionStay2D?.Invoke(this, collision);
	}
}
