using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAngleCon : MonoBehaviour
{

    public float topDistance = 30;
    //�����Χ��ĳ����ת����ͼ���ĵ㣩
    public float rotationPointX;
    public float rotationPointY;
    public float rotationPointZ;

    float rotationX;

    //��¼���ԭ������
    float temX;
    float temY;
    float temZ;

    //�Ƿ���top�ӽ�
    bool openCondition;

    //���������
    
    // Start is called before the first frame update
    void Start()
    {
        temX = this.transform.position.x;
        temY = this.transform.position.y;
        temZ = this.transform.position.z;

        rotationX = transform.localEulerAngles.x;
        //Debug.Log(rotationX);

        openCondition = false;
        
    }

    // Update is called once per frame
    //QE������ת��M top�ӽ�
    void Update()
    {
        float fieldView = Camera.main.fieldOfView;
        fieldView -= Input.GetAxis("Mouse ScrollWheel") * 10f;
        fieldView = Mathf.Clamp(fieldView, 15, 60);
        Camera.main.fieldOfView = fieldView;

        //�������ת
        if (Input.GetKey(KeyCode.Q)) 
        {
            transform.RotateAround(new Vector3(rotationPointX, rotationPointY, rotationPointZ), new Vector3(0f, 1f, 0f), 0.5f);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(new Vector3(rotationPointX, rotationPointY, rotationPointZ), new Vector3(0f, 1f, 0f), -0.5f);
        }
        if (Input.GetKeyDown(KeyCode.M) && openCondition == false)
        {
            this.transform.position = new Vector3(rotationPointX, rotationPointY+topDistance, rotationPointZ);
            //transform.Rotate(new Vector3(90, 180, 0));
            this.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
            openCondition = true;
        }
        else if (Input.GetKeyDown(KeyCode.M) && openCondition == true) 
        {
            this.transform.position = new Vector3(temX, temY, temZ);
            //transform.Rotate(new Vector3(30, 180, 0));
            this.transform.rotation = Quaternion.Euler(rotationX, 180f, 0f);
            openCondition = false;
        }
    }
}
