using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace MUtility
{
    public interface IHandleable
    {
        List<HandlePoint> GetHandles();
        void SetHandle(int index, Vector3 point);
    }

    public static class HandleableExtensions
    {
        const float defaultSize = 0.25f;
        static GUIStyle _style = new GUIStyle();
        static Vector3 _snap = new Vector3(.5f, .5f, .5f);

        public static IHandleable ApplyHandles(this IHandleable iHandleable, Transform transform, Object objectToRecord, Color color, Space space = Space.Self, float size = defaultSize)
        {
#if UNITY_EDITOR
            Undo.RecordObject(objectToRecord, "Free Move Handle");
            List<HandlePoint> handles = iHandleable.GetHandles();
            Handles.color = color;
            for (var i = 0; i < handles.Count; i++)
            {
                HandlePoint handle = handles[i];
                Vector3 pos = handle.position;

                if (space == Space.Self)
                    pos = transform.TransformPoint(pos);

                if (!  string.IsNullOrEmpty(handle.label))
                {
                    Handles.Label(pos + handle.labelShift, handles[i].label, _style);
                }

                var capFunction = handle.shape.ToCapFunction();
                Vector3 newPos = Handles.FreeMoveHandle(pos, Quaternion.identity, size, _snap, capFunction);
                if (pos == newPos) continue;
                if (space == Space.Self)
                    newPos = transform.InverseTransformPoint(newPos);
                iHandleable.SetHandle(i, newPos);
            }
#endif
            return iHandleable;
        }

#if UNITY_EDITOR
        static Handles.CapFunction ToCapFunction(this HandlePoint.Shape shape)
        {
            switch (shape)
            {
                case HandlePoint.Shape.Circle:
                    return Handles.CircleHandleCap;
                case HandlePoint.Shape.Rectangle:
                    return Handles.RectangleHandleCap;
            }
            return null;
        }
#endif
    }
}
