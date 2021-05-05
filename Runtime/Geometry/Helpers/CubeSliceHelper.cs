using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
public static class CubeSliceHelper
{
    static readonly int[][] nextIndex = new int[][]
    {
        /*  0 */ new int[] {4, 6, 8, 10, 1, 2, 5, 7, 9, 11, 3, 0},
        /*  1 */ new int[] {5, 7, 8, 10, 0, 3, 4, 6, 9, 11, 2, 1},
        /*  2 */ new int[] {4, 6, 9, 11, 0, 3, 5, 7, 8, 10, 1, 2},
        /*  3 */ new int[] {5, 7, 9, 11, 1, 2, 4, 6, 8, 10, 0, 3},

        /*  4 */ new int[] {0, 2, 8, 9, 5, 6, 1, 3, 10, 11, 7, 4},
        /*  5 */ new int[] {1, 3, 8, 9, 4, 7, 0, 2, 10, 11, 6, 5},
        /*  6 */ new int[] {0, 2, 10, 11, 4, 7, 1, 3, 8, 9, 4, 6},
        /*  7 */ new int[] {1, 3, 10, 11, 5, 6, 0, 2, 8, 9, 4, 7},

        /*  8 */ new int[] {0, 1, 4, 5, 9, 10, 2, 3, 6, 7, 11, 8},
        /*  9 */ new int[] {2, 3, 4, 5, 8, 11, 0, 1, 6, 7, 10, 9},
        /* 10 */ new int[] {0, 1, 6, 7, 8, 11, 2, 3, 4, 5, 9, 10},
        /* 11 */ new int[] {2, 3, 6, 7, 9, 10, 0, 1, 4, 5, 8, 11},
    };

    static LineSegment[] _segments = new LineSegment[12];
    static bool[] _tested = new bool[12];
    static List<Vector3> _intersections = new List<Vector3>();

    static int _lastIndex = 0;
    static int _testIndex = 0;
    static int _testCount = 0;

    static Plain _plain;

    static public Drawable Slice(Plain plain,
        Vector3 leftBottomBack,
        Vector3 leftBottomFront,
        Vector3 leftTopBack,
        Vector3 leftTopFront,
        Vector3 rightBottomBack,
        Vector3 rightBottomFront,
        Vector3 rightTopBack,
        Vector3 rightTopFront)
    {

        _segments[0] = new LineSegment(leftBottomBack, rightBottomBack); //  0
        _segments[1] = new LineSegment(leftBottomFront, rightBottomFront); //  1
        _segments[2] = new LineSegment(rightTopBack, leftTopBack); //  2
        _segments[3] = new LineSegment(rightTopFront, leftTopFront); //  3

        _segments[4] = new LineSegment(leftTopBack, leftBottomBack); //  4 
        _segments[5] = new LineSegment(leftTopFront, leftBottomFront); //  5
        _segments[6] = new LineSegment(rightBottomBack, rightTopBack); //  6
        _segments[7] = new LineSegment(rightBottomFront, rightTopFront); //  7

        _segments[8] = new LineSegment(leftBottomFront, leftBottomBack); //  8
        _segments[9] = new LineSegment(leftTopFront, leftTopBack); //  9
        _segments[10] = new LineSegment(rightBottomFront, rightBottomBack); // 10
        _segments[11] = new LineSegment(rightTopFront, rightTopBack); // 11

        _intersections.Clear();
        _tested.Fill(false);
        CubeSliceHelper._plain = plain;

        _lastIndex = 0;
        _testIndex = 0;
        _testCount = 0;

        while (_testCount < 12)
        {
            int testable = nextIndex[_lastIndex][_testIndex];

            if (_tested[testable])
            {
                _testIndex++;
                continue;
            }


            var segment = _segments[testable];
            Vector3? interection = plain.Intersect(segment);
            if (interection.HasValue)
            {
                _intersections.Add(interection.Value);
                _lastIndex = testable;
                _testIndex = 0;
            }
            else
            {
                _testIndex++;
            }

            _tested[testable] = true;
            _testCount++;
        }

        if (_intersections.Count >= 2)
            _intersections.Add(_intersections[0]);

        return new Drawable(_intersections.ToArray());
    }
}
}
