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
        if (Input.GetMouseButtonDown(0)) // 0 ��ʾ���
        {
            // ��ȡ���������Ļ����
            Vector3 mousePosition = Input.mousePosition;

            // ����һ�����ߴ���������������λ��
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hitInfo; // ���ڴ洢��ײ��Ϣ

            if (Physics.Raycast(ray, out hitInfo))
            {


                // ��ȡ���������
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
