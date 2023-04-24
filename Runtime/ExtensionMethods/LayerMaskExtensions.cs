using UnityEngine;

namespace MUtility
{
	public static class LayerMaskExtensions
	{
		public static bool Contains(this LayerMask mask, int layer)
		{
			return (mask.value & (1 << layer)) > 0;
		}
	}
}
