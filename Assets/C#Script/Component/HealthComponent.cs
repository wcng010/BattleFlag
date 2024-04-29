using C_Script.Data;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using C_Script.Entity.Enemy;

namespace C_Script.Component
{
    public class HealthComponent:EntityComponent
    {
        //传入敌方实体组件
        public void AttackHealth(Base.Abstract.Entity enemy)
        {
            if (enemy.EntityDataSo.CurrentHealth - DataSo.Attack > 0)
            {
                enemy.EntityDataSo.CurrentHealth -= DataSo.Attack;
                enemy.transform.GetComponentInChildren<NewHPBar>().UpdateHealthBar();
            }
            else
            {
                enemy.gameObject.SetActive(false);
                GameManager.Instance.Sort();
            }
        }


        public void RecoverHealth()
        {
            if (DataSo.CurrentHealth + 2 > DataSo.MaxHealth)
            {
                DataSo.CurrentHealth = DataSo.MaxHealth;
            }
            else
            {
                DataSo.CurrentHealth += 2;
            }
            transform.GetComponentInChildren<NewHPBar>().UpdateHealthBar();
        }
    }
}