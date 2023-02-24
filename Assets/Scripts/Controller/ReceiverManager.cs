using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ReceiverManager : MonoBehaviour
{
    private Vector3 mousePosition;
    public GameObject RecEventMesPanel;

    public GameObject Panel;//�������������
    public GameObject BG;
    public GameObject FatherObject;

   // public GameObject ReceiverDataPanel;
    public InputField Size;
    public InputField Alpha;
    public Text PosX;
    public Text PosY;

    public GameObject ReceiverManagerPanel;//�������༭ui
    public GameObject NoteManagerPanel;//Note�༭ui
    public GameObject RecEventManagerPanel;

    public GameObject ReceiverDataPanel;

    public GameObject ReceiverObject;//������Ԥ����
    private SpriteRenderer Receiversprite;
    public static GameObject TargetReceiver;//��ǰ�༭������

    public static Dictionary<GameObject, Receiver> ReceiverDic = new Dictionary<GameObject, Receiver>();//�������н�������Ϣ
    bool isDelete = false;
    GameObject[] Vlines;//�洢������Ϣ
    GameObject[] Hlines;//�洢������Ϣ
    public static bool Ready = false;

    private void Awake()
    {
        //Debug.Log("Awake");
        Vlines = GameObject.FindGameObjectsWithTag("VerticalLine");
        Hlines = GameObject.FindGameObjectsWithTag("HorizontalLine");
        if (Data.root.NoteData.Count == 0)//������Ӧ������һ��������
        {
            Receiver r = new Receiver(1.0f, 255, 640, 360);//Ĭ�Ϲ��������
            NoteDataManager.TargetReceiver = r;//ΪnOTE�༭����ֵ
            EventManager.TargetReceiver = r;
            TargetReceiver = Instantiate(ReceiverObject, new Vector3(r.Position_x / 160 - 1, r.Position_y / -160, 8),
            ReceiverObject.transform.rotation, FatherObject.transform);
            ChooseReceiverColor();
            Data.root.NoteData.Add(r);
            ReceiverDic.Add(TargetReceiver, r);
            FatherObject.SetActive(false);
        }
        else
        StartCoroutine(LoadResourceData());
        FatherObject.SetActive(false);
    }
    void Update()
    {

           if (Input.GetMouseButtonDown(0)&& ReceiverManagerPanel.activeSelf)//�������༭UI�������ʱ��Ч
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
            else if(Physics.Raycast(ray, out hit) && hit.collider.gameObject == BG)
            {
                RecInput();
            }
          }
        if (Input.GetMouseButtonDown(1)&&NoteManagerPanel.activeSelf&& 
            Input.mousePosition.x>=855&&Input.mousePosition.y<=535&& Input.mousePosition.y >=63)
            //�Ҽ�ѡ��������,note�༭������úͽ�������������ʱ��Ч
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;
            //Debug.Log("�Ҽ����");//�������༭����÷�����Ч
            if (Physics.Raycast(ray, out hit) &&hit.collider.gameObject.name=="Receiver(Clone)")
            {
                if (TargetReceiver == null)//ɾ��ѡ�н���������������ڴ�ѡ�������
                {
                    TargetReceiver = hit.collider.gameObject;
                    //Debug.Log("ɾ�����޸Ľ�����");
                    ChangedReceiver();
                }
                else if (hit.collider.gameObject.transform.position != TargetReceiver.transform.position)
                {
                    ChangeChoose();
                    TargetReceiver = hit.collider.gameObject;
                    //Debug.Log("�޸Ľ�����");
                    ChangedReceiver();
                }
            }
        }
        else if(Input.GetMouseButtonDown(1) && RecEventManagerPanel.activeSelf &&
            Input.mousePosition.x >= 855 && Input.mousePosition.y <= 535 && Input.mousePosition.y >= 63)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;
            //Debug.Log("�Ҽ����");//�������༭����÷�����Ч
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == "Receiver(Clone)")
            {
                if (TargetReceiver == null)//ɾ��ѡ�н���������������ڴ�ѡ�������
                {
                    TargetReceiver = hit.collider.gameObject;
                    //Debug.Log("ɾ�����޸Ľ�����");
                    ChangedReceiver();
                }
                else if (hit.collider.gameObject.transform.position != TargetReceiver.transform.position)
                {
                    ChangeChoose();
                    TargetReceiver = hit.collider.gameObject;
                    //Debug.Log("�޸Ľ�����");
                    ChangedReceiver();
                }
            }
        }
    }
    IEnumerator LoadResourceData()
    {
        try
        {
            for (int i = 0; i < Data.root.NoteData.Count; i++)
            {
                GameObject Receiver = Instantiate(ReceiverObject, new Vector3(Data.root.NoteData[i].Position_x / 160f-1,
                    Data.root.NoteData[i].Position_y / -160f, 8), ReceiverObject.transform.rotation, FatherObject.transform);
                ReceiverDic.Add(Receiver, Data.root.NoteData[i]);
                if (i == 0)
                    TargetReceiver = Receiver;
            }
            ChangedReceiver();
            Ready = true;
            //Debug.Log("ReadyReceiver");
        }
        catch { }
        yield return new WaitForEndOfFrame();
    }
    private void ChangedReceiver()//�޸Ľ�����
    {
        NoteDataManager.TargetReceiver = ReceiverDic[TargetReceiver];//�޸�value
        EventManager.TargetReceiver = ReceiverDic[TargetReceiver];//�޸��¼�������
        NoteDataManager.RefreshReceiverNoteData();
        EventManager.RefreshReceiverEventData();
        Debug.Log("�������޸�");
        ChooseReceiverColor();
        RefreshRecDataPanel();
        RecEventMesPanel.SetActive(false);
    }
    private void ChooseReceiverColor()//�޸�ѡ�н�������ɫ
    {
        Receiversprite = TargetReceiver.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        Receiversprite.color = new Color32(1, 239, 205, 255);
        Debug.Log("�ױ�����ɫ�ɹ�");
    }
    private void ChangeChoose()//�޸��ϸ�ѡ�еĽ���������ɫ
    {
        Receiversprite = TargetReceiver.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        Receiversprite.color = new Color32(255, 255, 255, 255);
        Debug.Log("�̱���޸ĳɹ�");
    }
    private void RecInput()//��������������
    {
        ChangeChoose();
        TargetReceiver = Instantiate(ReceiverObject, mousePosition, ReceiverObject.transform.rotation, FatherObject.transform);
        Debug.Log(TargetReceiver.transform.position);
        SetPosition(TargetReceiver);
        ChooseReceiverColor();
        Receiver r = new Receiver(1, 255, TargetReceiver.transform.localPosition.x * 160, TargetReceiver.transform.localPosition.y * -160);
       // Debug.Log(r.Position_x);
        //Debug.Log(r.Position_y);
        ReceiverDic.Add(TargetReceiver, r);//�����ֵ��
        Data.root.NoteData.Add(r);//����������Ϣ
        NoteDataManager.RefreshReceiverNoteData();
        EventManager.RefreshReceiverEventData();
        RefreshRecDataPanel();
    }
    private void SetPosition(GameObject TargetReceiver)//�����������̶�λ��
    {
        GameObject TargetVline = Vlines[0];//x��V����
        GameObject TargetHline = Hlines[0];//y��H����
        float Ydiff = (Hlines[0].transform.position.y - TargetReceiver.transform.position.y);//����y�����ֵ
        float Xdiff = (Vlines[0].transform.position.y - TargetReceiver.transform.position.x);//����x�����ֵ
        Ydiff = Mathf.Abs(Ydiff);//ȡ����ֵ
        Xdiff = Mathf.Abs(Xdiff);//ȡ����ֵ
        float XTarget = Xdiff;
        float YTarget = Ydiff;
        foreach (GameObject l in Vlines)//�����������ߣ��ҵ������
        {
            Xdiff = (l.transform.position.x - TargetReceiver.transform.position.x); //����note��line�ľ����
            Xdiff = Mathf.Abs(Xdiff);//ȡ����ֵ
            if (Xdiff < XTarget)
            { //�ҳ��������
                XTarget = Xdiff;
                TargetVline = l;//ȡ���������
            }
        }
        foreach (GameObject l in Hlines)//�������к��ߣ��ҵ������
        {
            Ydiff = (l.transform.position.y - TargetReceiver.transform.position.y); //����note��line�ľ����
            Ydiff = Mathf.Abs(Ydiff);//ȡ����ֵ
            if (Ydiff < YTarget)
            { //�ҳ��������
                YTarget = Ydiff;
                TargetHline = l;//ȡ���������
            }
        }
        TargetReceiver.transform.position = new Vector3(TargetVline.transform.position.x, 
                                                   TargetHline.transform.position.y, 8);
    }
   private void RecDelete()//ɾ������������
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)&& hit.collider.gameObject.name=="Receiver(Clone)")
        {
            ChangeChoose();
            Debug.Log("ɾ��");
            NoteDataManager.ClearReceiverNoteData();//����ý�����������2d�����notebase����
            EventManager.ClearReceiverEventData();
           Data.root.NoteData.Remove(ReceiverDic[hit.collider.gameObject]);//�Ƴ�����
            ReceiverDic.Remove(hit.collider.gameObject);//�Ƴ��ֵ�
            DestroyImmediate(hit.collider.gameObject);//ɾ��ʵ������u2d����
            GameObject[] Rec= GameObject.FindGameObjectsWithTag("Receiver");//��ֹ�����Ϳ�
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
    public void RefreshRecDataPanel()
    {
        Alpha.text = NoteDataManager.TargetReceiver.alpha.ToString();
        Size.text = NoteDataManager.TargetReceiver.size.ToString();
        PosX.text = NoteDataManager.TargetReceiver.Position_x.ToString();
        PosY.text = NoteDataManager.TargetReceiver.Position_y.ToString();
        Debug.Log("���ˢ��");
    }
    public void SetCloseRecPanel()
    {
        ReceiverDataPanel.SetActive(false);
    }
}
