using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CustomGameEvent : UnityEvent<Component, object>
{
}

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to listen to")] public GameEvent gameEvent;

    [Tooltip("Response to invoke when Event is raised.")]
    public CustomGameEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        response.Invoke(sender, data);
    }
}