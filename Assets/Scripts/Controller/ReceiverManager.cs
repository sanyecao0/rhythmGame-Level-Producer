using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReceiverManager : MonoBehaviour
{
    private Vector3 mousePosition;
    public GameObject Panel;//接收器处理面板
    public GameObject FatherObject;

    public GameObject ReceiverManagerPanel;//接收器编辑ui
    public GameObject NoteManagerPanel;//Note编辑ui

    public GameObject ReceiverObject;//接收器预制体
    private SpriteRenderer Receiversprite;
    public static GameObject TargetReceiver;//当前编辑接收器

    public static Dictionary<GameObject, ReceiverMes> ReceiverDic = new Dictionary<GameObject, ReceiverMes>();//保存所有接收器信息
    bool isDelete = false;
    GameObject[] Vlines;//存储竖线信息
    GameObject[] Hlines;//存储横线信息

    private void Awake()
    {
        Vlines = GameObject.FindGameObjectsWithTag("VerticalLine");
        Hlines = GameObject.FindGameObjectsWithTag("HorizontalLine");
        if (Data.data.NoteData.Count == 0)//新谱面应至少有一个接收器
        {
            ReceiverMes r = new ReceiverMes(1.0, 255, 640, 360);//默认构造接收器
            NoteDataManager.TargetReceiver = r.Receiver;//为nOTE编辑区赋值
            EventManager.TargetReceiver = r;
            TargetReceiver = Instantiate(ReceiverObject,new Vector3(r.Receiver.Position_x/160-1,r.Receiver.Position_y/-160,8) , 
            ReceiverObject.transform.rotation, FatherObject.transform);
            Debug.Log(TargetReceiver);
            ChooseReceiverColor();
            Data.data.NoteData.Add(r);
            ReceiverDic.Add(TargetReceiver, r);
        }
    }
    void Update()
    {

           if (Input.GetMouseButtonDown(0)&& ReceiverManagerPanel.activeSelf)//接收器编辑UI面板启用时生效
          {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 8;
            RaycastHit hit;
            if (isDelete)
            {
                RecDelete();
            }
            else if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == Panel)
            {
                RecInput();
            }
          }
        if (Input.GetMouseButtonDown(1)&&NoteManagerPanel.activeSelf&& 
            Input.mousePosition.x>=855&&Input.mousePosition.y<=535&& Input.mousePosition.y >=63)
            //右键选定接收器,note编辑面板启用和接收器管理不启用时生效
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;
            //Debug.Log("右键点击");//接收器编辑界面该方法无效
            if (Physics.Raycast(ray, out hit) &&hit.collider.gameObject.name=="Receiver(Clone)")
            {
                if (TargetReceiver == null)//删除选中接收器的情况，可在此选择接收器
                {
                    TargetReceiver = hit.collider.gameObject;
                    Debug.Log("删除后修改接收器");
                    ChangedReceiver();
                }
                else if (hit.collider.gameObject.transform.position != TargetReceiver.transform.position)
                {
                    ChangeChoose();
                    TargetReceiver = hit.collider.gameObject;
                    Debug.Log("修改接收器");
                    ChangedReceiver();
                }
            }
        }
    }
    private void ChangedReceiver()//修改接收器
    {
        NoteDataManager.TargetReceiver = ReceiverDic[TargetReceiver].Receiver;//修改value
        NoteDataManager.RefreshReceiverNoteData();
        EventManager.TargetReceiver = ReceiverDic[TargetReceiver];
        ChooseReceiverColor();
    }
    private void ChooseReceiverColor()//修改选中接收器配色
    {
        Receiversprite = TargetReceiver.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        Receiversprite.color = new Color32(1, 239, 205, 255);
        Debug.Log("白变绿配色成功");
    }
    private void ChangeChoose()//修改上个选中的接收器的配色
    {
        Receiversprite = TargetReceiver.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        Receiversprite.color = new Color32(255, 255, 255, 255);
        Debug.Log("绿变白修改成功");
    }
    private void RecInput()//新增接收器方法
    {
        ChangeChoose();
        TargetReceiver = Instantiate(ReceiverObject, mousePosition, ReceiverObject.transform.rotation, FatherObject.transform);
        SetPosition(TargetReceiver);
        ChooseReceiverColor();
        ReceiverMes r = new ReceiverMes(1, 255, TargetReceiver.transform.localPosition.x * 320, TargetReceiver.transform.localPosition.y * -320);
        ReceiverDic.Add(TargetReceiver, r);//加入字典绑定
        Data.data.NoteData.Add(r);//加入谱面信息
        NoteDataManager.RefreshReceiverNoteData();
    }
    private void SetPosition(GameObject TargetReceiver)//接收器吸附固定位置
    {
        GameObject TargetVline = Vlines[0];//x和V竖线
        GameObject TargetHline = Hlines[0];//y和H横线
        float Ydiff = (Hlines[0].transform.position.y - TargetReceiver.transform.position.y);//计算y距离差值
        float Xdiff = (Vlines[0].transform.position.y - TargetReceiver.transform.position.x);//计算x距离差值
        Ydiff = Mathf.Abs(Ydiff);//取绝对值
        Xdiff = Mathf.Abs(Xdiff);//取绝对值
        float XTarget = Xdiff;
        float YTarget = Ydiff;
        foreach (GameObject l in Vlines)//遍历所有竖线，找到最近的
        {
            Xdiff = (l.transform.position.x - TargetReceiver.transform.position.x); //计算note与line的距离差
            Xdiff = Mathf.Abs(Xdiff);//取绝对值
            if (Xdiff < XTarget)
            { //找出最近距离
                XTarget = Xdiff;
                TargetVline = l;//取得最近线条
            }
        }
        foreach (GameObject l in Hlines)//遍历所有横线，找到最近的
        {
            Ydiff = (l.transform.position.y - TargetReceiver.transform.position.y); //计算note与line的距离差
            Ydiff = Mathf.Abs(Ydiff);//取绝对值
            if (Ydiff < YTarget)
            { //找出最近距离
                YTarget = Ydiff;
                TargetHline = l;//取得最近线条
            }
        }
        TargetReceiver.transform.position = new Vector3(TargetVline.transform.position.x, 
                                                   TargetHline.transform.position.y, 8);
    }
   private void RecDelete()//删除接收器方法
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)&& hit.collider.gameObject.name=="Receiver(Clone)")
        {
            ChangeChoose();
            Debug.Log("删除");
            NoteDataManager.ClearReceiverNoteData();//清除该接收器下所有2d对象和notebase数据
            Data.data.NoteData.Remove(ReceiverDic[hit.collider.gameObject]);//移出谱面
            ReceiverDic.Remove(hit.collider.gameObject);//移出字典
            DestroyImmediate(hit.collider.gameObject);//删除实例化的u2d对象
            GameObject[] Rec= GameObject.FindGameObjectsWithTag("Receiver");//防止对象滞空
            try
            {
                TargetReceiver = Rec[0];
                ChangedReceiver();
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }
    }
    public void SetDelete()
    {
        isDelete = true;
    }
    public void SetInput()
    {
        isDelete = false; ;
    }
}
