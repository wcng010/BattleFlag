using System;
using System.Collections;
using C_Script.Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace C_Script.Entity.Friend
{
    public class FriendEntity : Base.Abstract.Entity
    {
        public Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public override void Move(Vector3 pos)
        {
            animator.SetBool("Move",true);
            GetMoveComponent.Move(pos);
        }

        public void Recover()
        {
            GetHealthComponent.RecoverHealth();
        }


        public override void Attack(Base.Abstract.Entity entity)
        {
            Debug.Log("PlayerAttack");
            transform.LookAt(entity.transform.position);
            animator.SetTrigger("Attack");
            GetHealthComponent.AttackHealth(entity);
            Invoke("DelayInvoke",2f);
        }

        private void DelayInvoke()
        {
            EventCentreManager.Instance.Publish(MyEventType.OpenMenu);
            GameManager.Instance.EntityAction();
        }

    }
}
