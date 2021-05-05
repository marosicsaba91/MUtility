using UnityEngine;

namespace Utility
{
    public struct HandlePoint
    {
        public enum Shape { Circle, Rectangle, LabelOnly }

        public Vector3 position;
        public string label;
        public Shape shape;
        public Vector3 labelShift;
         
        public HandlePoint(Vector3 position, Shape shape = Shape.Circle)  
        {
            this.label = null;
            this.position = position;
            this.shape = shape;
            this.labelShift = default;
        }

        public HandlePoint(Vector3 position, Shape shape, string label)
        {
            this.label = label;
            this.position = position;
            this.shape = shape;
            this.labelShift = default;
        }

        public HandlePoint(Vector3 position, Shape shape, string label, Vector3 labelShift)
        { 
            this.label = label;
            this.position = position;
            this.shape = shape;
            this.labelShift = labelShift;
        }

        public HandlePoint(Vector3 position, string label, Vector3 labelShift)
        {
            this.label = label;
            this.position = position;
            this.shape = Shape.Circle;
            this.labelShift = labelShift;
        }

        public HandlePoint(Vector3 position, string label)
        {
            this.label = label;
            this.position = position;
            this.shape = Shape.Circle;
            this.labelShift = default;
        }
    }
}
