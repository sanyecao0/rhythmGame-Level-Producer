using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverManager : MonoBehaviour
{
    private Vector3 mousePosition;
    public GameObject Panel;//�������������
    public GameObject FatherObject;

    public GameObject ReceiverManagerPanel;//�������༭ui
    public GameObject NoteManagerPanel;//Note�༭ui

    public GameObject ReceiverObject;//������Ԥ����
    private SpriteRenderer Receiversprite;
    public static GameObject TargetReceiver;//��ǰ�༭������

    public static Dictionary<GameObject, Receiver> ReciverDic = new Dictionary<GameObject, Receiver>();//�������н�������Ϣ
    bool isDelete = false;
    GameObject[] Vlines;//�洢������Ϣ
    GameObject[] Hlines;//�洢������Ϣ

    private void Awake()
    {
        Vlines = GameObject.FindGameObjectsWithTag("VerticalLine");
        Hlines = GameObject.FindGameObjectsWithTag("HorizontalLine");
        if (DataManager.NoteData.Count == 0)//������Ӧ������һ��������
        {
            Receiver r = new Receiver(1.0, 255, 640, 360);//Ĭ�Ϲ��������
            NoteDataManager.TargetReceiver = r;//ΪnOTE�༭����ֵ
            TargetReceiver = Instantiate(ReceiverObject,new Vector3(r.Position_x/160-1,r.Position_y/-160,8) , 
            ReceiverObject.transform.rotation, FatherObject.transform);
           // NoteDataManager.TargetReceiverObj = TargetReceiver;//ͬ����Ϣ
            ChooseReceiverColor();
            DataManager.NoteData.Add(r);
            ReciverDic.Add(TargetReceiver, r);
        }
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
                    Debug.Log("ɾ�����޸Ľ�����");
                    ChangedReceiver();
                }
                else if (hit.collider.gameObject.transform.position != TargetReceiver.transform.position)
                {
                    ChangeChoose();
                    TargetReceiver = hit.collider.gameObject;
                    Debug.Log("�޸Ľ�����");
                    ChangedReceiver();
                }
            }
        }
    }
    private void ChangedReceiver()//�޸Ľ�����
    {
        NoteDataManager.TargetReceiver = ReciverDic[TargetReceiver];//�޸�value
        NoteDataManager.RefreshReceiverNoteData();
        ChooseReceiverColor();
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
        SetPosition(TargetReceiver);
        ChooseReceiverColor();
        Receiver r = new Receiver(1, 255, TargetReceiver.transform.localPosition.x * 320, TargetReceiver.transform.localPosition.y * -320);
        ReciverDic.Add(TargetReceiver, r);//�����ֵ��
        DataManager.NoteData.Add(r);//����������Ϣ
        NoteDataManager.RefreshReceiverNoteData();
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
            DataManager.NoteData.Remove(ReciverDic[hit.collider.gameObject]);//�Ƴ�����
            ReciverDic.Remove(hit.collider.gameObject);//�Ƴ��ֵ�
            DestroyImmediate(hit.collider.gameObject);//ɾ��ʵ������u2d����
            GameObject[] Rec= GameObject.FindGameObjectsWithTag("Receiver");//��ֹ�����Ϳ�
            TargetReceiver = Rec[0];
            ChangedReceiver();
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
