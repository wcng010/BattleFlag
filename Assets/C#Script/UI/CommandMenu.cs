using System;
using C_Script.Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace C_Script.UI
{
    public class CommandMenu:MonoBehaviour
    {
        [SerializeField]private GameObject AttackBut;
        [SerializeField]private GameObject MoveBut;
        [SerializeField] private GameObject RecoverBut; 
        private void OnEnable()
        {
            EventCentreManager.Instance.Subscribe(MyEventType.EntityAfter,OpenMenu);
            EventCentreManager.Instance.Subscribe(MyEventType.ReadyFinished,OpenMenu);
            EventCentreManager.Instance.Subscribe(MyEventType.OpenMenu,OpenMenu);
            EventCentreManager.Instance.Subscribe(MyEventType.CloseMenu,CloseMenu);
        }

        private void OnDisable()
        {
            EventCentreManager.Instance.Unsubscribe(MyEventType.EntityAfter,OpenMenu);
            EventCentreManager.Instance.Unsubscribe(MyEventType.ReadyFinished,OpenMenu);
            EventCentreManager.Instance.Unsubscribe(MyEventType.OpenMenu,OpenMenu);
            EventCentreManager.Instance.Unsubscribe(MyEventType.CloseMenu,CloseMenu);
        }

        //事件，打开菜单
        private void OpenMenu()
        {
            AttackBut.SetActive(true);
            MoveBut.SetActive(true);
            RecoverBut.SetActive(true);
        }

        private void CloseMenu()
        {
            AttackBut.SetActive(false);
            MoveBut.SetActive(false);
            RecoverBut.SetActive(false);
        }
    }
}