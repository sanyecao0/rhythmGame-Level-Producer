using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FllowPlayer : MonoBehaviour
{
    float scale = 0.2f;
    void Update()
    {
        //通过键盘滑轮控制物体的缩放
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (transform.position.y <= 0)
            {
                transform.position += new Vector3(0, 1 * scale);
                //还需要一个修改播放时间的方法
               // Debug.Log("加" + transform.position);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            transform.position -= new Vector3(0, 1 * scale);
            //还需要一个修改播放时间的方法
            //Debug.Log("减"+transform.position);
        }

    }
}
