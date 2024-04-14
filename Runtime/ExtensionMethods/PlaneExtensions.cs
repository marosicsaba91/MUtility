
using UnityEngine;

namespace MUtility { 

	public static class PlaneExtensions
	{
		public static Vector3 Project(this Plane plane, Vector3 vector) =>
			vector - plane.normal * Vector3.Dot(plane.normal, vector);
	}

}