using System;
using UnityEngine;
using System.Collections.Generic;
namespace C_Script.Component
{
    public class RangeComponent:EntityComponent
    {
        public List<GameObject> GetRangeFloor() => blocks;
        List<GameObject> blocks=new List<GameObject>();
        [Header("盒状")]
        public bool box;
        [Tooltip("前后长度是z值，左右长度是x值,只要box能移中心点")]
        public Vector3 RangeMove;
        [Tooltip("前后长度是z值，左右长度是x值")]
        public Vector3 halfExtents;
        [Tooltip("手动校准")]
        public float blockWidth = 5f;
        //显示范围
        public void ShowRange()
        {

            //以实体所在方块中心为圆心发射一个半径为range*blockWidth的球状射线，过滤掉不在同一高度的方块，调用方块上的方法激活所有子物体
            
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 100f))
            {
                //blocks.Add(hitInfo.collider.gameObject.transform.parent.gameObject);
                GameObject standBlock = hitInfo.collider.gameObject.transform.parent.gameObject;
                //Collider[] colliders = Physics.OverlapSphere(blocks[0].transform.position, range * blockWidth);
                Collider[] colliders = MakeRange(standBlock.transform.position);
                foreach (Collider collider in colliders)
                {
                    GameObject block = collider.gameObject.transform.parent.gameObject;
                    if (block.tag == "Floor" && block.transform.position.y == standBlock.transform.position.y)//给地面方块打个标签区分且只能在同一平面移动
                    {
                        blocks.Add(block);//方便关
                        if (block != standBlock)
                        {
                            block.transform.GetChild(0).gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
        Collider[] MakeRange(Vector3 center)
        {
            if (box)//盒状范围
            {
               return Physics.OverlapBox(center+RangeMove*blockWidth, halfExtents*blockWidth);
            }
            else//十字范围
            {
                List<Collider> hitColliders = new List<Collider>();
                Vector3[] raydirfb = { Vector3.forward, Vector3.back };
                //前后长度是z值，左右长度是x值
                foreach (Vector3 rayDirection in raydirfb)
                {
                    Ray ray = new Ray(center, rayDirection);
                    
                    RaycastHit[] hits = Physics.RaycastAll(ray, halfExtents.z*blockWidth);
                    foreach (RaycastHit hit in hits)
                    {
                        // 获取碰撞到的碰撞体
                        Collider collider = hit.collider;

                        // 将碰撞体添加到列表中
                        hitColliders.Add(collider);

                        // 在这里可以处理碰撞体，例如打印名称
                        //Debug.Log("碰撞对象：" + collider.gameObject.name);
                    }
                }
                
                Vector3[] raydirlr = { Vector3.left, Vector3.right };
                //前后长度是y值，左右长度是x值
                foreach (Vector3 rayDirection in raydirlr)
                {
                    Ray ray = new Ray(center, rayDirection);

                    RaycastHit[] hits = Physics.RaycastAll(ray, halfExtents.x*blockWidth);
                    
                    foreach (RaycastHit hit in hits)
                    {
                        // 获取碰撞到的碰撞体
                        Collider collider = hit.collider;

                        // 将碰撞体添加到列表中
                        hitColliders.Add(collider);

                        // 在这里可以处理碰撞体，例如打印名称
                        //Debug.Log("碰撞对象：" + collider.gameObject.name);
                    }
                }
                return hitColliders.ToArray();
            }
        }
        //关闭范围
        public void CloseRange()//移动按下后调用
        {
            foreach(GameObject block in blocks)
            {
                block.transform.GetChild(0).gameObject.SetActive(false);
            }
            blocks.Clear();//清空一下
        }
        //判断点是否在圈内
        public bool JudgePointInRange(GameObject obj)
        {
            return (obj.name=="Plane")?true:false;
        }
    }
}