using System;
using UnityEngine;

namespace MUtility
{
	[Serializable]
	public class DisplayMember
	{
		[NonSerialized] public string memberName;
		[NonSerialized] public bool useMemberNameAsLabel;
		public float functionZoom = 1;
		public Vector2 functionOffset = Vector2.zero;

		public DisplayMember(string memberName, bool useMemberNameAsLabel = false)
		{
			this.memberName = memberName;
			this.useMemberNameAsLabel = useMemberNameAsLabel;
		}

	}
}