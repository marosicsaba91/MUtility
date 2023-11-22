using UnityEngine;

namespace MUtility
{
	public class Collision2DEventBroadcaster : MonoBehaviour
	{
		public delegate void Collision2DEvent(Collision2DEventBroadcaster transmitter, Collision2D collision);

		public event Collision2DEvent CollisionEnter2D = null;
		public event Collision2DEvent CollisionExit2D = null;
		public event Collision2DEvent CollisionStay2D = null;

		void OnCollisionEnter2D(Collision2D collision) =>
			CollisionEnter2D?.Invoke(this, collision);
		void OnCollisionExit2D(Collision2D collision) =>
			CollisionExit2D?.Invoke(this, collision);
		void OnCollisionStay2D(Collision2D collision) =>
			CollisionStay2D?.Invoke(this, collision);
	}
}
