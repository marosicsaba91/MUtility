﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class ButtonEventTransmitter : MonoBehaviour,

    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
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


    public event Action<PointerEventData> OnClick;
    public event Action<PointerEventData> OnDown;
    public event Action<PointerEventData> OnUp;
    public event Action<PointerEventData> OnEnter;
    public event Action<PointerEventData> OnExit;

    readonly List<DelayedSubscriber> _delayedSubscribersOnClick = new List<DelayedSubscriber>();
    readonly List<DelayedSubscriber> _delayedSubscribersOnDown= new List<DelayedSubscriber>();
    readonly List<DelayedSubscriber> _delayedSubscribersOnUp = new List<DelayedSubscriber>();
    readonly List<DelayedSubscriber> _delayedSubscribersOnEnter = new List<DelayedSubscriber>();
    readonly List<DelayedSubscriber> _delayedSubscribersOnExit = new List<DelayedSubscriber>();

    public void SubscribeOnClickDelayed(Action<PointerEventData> callback, float delay) =>
        _delayedSubscribersOnClick.Add(new DelayedSubscriber(this, callback, delay));
    public void SubscribeOnDownDelayed(Action<PointerEventData> callback, float delay) =>
        _delayedSubscribersOnDown.Add(new DelayedSubscriber(this, callback, delay));
    public void SubscribeOnUpDelayed(Action<PointerEventData> callback, float delay) =>
        _delayedSubscribersOnUp.Add(new DelayedSubscriber(this, callback, delay));
    public void SubscribeOnEnterDelayed(Action<PointerEventData> callback, float delay) =>
        _delayedSubscribersOnEnter.Add(new DelayedSubscriber(this, callback, delay));
    public void SubscribeOnExitDelayed(Action<PointerEventData> callback, float delay) =>
        _delayedSubscribersOnExit.Add(new DelayedSubscriber(this, callback, delay));

    public void OnPointerClick(PointerEventData eventData) 
    { 
        OnClick?.Invoke(eventData);
        EventHappened(eventData, _delayedSubscribersOnClick);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown?.Invoke(eventData);
        EventHappened(eventData, _delayedSubscribersOnDown);
    }
    public void OnPointerUp(PointerEventData eventData) 
    { 
        OnUp?.Invoke(eventData);
        EventHappened(eventData, _delayedSubscribersOnUp);
    }
    public void OnPointerEnter(PointerEventData eventData) 
    {
        OnEnter?.Invoke(eventData);
        EventHappened(eventData, _delayedSubscribersOnEnter);
    }
    public void OnPointerExit(PointerEventData eventData)
    { 
        OnExit?.Invoke(eventData);
        EventHappened(eventData, _delayedSubscribersOnExit);
    }

    static void EventHappened(PointerEventData eventData, List<DelayedSubscriber> subscribers)
    {
        foreach (DelayedSubscriber subscriber in subscribers)
            subscriber.EventHappened(eventData);
    }
    
    class DelayedSubscriber
    {
        readonly Action<PointerEventData> _callback;
        readonly float _delay;
        readonly MonoBehaviour _parent;

        public DelayedSubscriber(MonoBehaviour parent, Action<PointerEventData> callback, float delay)
        {
            this._parent = parent;
            this._callback = callback;
            this._delay = delay;
        }

        public void EventHappened(PointerEventData eventData)
        {
            _parent.StartCoroutine(DelayedCallBack(eventData));
        }

        IEnumerator DelayedCallBack(PointerEventData eventData)
        {
            yield return new WaitForSeconds(_delay);
            _callback.Invoke(eventData);
        }
    }
}