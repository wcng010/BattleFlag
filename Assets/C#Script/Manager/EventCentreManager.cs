using System.Collections.Generic;
using C_Script.Base.Template;
using UnityEngine;
using UnityEngine.Events;

namespace C_Script.Manager
{
    public enum MyEventType
    {
        EntityAfter,
        ReadyFinished,
        CloseMenu,
        OpenMenu,
        MyTurn,
        EnemyTurn,
        SomeOneDeath
    }

    public class EventCentreManager: NormSingleton<EventCentreManager>
    {
     
        private static readonly IDictionary<MyEventType, UnityEvent> Events =
            new Dictionary<MyEventType, UnityEvent>(); //Events字典装有若干个事件，一一对应事件类型，
        public void Subscribe(MyEventType MyEventType, UnityAction listener)
        {
            UnityEvent thisEvent; //事件
            if (Events.TryGetValue(MyEventType, out thisEvent))
            {
                thisEvent.AddListener(listener); //向事件中添加函数
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Events.Add(MyEventType, thisEvent);
            }
        }

        public void Unsubscribe(MyEventType MyEventType, UnityAction listener)
        {
            UnityEvent thisEvent;
            if (Events.TryGetValue(MyEventType, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public void Publish(MyEventType MyEventType)
        {
            UnityEvent thisEvent;
            if (Events.TryGetValue(MyEventType, out thisEvent))
            {
                thisEvent.Invoke();
            }
            else
            {
                Debug.LogError(MyEventType + "isn't Exist");
            }
        }
    }
}