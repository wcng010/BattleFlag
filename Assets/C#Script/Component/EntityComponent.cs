using System;
using C_Script.Data;
using C_Script.Entity.Enemy;
using UnityEngine;

namespace C_Script.Component
{
    public class EntityComponent :MonoBehaviour
    {
        protected EntityDataSo DataSo =>
            _dataSo ? _dataSo : _dataSo = GetComponent<Base.Abstract.Entity>().EntityDataSo;
        private EntityDataSo _dataSo;

        protected virtual void Awake()
        {
            GetComponent<Base.Abstract.Entity>().AddComponent(this);
        }
    }
}