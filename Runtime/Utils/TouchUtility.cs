using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ContinuousTouch
{
	internal TouchControl touchControl;
	internal int trackingId;
	internal Vector2 startScreenPosition;

	public event Action<ContinuousTouch> Updated;
	public event Action<ContinuousTouch> Ended;
	public Vector2 StartScreenPosition => startScreenPosition;
	public Vector2 DeltaPositionInPixel => touchControl.position.ReadValue() - startScreenPosition;
	public Vector2 DeltaPositionInCm => DeltaPositionInPixel * 2.54f / Screen.dpi;
	public Vector2 DeltaPositionInInch => DeltaPositionInPixel / Screen.dpi;

	public TouchControl TouchControl => touchControl;
	public int FingerID => trackingId;
	public int TapCount => touchControl.tapCount.ReadValue();
	public Vector2 PositionInPixel => touchControl.position.ReadValue();
	public Vector2 PositionInCm => touchControl.position.ReadValue() * 2.54f / Screen.dpi;
	public Vector2 PositionInInch => touchControl.position.ReadValue() / Screen.dpi;

	internal void UpdateTouch() =>
		Updated?.Invoke(this);
	internal void InvokeEndTouch() =>
		Ended?.Invoke(this);

	public bool Raycast(Camera camera, out RaycastHit hit)
	{
		Ray ray = camera.ScreenPointToRay(touchControl.position.ReadValue());
		return Physics.Raycast(ray, out hit);
	}

	public int Raycast2D(Camera camera, Collider2D[] colliders)
	{
		Vector2 origin = camera.ScreenToWorldPoint(touchControl.position.ReadValue());
		return Physics2D.OverlapPoint(origin, ContactFilter2D.noFilter, colliders);
	}
}

class TouchUtility : MonoBehaviour
{
	public delegate void TouchEvent(ContinuousTouch touch);

	readonly Dictionary<int, ContinuousTouch> _touchesByFingerId = new();
	public IReadOnlyDictionary<int, ContinuousTouch> TouchesByFingerId => _touchesByFingerId;

	public event TouchEvent TouchStarted;
	public event TouchEvent TouchEnded;

	void Update()
	{
		Touchscreen touchscreen = Touchscreen.current;
		if (touchscreen == null) return;

		for (int i = 0; i < touchscreen.touches.Count; i++)
		{
			TouchControl touch = touchscreen.touches[i];
			TouchPhase phase = touch.phase.ReadValue();
			int id = i;
			if (!_touchesByFingerId.ContainsKey(id) && phase != TouchPhase.Ended && phase != TouchPhase.Canceled)
			{
				ContinuousTouch tc = new()
				{
					touchControl = touch,
					trackingId = id,
					startScreenPosition = touch.position.ReadValue(),
				};
				_touchesByFingerId.Add(id, tc);
				TouchStarted?.Invoke(tc);
			}
			else if (_touchesByFingerId.ContainsKey(id))
			{
				ContinuousTouch tc = _touchesByFingerId[id];
				tc.touchControl = touch;

				if (phase is TouchPhase.Ended or TouchPhase.Canceled)
				{
					_touchesByFingerId.Remove(id);
					tc.InvokeEndTouch();
					TouchEnded?.Invoke(tc);
				}
				else
					tc.UpdateTouch();
			}
		}
	}
}
