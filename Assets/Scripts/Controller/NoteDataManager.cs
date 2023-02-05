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
	public static int index;//音符索引
	public static float Angle = 0;//生成角度,默认为0°即正上方
	public InputField inputAngle;//绑定角度输入框
	public bool NoteDelete = false;
	private Vector3 mousePosition, targetPosition, endPosition;

	public Dictionary<GameObject, Receiver> ReciverDic = new Dictionary<GameObject, Receiver>();
	public Dictionary<GameObject, NoteBase> NoteDic = new Dictionary<GameObject, NoteBase>();//存储note信息
	private Receiver TargetReciver;//当前编辑的接收器

	private void Awake()
	{
		Note = BlackClick;
		if (DataManager.NoteData.Count == 0)//新谱面应至少有一个接收器
		{
			Receiver r = new Receiver(1.0, 255, 640, 360);//默认构造接收器
			DataManager.NoteData.Add(r);
			TargetReciver = r;
		}
	}
	void Update()
	{
		if (FatherObject.activeSelf)//避免误触
		{
			if (Input.GetMouseButtonUp(0) && Input.mousePosition.x < 423)//左键抬起放置音符，且必须在左半屏MOUS
			{
				if (!EventSystem.current.IsPointerOverGameObject()) //如果没点到UI上
				{
					mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//转换世界坐标
					if (NoteDelete)//判断音符操作模式
						NoteInput_Delete();
					else
						NoteInput();
				}
			}
			else if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > 423)
			{
				Debug.Log("点击右半屏幕");
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == ReceiverPanel)
				{
					Debug.Log("点击了接收器面板");
				}
			}
		}
	}
	void NoteInput()
    {
        if (index != 4)//非hold音符统一采用此方式
        {
			GameObject note = Instantiate(Note, mousePosition, Note.transform.rotation, FatherObject.transform);
			//编辑面板实例化一个2d音符对象
			NoteBase notemes = new NoteBase(SetPosition(note) * GameTime.secPerBeat, index, Angle, 1, false);
			//音符信息实例化
			Debug.Log(notemes.start_time);
			NoteDic.Add(note, notemes);//根据字典绑定音符信息和unity2d对象
			TargetReciver.Note.Add(notemes);//存入当前指定接收器的note列表
		}
        else
        {
			//实例化Hold音符方法
			Debug.Log("实例化Hold音符");
			GameObject note = Instantiate(Note, Note.transform.position, Note.transform.rotation, FatherObject.transform);
			NoteBase notemes = new NoteBase(SetPosition(note) * GameTime.secPerBeat,
				GetEndPosition() * GameTime.secPerBeat, index, 0, 1, false);
			Debug.Log(notemes.start_time);
			Debug.Log(notemes.Finish_time);
			NoteDic.Add(note, notemes);//根据字典绑定音符信息和unity2d对象
			TargetReciver.Note.Add(notemes);//存入当前指定接收器的note列表
        }
	}
	void NoteInput_Delete()
    {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
			//Debug.Log(hit.collider.gameObject);
			TargetReciver.Note.Remove(NoteDic[hit.collider.gameObject]);//移出当前接收器note序列
			NoteDic.Remove(hit.collider.gameObject);//移出字典
			DestroyImmediate(hit.collider.gameObject);//删除实例化的u2d对象
		    }
	}
	Vector3 GetNotePos()
	{//坐标转换
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
			GameObject[] lines;//存储所有线信息
			lines = GameObject.FindGameObjectsWithTag("Line");
			GameObject TargetObject = lines[0];//默认初始化
			float diff = (lines[0].transform.position.y - NOTE.transform.position.y);//计算y距离差值
			diff = Mathf.Abs(diff);//取绝对值
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

	int GetEndPosition()
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
}
