using System;
using System.Collections.Generic;
using UnityEngine;

namespace MUtility
{
    [Serializable]
    public struct Cuboid : IDrawable, IHandleable, I3DSurface, I3DVolume, I3DContaining, IMesh
    {
        public Vector3 center;
        public Vector3 size;
        public Quaternion rotation;

        public Cuboid(Camera camera)
        {
            Transform cameraTransform = camera.transform;
            Vector3 pos = cameraTransform.position;
            Vector3 forward = cameraTransform.forward;
            Vector3 near = pos + (forward * camera.nearClipPlane);
            Vector3 far = pos + (forward * camera.farClipPlane);

            center = (near + far) / 2;
            size = new Vector3(
                camera.orthographicSize * camera.aspect * 2,
                camera.orthographicSize * 2,
                camera.farClipPlane - camera.nearClipPlane);

            rotation = cameraTransform.rotation;
        }

        public Cuboid(Vector3 center, Vector3 size)
        {
            this.center = center;
            this.size = size;
            rotation = Quaternion.identity;
        }

        public Cuboid(Vector3 center, Vector3 size, Quaternion rotation)
        {
            this.center = center;
            this.size = size;
            this.rotation = rotation;
        }

        public float Volume => size.x * size.y * size.z;

        public float Surface => 2f * (size.x * size.y + size.y * size.z + size.z * size.x);
               
        public float Width => size.x;
        public float Height => size.y;
        public float Length => size.z;

        public Vector3 RightCenter => center + rotation * new Vector3((size.x / 2f), 0, 0);
        public Vector3 LeftCenter => center + rotation * new Vector3(- (size.x / 2f), 0,0);
        public Vector3 TopCenter => center + rotation * new Vector3(0, (size.y / 2f), 0);
        public Vector3 BottomCenter => center + rotation * new Vector3(0, - (size.y / 2f), 0);
        public Vector3 FrontCenter => center + rotation * new Vector3(0, 0,  (size.z / 2f));
        public Vector3 BackCenter => center + rotation * new Vector3(0, 0,- (size.z / 2f));
        
        public Vector3[] SideCenters => new[] {
            RightCenter, TopCenter, FrontCenter, LeftCenter, BottomCenter, BackCenter };

        public Vector3 RightTopFrontCorner => center + rotation * new Vector3( (size.x / 2f), (size.y / 2f), - (size.z / 2f));
        public Vector3 LeftTopFrontCorner => center + rotation * new Vector3( - (size.x / 2f), (size.y / 2f), - (size.z / 2f));
        public Vector3 RightBottomFrontCorner => center + rotation * new Vector3( (size.x / 2f), - (size.y / 2f), - (size.z / 2f));
        public Vector3 LeftBottomFrontCorner => center + rotation * new Vector3( - (size.x / 2f), - (size.y / 2f), - (size.z / 2f));
        public Vector3 RightTopBackCorner => center + rotation * new Vector3( (size.x / 2f), (size.y / 2f), (size.z / 2f));
        public Vector3 LeftTopBackCorner => center + rotation * new Vector3( - (size.x / 2f), (size.y / 2f), (size.z / 2f));
        public Vector3 RightBottomBackCorner => center + rotation * new Vector3( (size.x / 2f), - (size.y / 2f), (size.z / 2f));
        public Vector3 LeftBottomBackCorner => center + rotation * new Vector3( - (size.x / 2f), - (size.y / 2f), (size.z / 2f));

        public Vector3[] Corners => new[] {
            RightTopFrontCorner, LeftTopFrontCorner, RightBottomFrontCorner, LeftBottomFrontCorner,
            RightTopBackCorner, LeftTopBackCorner, RightBottomBackCorner, LeftBottomBackCorner};

