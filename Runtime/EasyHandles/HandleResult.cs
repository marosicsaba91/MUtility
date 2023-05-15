using UnityEngine;
namespace MUtility
{
	public struct HandleResult
	{
		public HandleEvent handleEvent;
		public Vector3 newPosition;
		public Vector3 clickPosition;
		public Vector3 dragPosition => newPosition - clickPosition;
	}
}