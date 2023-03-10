using System;
using System.Collections;
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

	public float Scale;
	public GameObject Note;
	public GameObject FatherObject;
	public GameObject DataPanel;
	public Text time;
	public InputField Degree;
	public InputField Speed;
	public Dropdown boolValue;

	public GameObject ReceiverDataPanel;
	public GameObject ReceiverEventDataPanle;
	public InputField Size;
	public InputField Alpha;
	public Text PosX;
	public Text PosY;

	public static int index;//音符索引
	public static float Angle = 0;//生成角度,默认为0°即正上方
	public InputField inputAngle;//绑定角度输入框
	public bool NoteDelete = false;
	private bool HoldInput = false;
	private Vector3 mousePosition; //endPosition;

	 List<Vector3> HoldMousePosition = new List<Vector3>();
	 List<GameObject> TargetLine = new List<GameObject>();

	GameObject[] lines;//存储所有线信息

	public static Dictionary<GameObject, Note> NoteDic = new Dictionary<GameObject, Note>();//存储note信息
	public static Receiver TargetReceiver;//当前编辑的接收器
	static Note TargetNote;
	public static bool Ready = false;
	private void Start()
	{
		//Debug.Log("Start");
		Note = BlackClick;
		lines = GameObject.FindGameObjectsWithTag("Line");
		StartCoroutine(LoadResourceData());
		FatherObject.SetActive(false);
	}
	void Update()
	{
		if (FatherObject.activeSelf)//避免误触
		{
			if (Input.GetMouseButtonUp(0) && Input.mousePosition.x < 423&& UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()&&!HoldInput)//���̧������������ұ����������
			{
			  mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 14f));
			  if (NoteDelete)//判断音符操作模式
				NoteInput_Delete();
			 else
				NoteInput();
			}
			else if(Input.GetMouseButtonUp(0) && Input.mousePosition.x < 423 &&HoldInput)//hold专属方法
            {
				//Debug.Log(HoldMousePosition.Count);
				HoldMousePosition.Add(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,14f)));
				if (HoldMousePosition.Count == 2)
					HoldNoteInput();
				else
					TargetLine.Add(GetNearestLine(HoldMousePosition[0]));//拿到第一个点击位置的最近线条
			}
			else if (Input.GetMouseButtonDown(1))//选择接收器
			{
				if(Input.mousePosition.x > 423)
                {
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == "Receiver(Clone)")//隐患
					{
						TargetReceiver = ReceiverManager.ReceiverDic[hit.collider.gameObject];
						Debug.Log("修改接收器");
						OpenRecDataPanel();
					}
				}
                else//选择音符，打开详细信息面板
                {
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name=="Note")//隐患
					{
						TargetNote = NoteDic[hit.collider.gameObject];
						OpenDataPanel();
					}
				}
			}
		}
	}
	IEnumerator LoadResourceData()//实例化note等的方法
    {
		try
		{
			if (Data.root.NoteData.Count != 0)
				for (int i = 0; i < Data.root.NoteData.Count; i++)
				{
					for (int j = 0; j < Data.root.NoteData[i].Note.Count; j++)
					{
						int type = Data.root.NoteData[i].Note[j].type;

						if (type != 4)
						{
							GameObject note = Instantiate(GetNoteType(Data.root.NoteData[i].Note[j].type),
							new Vector3(-6.5f, 1.75f * (Data.root.NoteData[i].Note[j].start_time * GameTime.Basic_BPM) / 60f - 4f, 8),
							GetNoteType(Data.root.NoteData[i].Note[j].type).transform.rotation, FatherObject.transform);
							note.name = "Note";
							NoteDic.Add(note, Data.root.NoteData[i].Note[j]);
							
						}
						else if (type == 4)
						{
							GameObject note = Instantiate(GetNoteType(Data.root.NoteData[i].Note[j].type),
							new Vector3(-6.5f, 1.75f * (Data.root.NoteData[i].Note[j].start_time * GameTime.Basic_BPM + 1) / 60f - 4f, 8),
							GetNoteType(Data.root.NoteData[i].Note[j].type).transform.rotation, FatherObject.transform);
							note.transform.GetChild(1).gameObject.GetComponent<Transform>().transform.localScale =
								new Vector3(0.5f,Mathf.Abs( (Data.root.NoteData[i].Note[j].end_time *GameTime.Basic_BPM) / 60 -(Data.root.NoteData[i].Note[j].start_time *GameTime.Basic_BPM) / 60 ) * 0.5303f, 0);
							//Data.root.NoteData[i].Note[j].end_time
							//Data.root.NoteData[i].Note[j].start_time
							note.name = "Note";
							NoteDic.Add(note, Data.root.NoteData[i].Note[j]);
						}
					}
				}
			Ready = true;
			RefreshReceiverNoteData();
			Debug.Log("ReadyNote");
		}
        catch { }
		yield return new WaitForEndOfFrame();
    }
	private  GameObject GetNoteType(int index)
	{
		switch (index)
		{
			case 0:
				{
					return RedClick;
				}
			case 1:
				{
					return WhiteClick;
				}
			case 2:
				{
					return BlackClick;
				}
			case 3:
				{
					return Drag;
				}
			case 4:
				{
					return Hold;
				}
			case 6:
				{
					return Flick;
				}
			default: return BlackClick;
		}
	}
	public static void ClearReceiverNoteData()//清空被删除的接收器的内容音符
    {
		List<Note> note = new List<Note>(TargetReceiver.Note);
        try
        {
			foreach (KeyValuePair<GameObject, Note> kvp in NoteDic)//众所周知，不要用Foreach修改集合，所以这可能需要修改
			{
				for (int j = 0; j < TargetReceiver.Note.Count; j++)
				{
					if (kvp.Value == note[j])
					{
						NoteDic.Remove(kvp.Key);
						Destroy(kvp.Key);
						Debug.Log("清理完成");
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

	public static void RefreshReceiverNoteData()//根据选定的音符刷新编辑界面
    {
		    TargetReceiver = ReceiverManager.ReceiverDic[ReceiverManager.TargetReceiver];
			GameObject[] Notes= GameObject.FindGameObjectsWithTag("Note");
		    foreach (GameObject n in Notes)
            {
			   n.SetActive(false);
			   //Debug.Log("关闭一次");
			}
		   // Debug.Log("关闭完成");
			foreach (KeyValuePair<GameObject, Note> kvp in NoteDic)//遍历当前所有音符信息
			{
				foreach (Note note in TargetReceiver.Note)
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
		GameObject note = Instantiate(Note, mousePosition, Note.transform.rotation, FatherObject.transform);
		note.name = "Note";
		//编辑面板实例化一个2d音符对象
		Note notemes = new Note(SetPosition(note) * GameTime.secPerBeat, index, Angle, 1, false);
		//音符信息实例化
		NoteDic.Add(note, notemes);//根据字典绑定音符信息和unity2d对象
		TargetReceiver.Note.Add(notemes);//存入当前指定接收器的note列表
		Debug.Log(TargetReceiver.Position_x);
	}
	void HoldNoteInput()
    {
		//实例化Hold音符方法
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//转换世界坐标
		HoldMousePosition[1] = mousePosition;
		TargetLine.Add(GetNearestLine(HoldMousePosition[1]));//拿到第2个点击位置的最近线条
		Debug.Log("拿到第二个位置");
			GameObject note = Instantiate(Note, TargetLine[0].transform.position, Note.transform.rotation, FatherObject.transform);
			Note notemes = new Note(SetPosition(note) * GameTime.secPerBeat,
				float.Parse(TargetLine[1].name) * GameTime.secPerBeat, index, 0, 1, false);
			note.name = "Note";
			note.transform.GetChild(1).gameObject.GetComponent<Transform>().transform.localScale =
				new Vector3(0.5f, (Mathf.Abs(TargetLine[1].transform.position.y - TargetLine[0].transform.position.y) / 3.3f), 0);
			//Note.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();//获取对象的子对象的sprite的方法
			//Debug.Log(notemes.start_time);
			//Debug.Log(notemes.end_time);
		    HoldMousePosition.Clear();
		     TargetLine.Clear();//清空信息
			NoteDic.Add(note, notemes);//根据字典绑定音符信息和unity2d对象
			TargetReceiver.Note.Add(notemes);//存入当前指定接收器的note列
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
					HoldInput = false;
					HoldMousePosition.Clear();
					TargetLine.Clear();
					break;
                }
			case 1:
				{
					Note = WhiteClick;
					HoldInput = false;
					HoldMousePosition.Clear();
					TargetLine.Clear();
					break;
                }
			case 2:
                {
					Note = BlackClick;
					HoldInput = false;
					HoldMousePosition.Clear();
					TargetLine.Clear();
					break;
                }
			case 3:
                {
					Note = Drag;
					HoldInput = false;
					HoldMousePosition.Clear();
					TargetLine.Clear();
					break;
                }
			case 4:
                {
					Note = Hold;
					HoldInput = true;
					break;
                }
			case 6:
                {
					Note = Flick;
					HoldInput = false;
					HoldMousePosition.Clear();
					TargetLine.Clear();
					break;
                }
		}
    }
	public GameObject GetNearestLine(Vector3 MousePosition)
    {
		lines = GameObject.FindGameObjectsWithTag("Line");
		GameObject TargetObject = lines[0];
		float diff = (lines[0].transform.position.y - MousePosition.y);//����y�����ֵ
		diff = Mathf.Abs(diff);//ȡ����ֵ
		float Target = diff;
		foreach (GameObject l in lines)//遍历所有时间线
		{
			diff = (l.transform.position.y - MousePosition.y); //计算点击位置与line的距离差
			diff = Mathf.Abs(diff);//取绝对值
			if (diff < Target)
			{ //找出最近距离
				Target = diff;
				TargetObject = l;//取得最近线条
			}
		}
		return TargetObject;
	}
	public GameObject GetNearestLine(GameObject NOTE)
	{
		lines = GameObject.FindGameObjectsWithTag("Line");
		GameObject TargetObject = lines[0];
		float diff = (lines[0].transform.position.y - NOTE.transform.position.y);//����y�����ֵ
		diff = Mathf.Abs(diff);//ȡ����ֵ
		float Target = diff;
		foreach (GameObject l in lines)//遍历所有时间线
		{
			diff = (l.transform.position.y - NOTE.transform.position.y); //计算点击位置与line的距离差
			diff = Mathf.Abs(diff);//取绝对值
			if (diff < Target)
			{ //找出最近距离
				Target = diff;
				TargetObject = l;//取得最近线条
			}
		}
		return TargetObject;
	}
	private int SetPosition(GameObject NOTE)
	{
		    GameObject TargetObject = GetNearestLine(NOTE);
			NOTE.transform.position = TargetObject.transform.position;//放置吸附后音符
		    NOTE.transform.position = new Vector3(-6.5f, NOTE.transform.position.y, NOTE.transform.position.z);
			return int.Parse(TargetObject.name);//根据线的编号计算对应时间
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
	public void SetClosePanel()
    {
		DataPanel.SetActive(false);
    }
	public void SaveData()
    {
		TargetNote.degree = float.Parse(Degree.text);
		TargetNote.speed = float.Parse(Speed.text);
		TargetNote.fake = bool.Parse(boolValue.captionText.text);
    }
	public void OpenDataPanel()
    {
		Degree.text= TargetNote.degree.ToString();
		Speed.text = TargetNote.speed.ToString();
		boolValue.captionText.text = TargetNote.fake.ToString();
		time.text = TargetNote.start_time.ToString();
		DataPanel.SetActive(true);
		ReceiverDataPanel.SetActive(false);
    }
	public void SetCloseRecPanel()
	{
		ReceiverDataPanel.SetActive(false);
	}
	public void SetCloseRecEventPanel()
    {
		ReceiverEventDataPanle.SetActive(false);

	}
	public void SaveRecData()
	{
		TargetReceiver.size = float.Parse(Size.text);
		TargetReceiver.alpha = int.Parse(Alpha.text);
	}
	public void OpenRecDataPanel()
	{
		Alpha.text = TargetReceiver.alpha.ToString();
		Size.text = TargetReceiver.size.ToString();
		PosX.text = TargetReceiver.Position_x.ToString();
		PosY.text = TargetReceiver.Position_y.ToString();
		ReceiverDataPanel.SetActive(true);
		DataPanel.SetActive(false);
	}
}
