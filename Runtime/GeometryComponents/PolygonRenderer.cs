using System.Linq;
using UnityEngine;

namespace MUtility
{

	[RequireComponent(typeof(PolygonComponent), typeof(LineRenderer))]
	public class PolygonRenderer : MonoBehaviour
	{
		[SerializeField] PolygonComponent polygonComponent;
		[SerializeField] LineRenderer lineRenderer;
		[SerializeField] DisplayMember updateLineRenderer = new(nameof(ForceUpdateLine));

		void OnValidate()
		{
			bool isNew = polygonComponent == null || lineRenderer == null;

			if (polygonComponent == null)
				polygonComponent = GetComponent<PolygonComponent>();
			if (lineRenderer == null)
				lineRenderer = GetComponent<LineRenderer>();

			if (polygonComponent == null)
				return;
			if (lineRenderer == null)
				return;


			polygonComponent.Updated -= UpdateLine;
			polygonComponent.Updated += UpdateLine;

			if (isNew)
				UpdateLine();
		}

		void ForceUpdateLine()
		{
			if (polygonComponent.Polygon is Spline spline)
				spline.ForceRecalculatePoints();

			UpdateLine();
		}

		void UpdateLine()
		{
			lineRenderer.useWorldSpace = polygonComponent.space == Space.World;


			Vector3[] array = polygonComponent.PointsLocal.ToArray();

			lineRenderer.positionCount = array.Length;
			lineRenderer.SetPositions(array);
		}
	}
}