        public Rectangle3D RightSide => new Rectangle3D(RightCenter, new Vector2(size.z, size.y), 
            Quaternion.LookRotation(rotation * Vector3.right, rotation * Vector3.up));
        public Rectangle3D LeftSide => new Rectangle3D(LeftCenter, new Vector2(size.z, size.y),
            Quaternion.LookRotation(rotation * Vector3.left, rotation * Vector3.up));
        public Rectangle3D TopSide => new Rectangle3D(TopCenter, new Vector2(size.x, size.z),
            Quaternion.LookRotation(rotation * Vector3.up, rotation * Vector3.forward));
        public Rectangle3D BottomSide => new Rectangle3D(BottomCenter, new Vector2(size.x, size.z),
            Quaternion.LookRotation(rotation * Vector3.down, rotation * Vector3.forward));
        public Rectangle3D FrontSide => new Rectangle3D(FrontCenter, new Vector2(size.x, size.y),
            Quaternion.LookRotation(rotation * Vector3.forward, rotation * Vector3.up));
        public Rectangle3D BackSide => new Rectangle3D(BackCenter, new Vector2(size.x, size.y),
            Quaternion.LookRotation(rotation * Vector3.back, rotation * Vector3.up));
        public Rectangle3D[] Sides => new[] {
            RightSide, TopSide, FrontSide, LeftSide, BottomSide, BackSide };


        public Drawable Slice(Plain plain)
        {
            return CubeSliceHelper.Slice(
                plain,
                LeftBottomBackCorner,
                LeftBottomFrontCorner, 
                LeftTopBackCorner,
                LeftTopFrontCorner,
                RightBottomBackCorner,
                RightBottomFrontCorner,
                RightTopBackCorner,
                RightTopFrontCorner);
        }

        public List<HandlePoint> GetHandles()
        { 

            return new List<HandlePoint> {
                new HandlePoint(center, HandlePoint.Shape.Rectangle),                 // 0 Center
                new HandlePoint(RightCenter),               // 1 Right
                new HandlePoint(LeftCenter),                // 2 Left
                new HandlePoint(TopCenter),                 // 3 Top
                new HandlePoint(BottomCenter),              // 4 Bottom
                new HandlePoint(BackCenter),                // 5 Back
                new HandlePoint(FrontCenter),               // 6 Front
                new HandlePoint(RightTopBackCorner, HandlePoint.Shape.Rectangle),     // 7 Rescale
            };
        }

        public bool IsPointInside(Vector3 point) =>
            Mathf.Abs(center.x - point.x) < (size.x / 2f) &&
            Mathf.Abs(center.y - point.y) < (size.y / 2f) &&
            Mathf.Abs(center.z - point.z) < (size.z / 2f);

        public void SetHandle(int i, Vector3 newPoint)
        {
            float rescale = 0;
            float offset = 0;

            switch (i)
            {
                case 0: // Center
                    center = newPoint;
                    return;
                case 1: // Right
                    rescale = (newPoint.x - center.x) - (size.x / 2f);
                    offset = (newPoint.x - (center.x + (size.x / 2))) / 2f;
                    break;
                case 2: // Left
                    rescale = (center.x - newPoint.x) - (size.x / 2f);
                    offset = (newPoint.x - (center.x - (size.x / 2))) / 2f;
                    break;
                case 3: // Top
                    rescale = (newPoint.y - center.y) - (size.y / 2f);
                    offset = (newPoint.y - (center.y + (size.y / 2))) / 2f;
                    break;
                case 4: // Bottom
                    rescale = (center.y - newPoint.y) - (size.y / 2f);
                    offset = (newPoint.y - (center.y - (size.y / 2))) / 2f;
                    break;
                case 5: // Back
                    rescale = (center.z - newPoint.z) - (size.z / 2f);
                    offset = (newPoint.z - (center.z - (size.z / 2))) / 2f;
                    break;
                case 6: // Front
                    rescale = (newPoint.z - center.z) - (size.z / 2f);
                    offset = (newPoint.z - (center.z + (size.z / 2))) / 2f;
                    break;
                case 7: // Rescale
                    float rescaleX = (newPoint.x - center.x) * 2;
                    float rescaleY = (newPoint.y - center.y) * 2;
                    float rescaleZ = (newPoint.z - center.z) * 2;
                    size = new Vector3(rescaleX, rescaleY, rescaleZ);
                    return;
            }

            switch (i)
            {
                // Horizontal
                case 1:
                case 2:
                    size = new Vector3(size.x + rescale, size.y, size.z);
                    center = new Vector3(center.x + offset, center.y, center.z);
                    break;
                // Vertical
                case 3:
                case 4:
                    size = new Vector3(size.x, size.y + rescale, size.z);
                    center = new Vector3(center.x, center.y + offset, center.z);
                    break;
                // Dept
                case 5:
                case 6:
                    size = new Vector3(size.x, size.y, size.z + rescale);
                    center = new Vector3(center.x, center.y, center.z + offset);
                    break;
            }
        }

