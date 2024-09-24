
using System.Collections.Generic;
using UnityEngine;

// Use it like this:
// HashSet<Vector2Int> hashSet = new(FastVector2IntComparer.instance);
// Dictionary<Vector3Int, Object> dictionary = new(FastVector3IntComparer.instance);

public class FastVector2IntComparer : IEqualityComparer<Vector2Int>
{
	public static readonly FastVector2IntComparer instance = new();

	public bool Equals(Vector2Int x, Vector2Int y)
	{
		return x.x == y.x && x.y == y.y;
	}

	public int GetHashCode(Vector2Int v)
	{
		return v.x << 16 | (v.y & 0xFFFF);
	}
}
public class FastVector3IntComparer : IEqualityComparer<Vector3Int>
{
	public static readonly FastVector3IntComparer instance = new();

	public bool Equals(Vector3Int x, Vector3Int y)
	{
		return x.x == y.x && x.y == y.y && x.z == y.z;
	}

	public int GetHashCode(Vector3Int v)
	{
		return v.x << 16 | (v.y & 0xFFFF) << 8 | (v.z & 0xFF);
	}
}