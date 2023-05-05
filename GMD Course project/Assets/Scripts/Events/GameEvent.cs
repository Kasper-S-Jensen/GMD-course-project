using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> listeners = new();


    //overloading raise to allow for sending data and information of sender. This is optional
    public void Raise()
    {
        Raise(null, null);
    }

    public void Raise(object data)
    {
        Raise(null, data);
    }

    public void Raise(Component sender)
    {
        Raise(sender, null);
    }

    public void Raise(Component sender, object data)
    {
        for (var i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(sender, data);
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}