using System;
using System.Collections;
using System.Collections.Generic;
using C_Script.Manager;
using UnityEngine;
using UnityEngine.UI;

public class RoundShow : MonoBehaviour
{
    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        EventCentreManager.Instance.Subscribe(MyEventType.MyTurn,MyRound);
        EventCentreManager.Instance.Subscribe(MyEventType.EnemyTurn,EnemyRound);
    }

    private void OnDisable()
    {
        EventCentreManager.Instance.Unsubscribe(MyEventType.MyTurn,MyRound);
        EventCentreManager.Instance.Unsubscribe(MyEventType.EnemyTurn,EnemyRound);
    }
    
    private void MyRound()
    {
        _text.text = "My Turn: " + (GameManager.Instance.RoundNum-1);
        _text.color = Color.green;
    }

    private void EnemyRound()
    {
        _text.text = "Enemy Turn: " + (GameManager.Instance.RoundNum-1);
        _text.color = Color.red;
    }
}
