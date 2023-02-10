using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteDataManager : MonoBehaviour
{
	public GameObject BlackClick;
	public GameObject RedClick;
	public GameObject WhiteClick;
	public GameObject Flick;
	public GameObject Hold;
	public GameObject Drag;
	public GameObject ReceiverObject;

	public GameObject Note;
	public GameObject FatherObject;
	public static int index;//��������
	public static float Angle = 0;//���ɽǶ�,Ĭ��Ϊ0�㼴���Ϸ�
	public InputField inputAngle;//�󶨽Ƕ������
	public bool NoteDelete = false;
	private Vector3 mousePosition, endPosition;
	GameObject[] lines;//�洢��������Ϣ

	public static Dictionary<GameObject, NoteBase> NoteDic = new Dictionary<GameObject, NoteBase>();//�洢note��Ϣ
	public static Receiver TargetReceiver;//��ǰ�༭�Ľ�����

	private void Start()
	{
		Note = BlackClick;
		lines = GameObject.FindGameObjectsWithTag("Line");
		TargetReceiver = ReceiverManager.ReciverDic[ReceiverManager.TargetReceiver];
	}
	void Update()
	{
		if (FatherObject.activeSelf)//������
		{
			if (Input.GetMouseButtonUp(0) && Input.mousePosition.x < 423)//���̧������������ұ����������
			{
					mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//ת����������
					if (NoteDelete)//�ж���������ģʽ
						NoteInput_Delete();
					else
						NoteInput();
			}
			else if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > 423)
			{
				Debug.Log("����Ұ���Ļ");
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit) && hit.collider.gameObject==ReceiverObject)//����
				{
					TargetReceiver = ReceiverManager.ReciverDic[hit.collider.gameObject];
					Debug.Log(TargetReceiver.Position_x);
				}
			}
		}
	}
	public static void ClearReceiverNoteData()//��ձ�ɾ���Ľ���������������
    {
		List<NoteBase> note = new List<NoteBase>(TargetReceiver.Note);
        try
        {
			foreach (KeyValuePair<GameObject, NoteBase> kvp in NoteDic)//������֪����Ҫ��Foreach�޸ļ��ϣ������������Ҫ�޸�
			{
				for (int j = 0; j < TargetReceiver.Note.Count; j++)
				{
					if (kvp.Value == note[j])
					{
						NoteDic.Remove(kvp.Key);
						DestroyImmediate(kvp.Key);
						Debug.Log("���");
						break;
					}
				}
			}
		}
        catch(InvalidOperationException e)
		{
		}
		TargetReceiver.Note.Clear();
	}
	public static void RefreshReceiverNoteData()//����ѡ��������ˢ�±༭����
    {
		    TargetReceiver = ReceiverManager.ReciverDic[ReceiverManager.TargetReceiver];
			GameObject[] Notes= GameObject.FindGameObjectsWithTag("Note");
		    foreach (GameObject n in Notes)
            {
			   n.SetActive(false);
			   //Debug.Log("�ر�һ��");
			}
		   // Debug.Log("�ر����");
			foreach (KeyValuePair<GameObject, NoteBase> kvp in NoteDic)//������ǰ����������Ϣ
			{
				foreach (NoteBase note in TargetReceiver.Note)
				{
					if (kvp.Value==note)
					{
						Debug.Log(kvp.Key.transform.position.x+"Foreach IN");
						kvp.Key.SetActive(true);
						break;
					}
				}		
			}
    }
	void NoteInput()
    {
        if (index != 4)//��hold����ͳһ���ô˷�ʽ
        {
			GameObject note = Instantiate(Note, mousePosition, Note.transform.rotation, FatherObject.transform);
			note.name = "Note";
			//�༭���ʵ����һ��2d��������
			NoteBase notemes = new NoteBase(SetPosition(note) * GameTime.secPerBeat, index, Angle, 1, false);
			//������Ϣʵ����
			NoteDic.Add(note, notemes);//�����ֵ��������Ϣ��unity2d����
			TargetReceiver.Note.Add(notemes);//���뵱ǰָ����������note�б�
			Debug.Log(TargetReceiver.Position_x);
		}
        else
        {
			//ʵ����Hold��������
			Debug.Log("ʵ����Hold����");
			GameObject note = Instantiate(Note, Note.transform.position, Note.transform.rotation, FatherObject.transform);
			NoteBase notemes = new NoteBase(SetPosition(note) * GameTime.secPerBeat,
				GetEndPosition() * GameTime.secPerBeat, index, 0, 1, false);
			note.name = "Note";
			//Note.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();//��ȡ������Ӷ����sprite�ķ���
			Debug.Log(notemes.start_time);
			Debug.Log(notemes.Finish_time);
			NoteDic.Add(note, notemes);//�����ֵ��������Ϣ��unity2d����
			TargetReceiver.Note.Add(notemes);//���뵱ǰָ����������note�б�
        }
	}
	void NoteInput_Delete()
    {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)&& hit.collider.gameObject.name=="Note"&& hit.collider.gameObject.activeSelf)
		    { 
			TargetReceiver.Note.Remove(NoteDic[hit.collider.gameObject]);//�Ƴ���ǰ������note����
			NoteDic.Remove(hit.collider.gameObject);//�Ƴ��ֵ�
			DestroyImmediate(hit.collider.gameObject);//ɾ��ʵ������u2d����
		    }
	}

	private void NoteChoose()
    {
        switch (index) {
			case 0:
                {
					Note = RedClick;
					break;
                }
			case 1:
				{
					Note = WhiteClick;
					break;
                }
			case 2:
                {
					Note = BlackClick;
					break;
                }
			case 3:
                {
					Note = Drag;
					break;
                }
			case 4:
                {
					Note = Hold;
					break;
                }
			case 6:
                {
					Note = Flick;
					break;
                }
		}
    }
	private int SetPosition(GameObject NOTE)
	{
			GameObject TargetObject = lines[0];//Ĭ�ϳ�ʼ��
			float diff = (lines[0].transform.position.y - NOTE.transform.position.y);//����y�����ֵ
			diff = Mathf.Abs(diff);//ȡ����ֵ
			float Target = diff;
			foreach (GameObject l in lines)//��������ʱ����
			{
				diff = (l.transform.position.y - NOTE.transform.position.y); //����note��line�ľ����
				diff = Mathf.Abs(diff);//ȡ����ֵ
				if (diff < Target)
				{ //�ҳ��������
					Target = diff;
					TargetObject = l;//ȡ���������
				}
			}
			NOTE.transform.position = TargetObject.transform.position;//��������������
		    NOTE.transform.position = new Vector3(-6.5f, NOTE.transform.position.y, NOTE.transform.position.z);
			return int.Parse(TargetObject.name);//�����ߵı�ż����Ӧʱ��
    }

	int GetEndPosition()
	{
		GameObject[] lines;//�洢��������Ϣ
		lines = GameObject.FindGameObjectsWithTag("Line");
		GameObject TargetObject = lines[0];//Ĭ�ϳ�ʼ��
		float diff = (lines[0].transform.position.y - endPosition.y);//����y�����ֵ
		diff = Mathf.Abs(diff);//ȡ����ֵ
		float Target = diff;
		foreach (GameObject l in lines)//��������ʱ����
		{
			diff = (l.transform.position.y - endPosition.y); //����endPosition��line�ľ����
			diff = Mathf.Abs(diff);//ȡ����ֵ
			if (diff < Target)
			{ //�ҳ��������
				Target = diff;
				TargetObject = l;//ȡ���������
			}
		}
		return int.Parse(TargetObject.name);//�����ߵı�ż����Ӧʱ��
	}
	public void RedClickChoose()
    {
		index = 0;
		NoteChoose();
		NoteDelete = false;
	}
	public void BlackClickChoose()
	{
		index = 2;
		NoteChoose();
		NoteDelete = false;
	}
	public void whiteClickChoose()
	{
		index = 1;
		NoteChoose();
		NoteDelete = false;
	}
	public void  DragChoose()
	{
		index = 3;
		NoteChoose();
		NoteDelete = false;
	}
	public void HoldChoose()
	{
		index = 4;
		NoteChoose();
		NoteDelete = false;
	}
	public void FlickChoose()
	{
		index = 6;
		NoteChoose();
		NoteDelete = false;
	}
	public void SetNoteDelete()
    {
		NoteDelete = true;
    }

	public void SetAngle()
    {
		if(float.Parse(inputAngle.text)>=0&& float.Parse(inputAngle.text) <= 360)
        {
			Angle = float.Parse(inputAngle.text);
		}
        else
        {
			inputAngle.text = Angle.ToString();
        }
		//Debug.Log(Angle);
    }
}
