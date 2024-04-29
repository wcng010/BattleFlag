using System;
using System.Collections;
using System.Collections.Generic;
using C_Script.Component;
using C_Script.Data;
using C_Script.Entity.Enemy;
using C_Script.Entity.Friend;
using C_Script.Manager;
using UnityEngine;
using UnityEngine.AI;

namespace C_Script.MyTest
{
    public class MoveTest : Base.Abstract.Entity
    {
        private Animator _animator;
        
        private GameObject[] _playerEntities;

        private Transform _selectedPlayer;

        private Transform _selectedGrid;

        private Vector3 _dir;

        private NavMeshAgent _agent;

        private string _currentGridName;

        private int _passGridNum;
        
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        public override void Move(Vector3 pos)
        {
            
        }

        public override void Attack(Base.Abstract.Entity enemy)
        {
            _animator.SetTrigger(Attack1);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EnemyBehaviour();
            }
            Debug.Log(_agent.destination);
        }


        //怪物逻辑
        //如果攻击范围内有角色，则触发攻击动画，攻击
        //如果攻击范围内没有角色，则向最近我方对象是否
        public virtual void EnemyBehaviour()
        {
            _passGridNum = 0;
            SelectNearestPlayer();
            //判断SelectedPlayer是否处于攻击范围组件内，若在则发动攻击
            GetAttackRangeComponent.TestRange();
            //GetAttackRangeComponent.CloseRange();
            //判断中终点格子是否与girds中的格子同名。
            var gridTrans = GetRayComponent.GetPlayerGrid(_selectedPlayer.position).transform.parent;
            string name =  gridTrans.parent.name+gridTrans.name;
            //如果在攻击范围内则
            if (IsSelectPlayerOnRange(name))
            {
                Debug.Log("Attack");
                Attack(_selectedPlayer.GetComponent<FriendEntity>());
            }
            else
            {
                SelectNearestGrid(_selectedPlayer.transform.position);
                _animator.SetBool("Move",true);
                _agent.isStopped = false;
                _agent.destination =  new Vector3(_selectedGrid.transform.position.x,transform.position.y,_selectedGrid.transform.position.z);
                StartCoroutine(nameof(GridsPassNum));
            }
            //GameManager.Instance.CheakRoundEnd();
            //若没有结束则
            //EventCentreManager.Instance.Publish(MyEventType.EntityAfter);
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

        private void Awake()
        {
            _playerEntities = new GameObject[GameObject.FindGameObjectsWithTag("friend").Length];
            _playerEntities = GameObject.FindGameObjectsWithTag("friend");
            _animator = transform.GetComponent<Animator>();
            _agent = transform.GetComponent<NavMeshAgent>();
        }
        
        //寻找直线距离最近的Player的随机八个点。
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
                    _dir = dirTemp;
                }
            }
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
        
        //角色向下方发射一条射线，检测经过的盒子数，当前角色的经过的格子数==角色移动距离时，停止并将目的地改为当前指向的盒子中心。
        IEnumerator GridsPassNum()
        {
            //将当前脚下的方块名赋予_currentGridName;
            string gridName = GetRayComponent.GetGridName();
            //目标点
            Vector3 destination;
            //直到方块名不为null
            while (gridName == default) { yield return null;}
            //将起点脚底方块名赋给当前方块
            _currentGridName = gridName;
            //循环执行，获取脚底方块名，如不为null，且与当前方块名不匹配，
            while (true)
            {
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
                    //获取当前脚下的格子的中心点。
                    Vector3 gridCentre = GetRayComponent.GetFootGridHit().collider.transform.parent.position;
                    Vector3 rightGridCentre = new Vector3(gridCentre.x+2.39f,gridCentre.y,gridCentre.z);
                    Vector3 upperGridCentre = new Vector3(gridCentre.x, gridCentre.y, gridCentre.z + 2.25f);
                    Vector3 leftGridCentre = new Vector3(gridCentre.x - 2.39f, gridCentre.y, gridCentre.z);
                    Vector3 BelowGridCentre = new Vector3(gridCentre.x,gridCentre.y,gridCentre.z - 2.25f);
                    gridCentre = transform.position;
                    //选取右和上
                    if (_dir.x > 0 && _dir.z > 0)
                    {
                        RaycastHit raycastHit = GetRayComponent.GetGridUpDown(upperGridCentre);
                        if(raycastHit.collider==null) raycastHit = GetRayComponent.GetGridUpDown(rightGridCentre);
                        if (raycastHit.collider == null) raycastHit = GetRayComponent.GetGridUpDown(leftGridCentre);
                        if (raycastHit.collider == null) raycastHit = GetRayComponent.GetGridUpDown(BelowGridCentre);
                        if (raycastHit.collider != null)
                        {
                            gridCentre = raycastHit.collider.transform.parent.position;
                        }
                    }
                    //选取右下
                    else if (_dir.x > 0 && _dir.z < 0)
                    {
                        RaycastHit raycastHit = GetRayComponent.GetGridUpDown(BelowGridCentre);
                        if(raycastHit.collider==null) raycastHit = GetRayComponent.GetGridUpDown(rightGridCentre);
                        if (raycastHit.collider == null) raycastHit = GetRayComponent.GetGridUpDown(leftGridCentre);
                        if (raycastHit.collider == null) raycastHit = GetRayComponent.GetGridUpDown(upperGridCentre);
                        if (raycastHit.collider != null)
                        {
                            gridCentre = raycastHit.collider.transform.parent.position;
                        }
                    }
                    //选取左上
                    else if (_dir.x < 0 && _dir.z > 0)
                    {
                        RaycastHit raycastHit = GetRayComponent.GetGridUpDown(upperGridCentre);
                        if(raycastHit.collider==null) raycastHit = GetRayComponent.GetGridUpDown(leftGridCentre);
                        if (raycastHit.collider == null) raycastHit = GetRayComponent.GetGridUpDown(rightGridCentre);
                        if (raycastHit.collider == null) raycastHit = GetRayComponent.GetGridUpDown(BelowGridCentre);
                        if (raycastHit.collider != null)
                        {
                            gridCentre = raycastHit.collider.transform.parent.position;
                        }
                    }
                    //选取左下
                    else if (_dir.x < 0 && _dir.z < 0)
                    {
                        RaycastHit raycastHit = GetRayComponent.GetGridUpDown(BelowGridCentre);
                        if(raycastHit.collider==null) raycastHit = GetRayComponent.GetGridUpDown(leftGridCentre);
                        if (raycastHit.collider == null) raycastHit = GetRayComponent.GetGridUpDown(rightGridCentre);
                        if (raycastHit.collider == null) raycastHit = GetRayComponent.GetGridUpDown(upperGridCentre);
                        if (raycastHit.collider != null)
                        {
                            gridCentre = raycastHit.collider.transform.parent.position;
                        }
                    }
                    //根据两者
                    /*destination = new Vector3(gridCentre.x, transform.position.y, gridCentre.z);
                    Debug.Log(1);
                    _agent.destination = destination;*/
                    Debug.Log(1);
                    _agent.isStopped = true;
                    _animator.SetBool("Move",false);
                    break;
                }
                yield return null;
            }
            
            while (true)
            {
                if (Vector3.Distance(transform.position, _agent.destination) < _agent.stoppingDistance)
                {
                    _agent.isStopped = true;
                    _animator.SetBool("Move",false);
                    break;
                }
                yield return new WaitForSeconds(0.05f);
            }
            //GameManager.Instance.CheakRoundEnd();
        }
    }
}
