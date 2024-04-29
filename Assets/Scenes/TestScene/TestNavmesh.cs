using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TestNavmesh : MonoBehaviour
{
    public GameObject target; 
        Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        
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
                //pos = hitInfo.collider.transform.position;
                GetComponent<NavMeshAgent>().destination = target.transform.position;
                Debug.Log(GetComponent<NavMeshAgent>().hasPath);
               //Debug.Log(GetComponent<NavMeshAgent>().SetDestination(target.transform.position));
               // GetComponent<NavMeshAgent>().Move(Vector3.back * 10);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        GetMouseCliked();
    }
}
