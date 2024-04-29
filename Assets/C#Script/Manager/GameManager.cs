using System;
using System.Collections;
using System.Collections.Generic;
using C_Script.Base.Abstract;
using C_Script.Base.Template;
using C_Script.Component;
using C_Script.Entity.Enemy;
using C_Script.Entity.Friend;
using C_Script.Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


//处理点击问题
public class GameManager : HungrySingleton<GameManager>
{
    //移动回合角色
    [SerializeField]private List<Entity> Entities = new List<Entity>();
    //当前回合的实体对象
    [field:SerializeField]public Entity CurrentEntity { get; set;}
    
    //第几个敌人行动
    private int _currentEntityIndex = 0;

    private int Trigger;//0代表Move，1代表Attack 
    
    //当前回合数
    public int RoundNum { get; set; }

    //判断游戏胜负
    private bool isVictory;
    private bool isfalse;

    //获取鼠标点击物体
    private GameObject objCliked=null;
    
    protected override void Awake()
    {
        base.Awake();
        Sort();
    }

    private void OnEnable()
    {
        EventCentreManager.Instance.Subscribe(MyEventType.ReadyFinished,EntityAction);
        EventCentreManager.Instance.Subscribe(MyEventType.EntityAfter,EntityAction);
        EventCentreManager.Instance.Subscribe(MyEventType.SomeOneDeath,Sort);
    }

    private void OnDisable()
    {
        EventCentreManager.Instance.Unsubscribe(MyEventType.ReadyFinished,EntityAction);
        EventCentreManager.Instance.Unsubscribe(MyEventType.EntityAfter,EntityAction);
        EventCentreManager.Instance.Unsubscribe(MyEventType.SomeOneDeath,Sort);
    }
    
