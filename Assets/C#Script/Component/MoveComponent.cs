using System.Collections;
using C_Script.Manager;
using UnityEngine;
using UnityEngine.AI;
namespace C_Script.Component
{
    public class MoveComponent:EntityComponent
    {
        private NavMeshAgent agent;
        private Animator _animator;
        private float _timer;
        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        public void Move(Vector3 pos)
        {
            _timer = 0;
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent component not found on this GameObject.");
            }
            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(pos, out navMeshHit, 10.0f, NavMesh.AllAreas);
            agent.SetDestination(navMeshHit.position);
            StartCoroutine(Stop());
        }


        private IEnumerator Stop()
        {
            while (!agent.isStopped)
            {
                yield return null;
            }
            EventCentreManager.Instance.Publish(MyEventType.OpenMenu);
            GameManager.Instance.EntityAction();
            _animator.SetBool("Move",false);
        }
    }
}