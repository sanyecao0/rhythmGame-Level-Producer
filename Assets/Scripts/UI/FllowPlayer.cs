using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FllowPlayer : MonoBehaviour
{
    float scale = 0.2f;
    void Update()
    {
        //ͨ�����̻��ֿ������������
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (transform.position.y <= 0)
            {
                transform.position += new Vector3(0, 1 * scale);
                //����Ҫһ���޸Ĳ���ʱ��ķ���
               // Debug.Log("��" + transform.position);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            transform.position -= new Vector3(0, 1 * scale);
            //����Ҫһ���޸Ĳ���ʱ��ķ���
            //Debug.Log("��"+transform.position);
        }

    }
}
