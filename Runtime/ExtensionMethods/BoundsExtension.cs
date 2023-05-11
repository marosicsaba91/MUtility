using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MUtility
{
	public static class BoundsExtension
	{

		public static Bounds Transform(this Bounds bounds, Transform transform)
		{
			Bounds b = bounds;
			b.center = transform.TransformPoint(b.center);
			b.extents = transform.TransformVector(b.extents);
			return b;
		}


		public static Bounds Transform(this Bounds bounds, Vector3 offset, Quaternion rotate)
		{
			Bounds b = bounds;
			b.center = rotate * b.center + offset;
			b.extents = rotate * b.extents;
			return b;
		}


		public static Bounds Transform(this Bounds bounds, Vector3 offset, Quaternion rotate, Vector3 scale)
		{
			Bounds b = bounds;
			b.center = rotate * b.center + offset;
			b.extents = rotate * b.extents;
			b.extents.Scale(scale);
			return b;
		}


		public static bool Contains(this BoundsInt bounds, int x, int y, int z)
		{
			Vector3Int min = bounds.min;
			Vector3Int max = bounds.max;
			return
				x >= min.x && x < max.x &&
				y >= min.y && y < max.y &&
				z >= min.z && z < max.z;
		}

		public static Vector3 GetRandom(this BoundsInt bounds)
		{
			Vector3Int min = bounds.min;
			Vector3Int max = bounds.max;
			int x = Random.Range(min.x, max.x + 1);
			int y = Random.Range(min.y, max.y + 1);
			int z = Random.Range(min.z, max.z + 1);
			return new Vector3(x, y, z);
		}

		public static void Clamp(this ref BoundsInt bounds, Vector3Int min, Vector3Int max)
		{
			bounds.min = Vector3Int.Max(bounds.min, min);
			bounds.max = Vector3Int.Min(bounds.max, max);
		}

		public static void AddPoint(this ref BoundsInt bounds, Vector3Int point)
		{
			bounds.min = Vector3Int.Min(point, bounds.min);
			bounds.max = Vector3Int.Max(point, bounds.max);
		}
		public static void Combine(this ref BoundsInt bounds, BoundsInt other)
		{
			bounds.min = Vector3Int.Min(bounds.min, other.min);
			bounds.max = Vector3Int.Max(bounds.max, other.max);
		}

		public static IEnumerable<Vector3Int> WalkThrough(this BoundsInt bounds)
		{
			Vector3Int min = bounds.min;
			Vector3Int max = bounds.max;
			for (int x = min.x; x < max.x; x++)
				for (int y = min.y; y < max.y; y++)
					for (int z = min.z; z < max.z; z++)
						yield return new Vector3Int(x, y, z);
		}

		public static IEnumerable<(Vector3, Vector3)> GetLines(this Bounds bounds) =>
			GetBoundLines(bounds.min, bounds.size);

		public static IEnumerable<(Vector3, Vector3)> GetLines(this BoundsInt bounds) =>
			GetBoundLines(bounds.min, bounds.size);

		static IEnumerable<(Vector3, Vector3)> GetBoundLines(Vector3 origin, Vector3 size)
		{
			float x0 = origin.x;
			float x1 = origin.x + size.x;
			float y0 = origin.y;
			float y1 = origin.y + size.y;
			float z0 = origin.z;
			float z1 = origin.z + size.z;

			Vector3 p000 = new(x0, y0, z0);
			Vector3 p001 = new(x0, y0, z1);
			Vector3 p010 = new(x0, y1, z0);
			Vector3 p011 = new(x0, y1, z1);
			Vector3 p100 = new(x1, y0, z0);
			Vector3 p101 = new(x1, y0, z1);
			Vector3 p110 = new(x1, y1, z0);
			Vector3 p111 = new(x1, y1, z1);

			yield return (p000, p010);
			yield return (p010, p110);
			yield return (p110, p100);
			yield return (p100, p000);
			yield return (p000, p001);
			yield return (p010, p011);
			yield return (p100, p101);
			yield return (p110, p111);
			yield return (p001, p011);
			yield return (p011, p111);
			yield return (p111, p101);
			yield return (p101, p001);
		}

		public static int GetVolume(this BoundsInt bounds) => bounds.size.x * bounds.size.y * bounds.size.z;

		
		public static BoundsInt GetSection(this BoundsInt one, BoundsInt other)
		{
			var min = Vector3Int.Max(one.min, other.min);
			var max = Vector3Int.Min(one.max, other.max);
			Vector3Int size = max - min;
			return new BoundsInt(min, max - min);
		}

		public static BoundsInt Resize(this BoundsInt bounds, GeneralDirection3D direction, int steps) 
		{
			Vector3Int change = direction.ToVectorInt() * steps;

			if (direction.IsPositive())
				bounds.size += change;
			else
				bounds.min += change;

			return bounds;
		}

		public static BoundsInt ResizeWithLimits(this BoundsInt bounds, GeneralDirection3D direction, int steps,
			Vector3Int minPositionLimit, Vector3Int maxPositionLimit, Vector3Int minSizeLimit)
		{
			if (steps == 0)
				return bounds;

			if (steps < 0)
			{
				int minSize  = minSizeLimit.GetAxis(direction.GetAxis());
				int size = bounds.size.GetAxis(direction.GetAxis());
				steps = Mathf.Max(steps, minSize - size);
			}

			Vector3Int change = direction.ToVectorInt() * steps;

			if (direction.IsPositive())
			{
				bounds.max += change;
			}
			else
			{
				bounds.min += change;
			}

			bounds.max = Vector3Int.Min(bounds.max, maxPositionLimit);
			bounds.min = Vector3Int.Max(bounds.min, minPositionLimit);

			return bounds;
		}

		public static BoundsInt MoveWithLimits(this BoundsInt bounds, GeneralDirection3D direction, int steps,
			Vector3Int minPositionLimit, Vector3Int maxPositionLimit)
		{
			if (steps == 0)
				return bounds;

			Vector3Int change = direction.ToVectorInt() * steps;
			change = Vector3Int.Max(change, minPositionLimit - bounds.min);
			change = Vector3Int.Min(change, maxPositionLimit - bounds.max);

			bounds.position += change;
			 
			return bounds;
		}


	}
}