    public void Sort()
    {
        Entities.Clear();
        GameObject[] friend = GameObject.FindGameObjectsWithTag("friend");
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var obj in friend)
        {
            Entities.Add(obj.GetComponent<Entity>());
        }
        foreach (var obj in enemy)
        {
            Entities.Add(obj.GetComponent<Entity>());
        }
        Entities.Sort((a, b) =>
            {
                return a.EntityDataSo.Speed
                    .CompareTo(b.EntityDataSo.Speed);
            }
        );
        Entities.Reverse();
    }
    
    //排兵布阵结束调用，每次实体移动后调用
    public void EntityAction()
    {
        if (_currentEntityIndex >= Entities.Count)
            _currentEntityIndex = 0;
        //得到当前移动实体
        CurrentEntity = Entities[_currentEntityIndex++];
        //如果实体是怪物，则执行一次怪物移动
        if (CurrentEntity is EnemyEntity)
        {
            (CurrentEntity as EnemyEntity)?.EnemyBehaviour();
        }
        else
        {
            RoundNum++;
            EventCentreManager.Instance.Publish(MyEventType.MyTurn);
        }
        if (_currentEntityIndex >= Entities.Count)
            _currentEntityIndex = 0;
    }
    //显示移动范围，判断鼠标点击位置是否在移动范围内，若是则CurrentEntity.Move(pos)，移动到当前位置;
    public void EntityMove()
    {
        CurrentEntity.GetAttackRangeComponent.CloseRange();
        //显示范围组件调用
        CurrentEntity.GetRangeComponent.ShowRange();
        //获取屏幕点击点
        StartCoroutine("GetMove");
    }
    
    public void EntityAttack()
    {
        CurrentEntity.GetRangeComponent.CloseRange();
        //显示范围组件调用
        CurrentEntity.GetAttackRangeComponent.ShowRange();
        StartCoroutine("EntityAttackIEnum");
    }


    public void EntityRecover()
    {
        (CurrentEntity as FriendEntity)?.Recover();
        EntityAction();
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Alpha1))EventCentreManager.Instance.Publish(MyEventType.OpenMenu);
    }

    private IEnumerator GetMove()
    {
        objCliked = null;
        while (true)
        {
            GetMouseCliked();
            if (objCliked != null && objCliked.transform.parent.CompareTag("Floor"))
            {
                break;
            }
            
            yield return null;
        }
        
        //判断点是否在范围内
        if (CurrentEntity.GetRangeComponent.JudgePointInRange(objCliked))
        {
            EventCentreManager.Instance.Publish(MyEventType.CloseMenu);
            CurrentEntity.GetRangeComponent.CloseRange();
            CurrentEntity.Move(objCliked.transform.position/*new Vector3( objCliked.transform.position.x,CurrentEntity.transform.position.y,objCliked.transform.position.z)*/);
        }

       /* temp = new Vector3(objCliked.transform.position.x, CurrentEntity.transform.position.y,
            objCliked.transform.position.z);
        while (Vector3.Distance(CurrentEntity.transform.position, temp) > 0.1f)
        {
            Debug.Log(1);
            yield return null;
        }*/
        CurrentEntity.GetRangeComponent.CloseRange();
    }

    public void GetMouseCliked()
    {
        if (Input.GetMouseButtonDown(0)) // 0 表示左键
        {
            // 获取鼠标点击的屏幕坐标
            Vector3 mousePosition = Input.mousePosition;

            // 创建一条射线从摄像机射向鼠标点击位置
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hitInfo; // 用于存储碰撞信息

            if (Physics.Raycast(ray, out hitInfo))
            {
                // 获取点击的物体
                objCliked = hitInfo.collider.gameObject;
            }
        }
    }


    IEnumerator EntityAttackIEnum()
    {
        //得到点击的GameObj,tag为"Enemy"
        GameObject obj;
        while (true)
        {
            //获取鼠标点击对象信息
            obj = GetClickedObj();
            if (obj != null && GetClickedObj().CompareTag("Enemy"))
            {
                var gridTrans = CurrentEntity.GetRayComponent.GetGridUpDown(obj.transform.position).transform.parent;
                string name =  gridTrans.parent.name+gridTrans.name;
                //如果在范围内
                if (IsSelectPlayerOnRange(name))
                {
                    EventCentreManager.Instance.Publish(MyEventType.CloseMenu);
                    //将敌人的实体组件传入，敌人扣血
                    CurrentEntity.Attack(obj.GetComponent<Entity>());
                    CurrentEntity.GetAttackRangeComponent.CloseRange();
                    CheakRoundEnd();
                    break;
                }
            }
            yield return null;
        }
        //判断Enemy是否在范围内
    }

    private bool IsSelectPlayerOnRange(string name)
    {
        //获得攻击范围内的格子
        List<GameObject> grids = CurrentEntity.GetAttackRangeComponent.GetRangeFloor();
        foreach (var grid in grids)
        {
            if (grid.transform.parent.name+grid.name==name)
            {
                return true;
            }
        }
        return false;
    }
    
    public GameObject GetClickedObj()
    {
        if (Input.GetMouseButton(0))
        {
            // 获取鼠标点击的屏幕坐标
            Vector3 mousePosition = Input.mousePosition;

            // 创建一条射线从摄像机射向鼠标点击位置
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hitInfo; // 用于存储碰撞信息

            if (Physics.Raycast(ray, out hitInfo))
            {
                // 获取点击的物体
                return objCliked = hitInfo.collider.gameObject;
            }
        }
        return null;
    }
//判断游戏结束,角色攻击后判定，怪物攻击后判定
    public void CheakRoundEnd()
    {
        GameObject[] friendUnits = GameObject.FindGameObjectsWithTag("friend");//tag:friend
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("Enemy");//tag:enemy

        int playerUnitCount = friendUnits.Length;
        int enemyUnitCount = enemyUnits.Length;

        if (playerUnitCount == 0)
        {
            // 游戏失败逻辑
            isfalse = true;
        }
        else if (enemyUnitCount == 0)
        {
            // 游戏胜利逻辑
            isVictory = true;
        }
    }

    void OnGUI()
    {
        //Time.timeScale = 0;
        // 在这里放置GUI代码，包括按钮的创建和事件处理
        if (isVictory)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "Next Level"))
            {
                // 获取当前场景的构建索引
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

                // 加载下一个场景（当前场景的索引 + 1）
                SceneManager.LoadScene(currentSceneIndex + 1);

                Time.timeScale = 1;
            }
        }
        else if (isfalse)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "Try Again"))
            {
                // 获取当前场景的名称
                string currentSceneName = SceneManager.GetActiveScene().name;

                // 重新加载当前场景
                SceneManager.LoadScene(currentSceneName);

                Time.timeScale = 1;
            }
        }
    }
}
