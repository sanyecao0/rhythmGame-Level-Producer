using System.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

	public float Scale;
	public GameObject Note;
	public GameObject FatherObject;
	public static int index;//音符索引
	public static float Angle = 0;//生成角度,默认为0°即正上方
	public InputField inputAngle;//绑定角度输入框
	public bool NoteDelete = false;
	private Vector3 mousePosition, endPosition;
	GameObject[] lines;//存储所有线信息

	public static Dictionary<GameObject, NoteBase> NoteDic = new Dictionary<GameObject, NoteBase>();//存储note信息
	public static Receiver TargetReceiver;//当前编辑的接收器

	private void Start()
	{
		Note = BlackClick;
		lines = GameObject.FindGameObjectsWithTag("Line");
	}
	void Update()
	{
		if (FatherObject.activeSelf)//避免误触
		{
<<<<<<< HEAD
			if (Input.GetMouseButtonUp(0) && Input.mousePosition.x < 423&& UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())//���̧������������ұ����������
=======
			if (Input.GetMouseButtonUp(0) && Input.mousePosition.x < 423)//左键抬起放置音符，且必须在左半屏
>>>>>>> 038fab1b715826523ac4091a8e1e4de63ba90dc1
			{
					
					endPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 14f));
					if (NoteDelete)//判断音符操作模式
						NoteInput_Delete();
					else
						NoteInput();
			}
			else if (Input.GetMouseButtonDown(0) && Input.mousePosition.x < 423)//获取鼠标按下时的坐标
			{
				mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//转换世界坐标
			}
			else if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > 423)
			{
<<<<<<< HEAD
				//Debug.Log("����Ұ���Ļ");
=======
				Debug.Log("点击右半屏幕");
>>>>>>> 038fab1b715826523ac4091a8e1e4de63ba90dc1
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit) && hit.collider.gameObject==ReceiverObject)//隐患
				{
					TargetReceiver = ReceiverManager.ReceiverDic[hit.collider.gameObject].Receiver;
					Debug.Log(TargetReceiver.Position_x);
				}
			}
		}
	}
	public static void ClearReceiverNoteData()//清空被删除的接收器的内容音符
    {
		List<NoteBase> note = new List<NoteBase>(TargetReceiver.Note);
        try
        {
			foreach (KeyValuePair<GameObject, NoteBase> kvp in NoteDic)//众所周知，不要用Foreach修改集合，所以这可能需要修改
			{
				for (int j = 0; j < TargetReceiver.Note.Count; j++)
				{
					if (kvp.Value == note[j])
					{
						NoteDic.Remove(kvp.Key);
						Destroy(kvp.Key);
						Debug.Log("完成");
						break;
					}
				}
			}
		}
        catch(InvalidOperationException)
		{
			return;
		}
		TargetReceiver.Note.Clear();
	}

	public static float CalcHoldLength(float StartPosition,float EndPosition)//e.g: CalcHoldLength(mousePosition.y, GetEndPosition(true)) / Scale)
	{
		return Mathf.Abs(EndPosition - StartPosition) * (7f / GameTime.BeatCutCount);// 跨了几小节 * 小节间间距
	}
	public static void RefreshReceiverNoteData()//根据选定的音符刷新编辑界面
    {
		    TargetReceiver = ReceiverManager.ReceiverDic[ReceiverManager.TargetReceiver].Receiver;
			GameObject[] Notes= GameObject.FindGameObjectsWithTag("Note");
		    foreach (GameObject n in Notes)
            {
			   n.SetActive(false);
			   //Debug.Log("关闭一次");
			}
		   // Debug.Log("关闭完成");
			foreach (KeyValuePair<GameObject, NoteBase> kvp in NoteDic)//遍历当前所有音符信息
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
        if (index != 4)//非hold音符统一采用此方式
        {
			GameObject note = Instantiate(Note, mousePosition, Note.transform.rotation, FatherObject.transform);
			note.name = "Note";
			//编辑面板实例化一个2d音符对象
			NoteBase notemes = new NoteBase(SetPosition(note) * GameTime.secPerBeat, index, Angle, 1, false);
			//音符信息实例化
			NoteDic.Add(note, notemes);//根据字典绑定音符信息和unity2d对象
			TargetReceiver.Note.Add(notemes);//存入当前指定接收器的note列表
			Debug.Log(TargetReceiver.Position_x);
		}
        else
        {
			//实例化Hold音符方法
			Debug.Log("实例化Hold音符");
			GameObject note = Instantiate(Note, mousePosition, Note.transform.rotation, FatherObject.transform);
			NoteBase notemes = new NoteBase(SetPosition(note) * GameTime.secPerBeat,
				GetEndPosition() * GameTime.secPerBeat, index, 0, 1, false);
			note.name = "Note";

			note.transform.GetChild(1).gameObject.GetComponent<Transform>().transform.localScale =
				new Vector3(0.5724017f, CalcHoldLength(mousePosition.y, GetEndPosition(true)) / Scale);
			//Note.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();//获取对象的子对象的sprite的方法
			Debug.Log(notemes.start_time);
			Debug.Log(notemes.Finish_time);
			NoteDic.Add(note, notemes);//根据字典绑定音符信息和unity2d对象
			TargetReceiver.Note.Add(notemes);//存入当前指定接收器的note列表
			
			
        }
	}
	void NoteInput_Delete()
    {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)&& hit.collider.gameObject.name=="Note"&& hit.collider.gameObject.activeSelf)
		    { 
			TargetReceiver.Note.Remove(NoteDic[hit.collider.gameObject]);//移出当前接收器note序列
			NoteDic.Remove(hit.collider.gameObject);//移出字典
			Destroy(hit.collider.gameObject);//删除实例化的u2d对象
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
<<<<<<< HEAD
		lines = GameObject.FindGameObjectsWithTag("Line");
		GameObject TargetObject = lines[0];//Ĭ�ϳ�ʼ��
			float diff = (lines[0].transform.position.y - NOTE.transform.position.y);//����y�����ֵ
			diff = Mathf.Abs(diff);//ȡ����ֵ
=======
			GameObject TargetObject = lines[0];//默认初始化
			float diff = (lines[0].transform.position.y - NOTE.transform.position.y);//计算y距离差值
			diff = Mathf.Abs(diff);//取绝对值
>>>>>>> 038fab1b715826523ac4091a8e1e4de63ba90dc1
			float Target = diff;
			foreach (GameObject l in lines)//遍历所有时间线
			{
				diff = (l.transform.position.y - NOTE.transform.position.y); //计算note与line的距离差
				diff = Mathf.Abs(diff);//取绝对值
				if (diff < Target)
				{ //找出最近距离
					Target = diff;
					TargetObject = l;//取得最近线条
				}
			}
			NOTE.transform.position = TargetObject.transform.position;//放置吸附后音符
		    NOTE.transform.position = new Vector3(-6.5f, NOTE.transform.position.y, NOTE.transform.position.z);
			return int.Parse(TargetObject.name);//根据线的编号计算对应时间
    }

	float GetEndPosition(bool isPosition = false)//复用，当isPosition为false时返回编号
	{
		GameObject[] lines;//存储所有线信息
		lines = GameObject.FindGameObjectsWithTag("Line");
		GameObject TargetObject = lines[0];//默认初始化
		float diff = (lines[0].transform.position.y - endPosition.y);//计算y距离差值
		diff = Mathf.Abs(diff);//取绝对值
		float Target = diff;
		foreach (GameObject l in lines)//遍历所有时间线
		{
			diff = (l.transform.position.y - endPosition.y); //计算endPosition与line的距离差
			diff = Mathf.Abs(diff);//取绝对值
			if (diff < Target)
			{ //找出最近距离
				Target = diff;
				TargetObject = l;//取得最近线条
			}
		}
		if (!isPosition)//返回编号
		{
			return int.Parse(TargetObject.name);//根据线的编号计算对应时间
		}
		else
		{
			return TargetObject.transform.position.y;
		}
		
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
