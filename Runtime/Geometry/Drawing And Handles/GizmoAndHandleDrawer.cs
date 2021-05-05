using UnityEngine;
using Utility;

namespace  Example.Geometry
{
    public class GizmoAndHandleDrawer : MonoBehaviour
    { 
        public enum DrawingType { OnDrawGizmos, OnDrawGizmosSelected, Update, Non}
  
        public Object objectToDraw;
        [Space]
        public DrawingType drawGizmos = DrawingType.OnDrawGizmosSelected;
        public bool drawingHandles = false;
        [Space]
        public Space space = Space.Self;
        public Color color = Color.red;  
        
        public void ApplyHandles()
        {
            if(!drawingHandles) return;

            if (objectToDraw is IHandleable handleableObject) 
                handleableObject.ApplyHandles(transform, objectToDraw, color, space);
            else
                Debug.LogError($"Can't draw handles to {objectToDraw.name}, because it doesn't implements IHandleable interface!", gameObject);
        }

        void Update()
        {
            if(drawGizmos != DrawingType.Update) return;

            if (objectToDraw is IDrawable handleableObject)
            {
                var drawable = handleableObject.ToDrawable();
                if(space == Space.Self)
                    drawable.Transform(transform);
                drawable.DrawDebug(color);
            }
            else
                Debug.LogError($"Can't draw object with Debug.DrawLine() to {objectToDraw.name}, because it doesn't implements IDrawable interface!", gameObject);
        }       
        
        void OnDrawGizmos()
        {
            if(drawGizmos != DrawingType.OnDrawGizmos) return;
            DrawGizmo();
        }     
        
        void OnDrawGizmosSelected()
        {
            if(drawGizmos != DrawingType.OnDrawGizmosSelected) return;
            DrawGizmo();
        }

        void DrawGizmo()
        {
            if (objectToDraw is IDrawable handleableObject)
            {
                var drawable = handleableObject.ToDrawable();
                if (space == Space.Self)
                    drawable.Transform(transform);
                drawable.DrawGizmo(color);
            }
            else
                Debug.LogError($"Can't draw gizmos to {objectToDraw.name}, because it doesn't implements IDrawable interface!",
                    gameObject);
        }
    }
}