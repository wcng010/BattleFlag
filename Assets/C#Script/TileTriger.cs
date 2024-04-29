using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTriger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
       // Debug.Log("�������𣿣�");
        TestObj();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position+Vector3.up, Vector3.up);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Enter");
    //    if (other.name != name)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}
    public void TestObj()
    {
        
        Ray ray = new Ray(transform.position/*+ Vector3.up*/, Vector3.up);
        Debug.DrawRay(transform.position, Vector3.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);
        foreach (RaycastHit hit in hits)
        {
            // ��ȡ��ײ������ײ��
            Collider collider = hit.collider;

            // ����ײ����ӵ��б���
            if (hit.collider.gameObject != gameObject)
            {
                gameObject.SetActive(false);
                break;
            }

            // ��������Դ�����ײ�壬�����ӡ����
            Debug.Log("��ײ����" + collider.gameObject.name);
        }
    }
}
