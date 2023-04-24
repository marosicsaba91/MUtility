using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEventTransmitter : MonoBehaviour,
	IPointerClickHandler,
	IPointerDownHandler,
	IPointerUpHandler,
	IDragHandler,
	IPointerEnterHandler,
	IPointerExitHandler
{

	Button _button;

	public Button Button
	{
		get
		{
			if (_button == null)
				_button = GetComponent<Button>();
			return _button;
		}
	}

	public event Action<PointerEventData> OnEnter;
	public event Action<PointerEventData> OnExit;
	public event Action<PointerEventData> OnDown;
	public event Action<PointerEventData> OnUp;
	public event Action<PointerEventData> OnDrag;
	public event Action<PointerEventData> OnClick;

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClick?.Invoke(eventData);
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		OnDown?.Invoke(eventData);
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		OnUp?.Invoke(eventData);
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		OnEnter?.Invoke(eventData);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		OnExit?.Invoke(eventData);
	}
	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		OnDrag?.Invoke(eventData);
	}

}