        public void ToMesh(Mesh result) => ToMesh(result, true);

        public void ToMesh(Mesh resultMesh, bool normalOut)
        {
            resultMesh.Clear();
            const int vertexCount = 24;
            const int triangleCount = 36;
            var vertices = new Vector3[vertexCount];
            var normals = new Vector3[vertexCount];
            var uv = new Vector2[vertexCount];
            var triangles = new int[triangleCount];

            var vertexIndex = 0;
            var triangleIndex = 0; 
            Rectangle3D[] sides = Sides;
            foreach (Rectangle3D side in sides)
            { 
                Vector3[] corners = side.Corners;
                Vector3 normal = normalOut? side.Normal: -side.Normal;
                for (var cornerIndex = 0; cornerIndex < 4; cornerIndex++)
                {
                    vertices[vertexIndex] = corners[cornerIndex];
                    normals[vertexIndex] = normal;
                    uv[vertexIndex] = new Vector2(   cornerIndex == 1 || cornerIndex == 2 ? 0 : 1, cornerIndex < 2 ? 1: 0);

                    vertexIndex++;
                }

                if (normalOut)
                {
                    triangles[triangleIndex * 6 + 0] = vertexIndex - 1;
                    triangles[triangleIndex * 6 + 1] = vertexIndex - 4;
                    triangles[triangleIndex * 6 + 2] = vertexIndex - 2;

                    triangles[triangleIndex * 6 + 3] = vertexIndex - 3;
                    triangles[triangleIndex * 6 + 4] = vertexIndex - 2;
                    triangles[triangleIndex * 6 + 5] = vertexIndex - 4;
                }
                else
                {
                    triangles[triangleIndex * 6 + 0] = vertexIndex - 2;
                    triangles[triangleIndex * 6 + 1] = vertexIndex - 4;
                    triangles[triangleIndex * 6 + 2] = vertexIndex - 1;

                    triangles[triangleIndex * 6 + 3] = vertexIndex - 4;
                    triangles[triangleIndex * 6 + 4] = vertexIndex - 2;
                    triangles[triangleIndex * 6 + 5] = vertexIndex - 3;
                }
                triangleIndex++;
            }

            resultMesh.vertices = vertices;
            resultMesh.normals = normals;
            resultMesh.uv = uv;
            resultMesh.triangles = triangles;
            resultMesh.RecalculateBounds();
            resultMesh.RecalculateTangents();
            ;
        }


        public Drawable ToDrawable() => new Drawable(
            new[] { RightTopFrontCorner, LeftTopFrontCorner, LeftBottomFrontCorner, RightBottomFrontCorner, RightTopFrontCorner },
            new[] { RightTopBackCorner, LeftTopBackCorner, LeftBottomBackCorner, RightBottomBackCorner, RightTopBackCorner },
            new[] { RightTopFrontCorner, RightTopBackCorner },
            new[] { LeftTopFrontCorner, LeftTopBackCorner },
            new[] { LeftBottomFrontCorner, LeftBottomBackCorner },
            new[] { RightBottomFrontCorner, RightBottomBackCorner }
            );
    }
}
