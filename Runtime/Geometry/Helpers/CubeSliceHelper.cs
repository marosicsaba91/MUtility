using System.Collections.Generic;

namespace MUtility
{
	public static class CubeSliceHelper
	{
		static readonly int[][] nextIndex = new[]
		{
        /*  0 */ new[] {4, 6, 8, 10, 1, 2, 5, 7, 9, 11, 3, 0},
        /*  1 */ new[] {5, 7, 8, 10, 0, 3, 4, 6, 9, 11, 2, 1},
        /*  2 */ new[] {4, 6, 9, 11, 0, 3, 5, 7, 8, 10, 1, 2},
        /*  3 */ new[] {5, 7, 9, 11, 1, 2, 4, 6, 8, 10, 0, 3},

        /*  4 */ new[] {0, 2, 8, 9, 5, 6, 1, 3, 10, 11, 7, 4},
        /*  5 */ new[] {1, 3, 8, 9, 4, 7, 0, 2, 10, 11, 6, 5},
        /*  6 */ new[] {0, 2, 10, 11, 4, 7, 1, 3, 8, 9, 4, 6},
        /*  7 */ new[] {1, 3, 10, 11, 5, 6, 0, 2, 8, 9, 4, 7},

        /*  8 */ new[] {0, 1, 4, 5, 9, 10, 2, 3, 6, 7, 11, 8},
        /*  9 */ new[] {2, 3, 4, 5, 8, 11, 0, 1, 6, 7, 10, 9},
        /* 10 */ new[] {0, 1, 6, 7, 8, 11, 2, 3, 4, 5, 9, 10},
        /* 11 */ new[] {2, 3, 6, 7, 9, 10, 0, 1, 4, 5, 8, 11},
	};

		static readonly LineSegment[] segments = new LineSegment[12];
		static readonly bool[] tested = new bool[12];
		static readonly List<UnityEngine.Vector3> intersections = new List<UnityEngine.Vector3>();

		static int _lastIndex = 0;
		static int _testIndex = 0;
		static int _testCount = 0;

		static Plain _plain;

		public static Drawable Slice(Plain plain,
			UnityEngine.Vector3 leftBottomBack,
			UnityEngine.Vector3 leftBottomFront,
			UnityEngine.Vector3 leftTopBack,
			UnityEngine.Vector3 leftTopFront,
			UnityEngine.Vector3 rightBottomBack,
			UnityEngine.Vector3 rightBottomFront,
			UnityEngine.Vector3 rightTopBack,
			UnityEngine.Vector3 rightTopFront)
		{

			segments[0] = new LineSegment(leftBottomBack, rightBottomBack); //  0
			segments[1] = new LineSegment(leftBottomFront, rightBottomFront); //  1
			segments[2] = new LineSegment(rightTopBack, leftTopBack); //  2
			segments[3] = new LineSegment(rightTopFront, leftTopFront); //  3

			segments[4] = new LineSegment(leftTopBack, leftBottomBack); //  4 
			segments[5] = new LineSegment(leftTopFront, leftBottomFront); //  5
			segments[6] = new LineSegment(rightBottomBack, rightTopBack); //  6
			segments[7] = new LineSegment(rightBottomFront, rightTopFront); //  7

			segments[8] = new LineSegment(leftBottomFront, leftBottomBack); //  8
			segments[9] = new LineSegment(leftTopFront, leftTopBack); //  9
			segments[10] = new LineSegment(rightBottomFront, rightBottomBack); // 10
			segments[11] = new LineSegment(rightTopFront, rightTopBack); // 11

			intersections.Clear();
			tested.Fill(false);
			_plain = plain;

			_lastIndex = 0;
			_testIndex = 0;
			_testCount = 0;

			while (_testCount < 12)
			{
				int testable = nextIndex[_lastIndex][_testIndex];

				if (tested[testable])
				{
					_testIndex++;
					continue;
				}


				LineSegment segment = segments[testable];
				UnityEngine.Vector3? intersection = plain.Intersect(segment);
				if (intersection.HasValue)
				{
					intersections.Add(intersection.Value);
					_lastIndex = testable;
					_testIndex = 0;
				}
				else
				{
					_testIndex++;
				}

				tested[testable] = true;
				_testCount++;
			}

			if (intersections.Count >= 2)
				intersections.Add(intersections[0]);

			return new Drawable(intersections.ToArray());
		}
	}
}
