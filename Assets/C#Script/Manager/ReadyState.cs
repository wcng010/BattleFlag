using System;
using UnityEngine;

namespace C_Script.Manager
{

    public class ReadyState : MonoBehaviour
    {
        private GameObject selectedUnit; // 选中的己方单位
        private GameObject[] startPlanes; // 存储带有"StartPlane"标签的格子
        private float originalYPosition;
        private bool action;
        

        private void OnEnable()
        {
            EventCentreManager.Instance.Subscribe(MyEventType.ReadyFinished,End);
        }

        private void OnDisable()
        {
            EventCentreManager.Instance.Unsubscribe(MyEventType.ReadyFinished,End);
        }

        private void End() => action = true;

        private void Start()
        {
            // 获取所有带有"StartPlane"标签的格子
            startPlanes = GameObject.FindGameObjectsWithTag("StartPlane");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ReadyArmy();
            }

            //排兵布阵结束
            if (action)
            {
                // 遍历并禁用所有 "StartPlane" 物体
                foreach (GameObject startPlane in startPlanes)
                {
                    startPlane.SetActive(false);
                }

                this.enabled = false;
            }
        }

        public void ReadyArmy()
        {
            //调整军队逻辑
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                Debug.Log(1);
                if (selectedUnit == null)
                {
                    // 如果没有选中单位，则尝试选中一个己方单位
                    if (IsPlayerUnit(clickedObject))
                    {
                        selectedUnit = clickedObject;

                        // 记录原始的Y轴坐标
                        originalYPosition = selectedUnit.transform.position.y;

                        selectedUnit.transform.Translate(Vector3.up * 0.5f); // 提示玩家选中了单位
                    }
                }
                else
                {
                    // 如果已经选中了一个己方单位，则尝试放置或交换单位位置
                    if (IsStartPlane(clickedObject))
                    {
                        // 恢复单位的Y轴坐标
                        selectedUnit.transform.position = new Vector3(
                            selectedUnit.transform.position.x,
                            originalYPosition,
                            selectedUnit.transform.position.z
                        );

                        // 检查点击的格子上方是否有带有"friend"标签的物体
                        RaycastHit objectAboveHit;
                        if (Physics.Raycast(clickedObject.transform.position, Vector3.up, out objectAboveHit))
                        {
                            if (!objectAboveHit.collider.CompareTag("friend"))
                            {
                                PlaceUnit(selectedUnit, clickedObject.transform.position);
                            }
                            else
                            {
                                Debug.Log("点击的格子上方有带有\"friend\"标签的物体，不执行操作。");
                            }
                        }
                        else
                        {
                            PlaceUnit(selectedUnit, clickedObject.transform.position);
                        }
                    }
                    else if (IsPlayerUnit(clickedObject))
                    {
                        // 恢复单位的Y轴坐标
                        selectedUnit.transform.position = new Vector3(
                            selectedUnit.transform.position.x,
                            originalYPosition,
                            selectedUnit.transform.position.z
                        );

                        SwapUnitPositions(selectedUnit, clickedObject);
                    }

                    selectedUnit.transform.position = new Vector3(
                            selectedUnit.transform.position.x,
                            originalYPosition,
                            selectedUnit.transform.position.z
                        );
                    // 清除选中单位
                    selectedUnit = null;
                }
            }
        }

        private bool IsPlayerUnit(GameObject obj)
        {
            // 检查对象是否是己方单位
            return obj.CompareTag("friend");
        }

        private bool IsStartPlane(GameObject obj)
        {
            // 检查对象是否是有子物体"StartPlane"的格子
            return obj.CompareTag("StartPlane");
        }

        private void PlaceUnit(GameObject unit, Vector3 position)
        {
            // 计算目标位置，位于被点击的方格的中央的上方
            Vector3 targetPosition = new Vector3(position.x, position.y + 2f, position.z);

            // 将选中单位的位置设置为目标位置
            unit.transform.position = targetPosition;
        }

        private void SwapUnitPositions(GameObject unit1, GameObject unit2)
        {
            // 交换两个单位的位置
            Vector3 temp = unit1.transform.position;
            unit1.transform.position = unit2.transform.position;
            unit2.transform.position = temp;
        }
    }
}
