using System;
using System.Collections;
using System.Collections.Generic;
using C_Script.Base.Abstract;
using UnityEngine;

public class SortTest : MonoBehaviour
{
    [SerializeField]private List<Entity> _objs = new List<Entity>();
    private void Awake()
    {
        GameObject[] friend = GameObject.FindGameObjectsWithTag("friend");
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var obj in friend)
        {
            _objs.Add(obj.GetComponent<Entity>());
        }
        foreach (var obj in enemy)
        {
            _objs.Add(obj.GetComponent<Entity>());
        }
        _objs.Sort((a, b) =>
            {
                return a.EntityDataSo.Speed
                    .CompareTo(b.EntityDataSo.Speed);
            }
        );
        _objs.Reverse();
    }
}
