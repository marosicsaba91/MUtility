using UnityEngine;

namespace MUtility
{

	public static class Matrix4X4Extensions
	{
		public static Quaternion ToQuaternion(this Matrix4x4 m)
		{
			Quaternion q = new Quaternion();
			q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
			q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
			q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
			q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
			q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
			q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
			q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));
			return q;
		}

		public static Vector3 ToPosition(this Matrix4x4 m)
		{
			return m.GetColumn(3);
		}

		public static string ToNiceString(this Matrix4x4 matrix)
		{
			string text = "(";
			for (int y = 0; y < 4; y++)
				for (int x = 0; x < 4; x++)
				{
					if (x == 0 && y > 0)
						text += ")(";

					int i = x * 4 + y;
					float value = matrix[i];
					string s = value.ToString("0.##");
					text += s;
					if (x != 3)
						text += ", ";
				}
			text += ")";
			return text;
		}

		public static void SetTransform(this Matrix4x4 matrix, Transform transform)
		{
			transform.localPosition = matrix.GetColumn(3);
			transform.localRotation = matrix.rotation;
			transform.localScale = matrix.lossyScale;
		}

	}
}