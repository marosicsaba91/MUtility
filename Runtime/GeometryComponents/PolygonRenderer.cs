using System.Linq;
using UnityEngine;

namespace MUtility
{

    [RequireComponent(typeof(PolygonComponent), typeof(LineRenderer))]
    public class PolygonRenderer : MonoBehaviour
    {
        [SerializeField, HideInInspector] PolygonComponent polygonComponent;
        [SerializeField, HideInInspector] LineRenderer lineRenderer;
        [SerializeField] DisplayMember updateLineRenderer = new(nameof(UpdateLine));

        void OnValidate()
        {
            bool isNew = polygonComponent == null || lineRenderer == null;

            if (polygonComponent == null)
                polygonComponent = GetComponent<PolygonComponent>();
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();

            if (polygonComponent == null) return;
            if (lineRenderer == null) return;


            polygonComponent.Updated -= UpdateLine;
            polygonComponent.Updated += UpdateLine;

            if (isNew)
                UpdateLine();
        }

        void UpdateLine()
        {
            lineRenderer.useWorldSpace =
                polygonComponent.Space == PolygonComponent.PolygonSpace.WorldWithPose ||
                polygonComponent.Space == PolygonComponent.PolygonSpace.World;


            Vector3[] array = polygonComponent.PointsLocal.ToArray();

            lineRenderer.positionCount = array.Length;
            lineRenderer.SetPositions(array);
        }
    }
}