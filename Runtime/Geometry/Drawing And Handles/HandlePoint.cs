using UnityEngine;

namespace MUtility
{
    public struct HandlePoint
    {
        public enum Shape { Circle, Rectangle, LabelOnly }

        public UnityEngine.Vector3 position;
        public string label;
        public Shape shape;
        public UnityEngine.Vector3 labelShift;
         
        public HandlePoint(UnityEngine.Vector3 position, Shape shape = Shape.Circle)  
        {
            label = null;
            this.position = position;
            this.shape = shape;
            labelShift = default;
        }

        public HandlePoint(UnityEngine.Vector3 position, Shape shape, string label)
        {
            this.label = label;
            this.position = position;
            this.shape = shape;
            labelShift = default;
        }

        public HandlePoint(UnityEngine.Vector3 position, Shape shape, string label, UnityEngine.Vector3 labelShift)
        { 
            this.label = label;
            this.position = position;
            this.shape = shape;
            this.labelShift = labelShift;
        }

        public HandlePoint(UnityEngine.Vector3 position, string label, UnityEngine.Vector3 labelShift)
        {
            this.label = label;
            this.position = position;
            shape = Shape.Circle;
            this.labelShift = labelShift;
        }

        public HandlePoint(UnityEngine.Vector3 position, string label)
        {
            this.label = label;
            this.position = position;
            shape = Shape.Circle;
            labelShift = default;
        }
    }
}
