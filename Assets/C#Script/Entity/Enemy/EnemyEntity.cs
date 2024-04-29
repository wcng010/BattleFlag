using System;
using System.Collections;
using System.Collections.Generic;
using C_Script.Component;
using C_Script.Entity.Friend;
using C_Script.Manager;
using UnityEngine;
using UnityEngine.AI;

namespace C_Script.Entity.Enemy
{
    public class EnemyEntity : Base.Abstract.Entity
    {
        private Animator _animator;
        
        private GameObject[] _playerEntities;

        private Transform _selectedPlayer;
        
        private Transform _selectedGrid;
        
        private NavMeshAgent _agent;

        private string _currentGridName;

        private int _passGridNum;
        
        private static readonly int Attack1 = Animator.StringToHash("Attack");

        public override void Move(Vector3 pos) { }
        public override void Attack(Base.Abstract.Entity enemy)
        {
            transform.LookAt(enemy.transform.position);
            _animator.SetTrigger(Attack1);
            //敌人扣血
            GetHealthComponent.AttackHealth(enemy);
            //enemy.GetHealthComponent.ReduceHealth(EntityDataSo);
        }


        private void DelayClose() => GetAttackRangeComponent.CloseRange();

        //怪物逻辑
        //如果攻击范围内有角色，则触发攻击动画，攻击
        //如果攻击范围内没有角色，则向最近我方对象是否
        public virtual void EnemyBehaviour()
        {
            _passGridNum = 0;
            SelectNearestPlayer();
            //判断SelectedPlayer是否处于攻击范围组件内，若在则发动攻击
            GetAttackRangeComponent.TestRange();
            //判断中终点格子是否与girds中的格子同名。
            Debug.Log(_selectedPlayer.position);
            var gridTrans = GetRayComponent.GetPlayerGrid(_selectedPlayer.position).transform.parent;
            string name =  gridTrans.parent.name+gridTrans.name;
            //如果在攻击范围内则
            if (IsSelectPlayerOnRange(name))
            {
                GetAttackRangeComponent.ShowRange();
                Attack(_selectedPlayer.GetComponent<FriendEntity>());
                Invoke(nameof(DelayClose),1f);
            }
            else
            {
                SelectNearestGrid(_selectedPlayer.transform.position);
                _animator.SetBool("Move",true);
                Invoke(nameof(DelayCloseMove),3f);
                _agent.isStopped = false;
                _agent.destination =new Vector3(_selectedGrid.position.x,transform.position.y,_selectedGrid.transform.position.z);
                StartCoroutine(nameof(GridsPassNum));
            }
            GameManager.Instance.CheakRoundEnd();
            //若没有结束则
            EventCentreManager.Instance.Publish(MyEventType.EntityAfter);
            EventCentreManager.Instance.Publish(MyEventType.OpenMenu);
        }


        private void DelayCloseMove() => _animator.SetBool("Move", false);
        
        
        private void Awake()
        {
            _playerEntities = new GameObject[GameObject.FindGameObjectsWithTag("friend").Length];
            _playerEntities = GameObject.FindGameObjectsWithTag("friend");
            _animator = transform.GetComponent<Animator>();
            _agent = transform.GetComponent<NavMeshAgent>();
        }
        
        
        private void SelectNearestGrid(Vector3 pos)
        {
            //判断所选择Player的四周的Grid是否没有空，为空则将Grid设置为所选择的Grid
            RaycastHit hit = GetRayComponent.GetEmptyGrid(pos);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.transform.parent.name);
                _selectedGrid = hit.collider.transform;
            }
            else
            {
                SelectNearestGrid(pos);
            }
        }
        
        //寻找直线距离最近的Player，将他选中
        private void SelectNearestPlayer()
        {
            float distance = Single.MaxValue;
            foreach (var player in _playerEntities)
            {
                float temp;
                Vector3 dirTemp = player.transform.position - transform.position;
                temp = dirTemp.magnitude;
                if(temp<0)Debug.Log("Error");
                if (temp < distance)
                {
                    distance = temp;
                    _selectedPlayer = player.transform;
                }
            }
        }
        //角色向下方发射一条射线，检测经过的盒子数，当前角色的经过的格子数==角色移动距离时，停止并将目的地改为当前指向的盒子中心。
 //角色向下方发射一条射线，检测经过的盒子数，当前角色的经过的格子数==角色移动距离时，停止并将目的地改为当前指向的盒子中心。
        IEnumerator GridsPassNum()
        {
            //将当前脚下的方块名赋予_currentGridName;
            string gridName = GetRayComponent.GetGridName();
            //直到方块名不为null
            while (gridName == default) { yield return null;}
            //将起点脚底方块名赋给当前方块
            _currentGridName = gridName;
            //循环执行，获取脚底方块名，如不为null，且与当前方块名不匹配，
            while (true)
            {
                //Debug.Log("找方块循环"+_passGridNum);
                gridName = GetRayComponent.GetGridName();
                if (gridName != default && _currentGridName != gridName)
                {
                    _currentGridName = gridName;
                    _passGridNum++;
                }
                if (Vector3.Distance(transform.position, _agent.destination) < _agent.stoppingDistance) 
                {
                    break;
                }
                //如果移动的格子数为最后一格
                if (_passGridNum >= EntityDataSo.MoveRange-1)
                {
                    var hit = GetRayComponent.GetPlayerGrid(transform.position);
                    _agent.destination = hit.collider.transform.position;
                    break;
                }
                yield return null;
            }
            
            while (true)
            {
                if (Vector3.Distance(transform.position, _agent.destination) < _agent.stoppingDistance)
                {
                    _animator.SetBool("Move",false);
                    _agent.isStopped = true;
                    break;
                }
                yield return new WaitForSeconds(0.05f);
            }
        }

        private bool IsSelectPlayerOnRange(string name)
        {
            //获得攻击范围内的格子
            List<GameObject> grids = GetAttackRangeComponent.GetRangeFloor();
            foreach (var grid in grids)
            {
                if (grid.transform.parent.name+grid.name==name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
