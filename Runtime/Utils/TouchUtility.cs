using System;
using System.Collections.Generic;
using UnityEngine;
public class ContinuousTouch
{
	internal Touch singleTouchEvent;
	internal Vector2 startScreenPosition;

	public event Action<ContinuousTouch> Updated;
	public event Action<ContinuousTouch> Ended;
	public Vector2 StartScreenPosition => startScreenPosition;
	public Vector2 DeltaPositionInPixel => singleTouchEvent.position - startScreenPosition;
	public Vector2 DeltaPositionInCm => DeltaPositionInPixel * 2.54f / Screen.dpi;
	public Vector2 DeltaPositionInInch => DeltaPositionInPixel / Screen.dpi;

	public Touch SingleTouchEvent => singleTouchEvent;
	public int FingerID => singleTouchEvent.fingerId;
	public int TapCount => singleTouchEvent.tapCount;
	public Vector2 PositionInPixel => singleTouchEvent.position;
	public Vector2 PositionInCm => singleTouchEvent.position * 2.54f / Screen.dpi;
	public Vector2 PositionInInch => singleTouchEvent.position / Screen.dpi;

	internal void UpdateTouch() =>
		Updated?.Invoke(this);
	internal void InvokeEndTouch() =>
		Ended?.Invoke(this);

	public bool Raycast(Camera camera, out RaycastHit hit)
	{
		Ray ray = camera.ScreenPointToRay(singleTouchEvent.position);
		return Physics.Raycast(ray, out hit);
	}

	public int Raycast2D(Camera camera, Collider2D[] colliders)
	{
		Vector2 origin = camera.ScreenToWorldPoint(singleTouchEvent.position);
		ContactFilter2D filter = new ContactFilter2D().NoFilter();
		return Physics2D.OverlapPoint(origin, filter, colliders);
	}
}

class TouchUtility : MonoBehaviour
{

	public delegate void TouchEvent(ContinuousTouch touch);

	readonly Dictionary<int, ContinuousTouch> touchesByFingerId = new();
	public IReadOnlyDictionary<int, ContinuousTouch> TouchesByFingerId => touchesByFingerId;

	public event TouchEvent TouchStarted;
	public event TouchEvent TouchEnded;

	void Update()
	{
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			TouchPhase phase = touch.phase;
			int id = touch.fingerId;
			if (!touchesByFingerId.ContainsKey(id) && phase != TouchPhase.Ended && phase != TouchPhase.Canceled)
			{
				ContinuousTouch tc = new()
				{
					singleTouchEvent = touch,
					startScreenPosition = touch.position,
				};
				touchesByFingerId.Add(id, tc);
				TouchStarted?.Invoke(tc);
			}
			else
			{
				ContinuousTouch tc = touchesByFingerId[id];
				tc.singleTouchEvent = touch;

				if (phase is TouchPhase.Ended or TouchPhase.Canceled)
				{
					touchesByFingerId.Remove(id);
					tc.InvokeEndTouch();
					TouchEnded?.Invoke(tc);
				}
				else
					tc.UpdateTouch();
			}
		}
	}
}
