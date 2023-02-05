using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteDataManager : MonoBehaviour
{
	public GameObject BlackClick;
	public GameObject RedClick;
	public GameObject WhiteClick;
	public GameObject Flick;
	public GameObject Hold;
	public GameObject Drag;


	public GameObject Note;
	public GameObject FatherObject;
	public GameObject ReceiverPanel;
	public static int index;//��������
	public static float Angle = 0;//���ɽǶ�,Ĭ��Ϊ0�㼴���Ϸ�
	public InputField inputAngle;//�󶨽Ƕ������
	public bool NoteDelete = false;
	private Vector3 mousePosition, targetPosition, endPosition;

	public Dictionary<GameObject, Receiver> ReciverDic = new Dictionary<GameObject, Receiver>();
	public Dictionary<GameObject, NoteBase> NoteDic = new Dictionary<GameObject, NoteBase>();//�洢note��Ϣ
	private Receiver TargetReciver;//��ǰ�༭�Ľ�����

	private void Awake()
	{
		Note = BlackClick;
		if (DataManager.NoteData.Count == 0)//������Ӧ������һ��������
		{
			Receiver r = new Receiver(1.0, 255, 640, 360);//Ĭ�Ϲ��������
			DataManager.NoteData.Add(r);
			TargetReciver = r;
		}
	}
	void Update()
	{
		if (FatherObject.activeSelf)//������
		{
			if (Input.GetMouseButtonUp(0) && Input.mousePosition.x < 423)//���̧������������ұ����������MOUS
			{
				if (!EventSystem.current.IsPointerOverGameObject()) //���û�㵽UI��
				{
					mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//ת����������
					if (NoteDelete)//�ж���������ģʽ
						NoteInput_Delete();
					else
						NoteInput();
				}
			}
			else if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > 423)
			{
				Debug.Log("����Ұ���Ļ");
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == ReceiverPanel)
				{
					Debug.Log("����˽��������");
				}
			}
		}
	}
	void NoteInput()
    {
        if (index != 4)//��hold����ͳһ���ô˷�ʽ
        {
			GameObject note = Instantiate(Note, mousePosition, Note.transform.rotation, FatherObject.transform);
			//�༭���ʵ����һ��2d��������
			NoteBase notemes = new NoteBase(SetPosition(note) * GameTime.secPerBeat, index, Angle, 1, false);
			//������Ϣʵ����
			Debug.Log(notemes.start_time);
			NoteDic.Add(note, notemes);//�����ֵ��������Ϣ��unity2d����
			TargetReciver.Note.Add(notemes);//���뵱ǰָ����������note�б�
		}
        else
        {
			//ʵ����Hold��������
			Debug.Log("ʵ����Hold����");
			GameObject note = Instantiate(Note, Note.transform.position, Note.transform.rotation, FatherObject.transform);
			NoteBase notemes = new NoteBase(SetPosition(note) * GameTime.secPerBeat,
				GetEndPosition() * GameTime.secPerBeat, index, 0, 1, false);
			Debug.Log(notemes.start_time);
			Debug.Log(notemes.Finish_time);
			NoteDic.Add(note, notemes);//�����ֵ��������Ϣ��unity2d����
			TargetReciver.Note.Add(notemes);//���뵱ǰָ����������note�б�
        }
	}
	void NoteInput_Delete()
    {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
			//Debug.Log(hit.collider.gameObject);
			TargetReciver.Note.Remove(NoteDic[hit.collider.gameObject]);//�Ƴ���ǰ������note����
			NoteDic.Remove(hit.collider.gameObject);//�Ƴ��ֵ�
			DestroyImmediate(hit.collider.gameObject);//ɾ��ʵ������u2d����
		    }
	}
	Vector3 GetNotePos()
	{//����ת��
		Vector3 imagePos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle(Note.transform.parent as RectTransform, Input.mousePosition, null, out imagePos);
		return imagePos;
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
			GameObject[] lines;//�洢��������Ϣ
			lines = GameObject.FindGameObjectsWithTag("Line");
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
