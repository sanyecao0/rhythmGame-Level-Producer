using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    //信息面板使用
    public GameObject ReceiverDataPanel;
    public GameObject NoteDataPanel;
    public GameObject EventDataPanel;
    public GameObject UIEventDataPanel;
    public GameObject TimingDataPanel;

    public Text EventType;
    public Text startTime;
    public Text endTime;
    public Dropdown formulaType;
    public Dropdown formulaType_Type;
    public InputField Target;

    public Text UIEvent_StartTime;
    public Text UiEvent_EndTime;
    public InputField TargetString;

    public Text StartTime;
    public InputField TimingNewBpm;

    //编辑界面
    public GameObject SizeEventObject;
    public GameObject alphaEventObject;
    public GameObject PosEventObject;
    public GameObject TimingGroupObject;
    public GameObject UIeventObject;

    public Dropdown TypeChoose;
    public Dropdown formulaTypeChoose;
    public Dropdown formulaType_TypeChoose;

    private int type = 0;
    private string FormulaType = "LINEAR";
    private string FormulaType_type = "EASE_IN";

    public GameObject FatherObject;
    bool isDelete = false;
    public static Receiver TargetReceiver;//当前编辑的接收器
    public ReceiverEvent TargetRecEvent;//当前信息面板编辑的事件
    public UIEvent TargetUIevent;
    public TimingGroup TargetTiming;
    public static Dictionary<GameObject, ReceiverEvent> EventDic = new Dictionary<GameObject, ReceiverEvent>();
    public static Dictionary<GameObject, UIEvent> UIEventsDic = new Dictionary<GameObject, UIEvent>();
    public static IDictionary<GameObject, TimingGroup> TimingDic = new Dictionary<GameObject, TimingGroup>();

    List<Vector3> HoldMousePosition = new List<Vector3>();
    Vector3 mousePosition;
    List<GameObject> TargetLine = new List<GameObject>();
    GameObject[] lines;//存储所有线信息

   public  static bool Ready = false;
    private void Awake()
    {
        Debug.Log("Awake");
        StartCoroutine(StartLoad());
        FatherObject.SetActive(false);
    }
    IEnumerator StartLoad()
    {
        try
        {
            LoadUIEvent();
            LoadTimingGroup();
            LoadReourceRecEventData();
            RefreshReceiverEventData();
        }
        catch { };
        yield return new WaitForEndOfFrame();
    }

    private void Update()
    {
        GetChangeType();
        if (Input.GetMouseButtonUp(0) && FatherObject.activeSelf&&Input.mousePosition.x<=746)//左键删添音符
        {
            //Debug.Log(Input.mousePosition);
            if (isDelete)
            {
                DeleteEvent();
            }
            else
            {
                if (type == 4)
                {
                    mousePosition=Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 14f));
                    Debug.Log("安装Timing");
                    TimingInput();
                    }
                else
                {
                    HoldMousePosition.Add(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 14f)));
                    if (HoldMousePosition.Count == 2)
                        EventInput();
                    else
                    {
                        TargetLine.Add(GetNearestLine(HoldMousePosition[0]));//拿到第一个点击位置的最近线条
                    }
                }
            }

        }
        else if (Input.GetMouseButtonDown(1))//选择接收器
        {
            if (Input.mousePosition.x >= 746)//右半屏幕
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == "Receiver(Clone)")//隐患
                {
                    TargetReceiver = ReceiverManager.ReceiverDic[hit.collider.gameObject];
                    //Debug.Log("选择成功");
                }
            }
        }
        else if(Input.GetMouseButton(1)&& FatherObject.activeSelf)//右键查看信息
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == "RecEvent" && hit.collider.gameObject.activeSelf)
            {
                TargetRecEvent = EventDic[hit.collider.gameObject];
                OpenRecDataPanel();
            }
            else if(Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == "UIEvent(Clone)" && hit.collider.gameObject.activeSelf)
            {
                TargetUIevent = UIEventsDic[hit.collider.gameObject];
                OpenUIEventDataPanel();
            }
            else if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == "TimingGroup(Clone)" && hit.collider.gameObject.activeSelf)
            {
                TargetTiming = TimingDic[hit.collider.gameObject];
                OpenTimingGroupPanel();
            }
        }
    }
    public GameObject GetNearestLine(Vector3 MousePosition)
    {
        lines = GameObject.FindGameObjectsWithTag("EventLine");
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
    void TimingInput()
    {
        TargetLine.Add(GetNearestLine(mousePosition));//拿到第2个点击位置的最近线条
        GameObject TimingGroup = Instantiate(GetEventGameObject(),
          TargetLine[0].transform.position,
          GetEventGameObject().transform.rotation,
          FatherObject.transform);
        SetPosition(TimingGroup);
        TimingGroup tg = new TimingGroup(float.Parse(TargetLine[0].name) * GameTime.secPerBeat, GameTime.Basic_BPM);
        TimingDic.Add(TimingGroup, tg);
        Data.root.OtherEvent.TimingGroup.Add(tg);
        //Debug.Log("时间组放置");
        HoldMousePosition.Clear();
        TargetLine.Clear();
    }
    void EventInput()
    {
        GetChangeType();
        if (type==0||type==1||type==2)
        {
            //Debug.Log("进入接收器事件");
            InputRecEvent();
        }
        else if(type==3)
        {
            //Debug.Log("进入UI事件");
            InputUIEvent();
        }
       // Debug.Log(HoldMousePosition.Count+"清理前");
        HoldMousePosition.Clear();
        TargetLine.Clear();
       // Debug.Log(HoldMousePosition.Count + "清理后");
    }
    public void InputRecEvent()
    {
        TargetLine.Add(GetNearestLine(HoldMousePosition[1]));//拿到第2个点击位置的最近线条
        //Debug.Log("第2位置get");
        GameObject RecEvent = Instantiate(GetEventGameObject(), TargetLine[0].transform.position, GetEventGameObject().transform.rotation, FatherObject.transform);
        SetPosition(RecEvent);
        ReceiverEvent eventmes = new ReceiverEvent(type, float.Parse(TargetLine[0].name) * GameTime.secPerBeat,
        float.Parse(TargetLine[1].name) * GameTime.secPerBeat, FormulaType + FormulaType_type, "0");
        RecEvent.name = "RecEvent";
        RecEvent.transform.GetChild(1).gameObject.GetComponent<Transform>().transform.localScale =
            new Vector3(0.5f, (Mathf.Abs(TargetLine[1].transform.position.y - TargetLine[0].transform.position.y) / 3.3f), 0);
        EventDic.Add(RecEvent, eventmes);//根据字典绑定音符信息和unity2d对象
        TargetReceiver.Event.Add(eventmes);//存入当前指定接收器的event列
        //Debug.Log("rec放置完成");
    }
    public void InputUIEvent()
    {
        TargetLine.Add(GetNearestLine(HoldMousePosition[1]));//拿到第2个点击位置的最近线条
        //Debug.Log("第2位置getui事件");
        GameObject UIEvent = Instantiate(GetEventGameObject(),
            TargetLine[0].transform.position,
            GetEventGameObject().transform.rotation,
            FatherObject.transform);
        SetPosition(UIEvent);
        UIEvent.transform.GetChild(1).gameObject.GetComponent<Transform>().transform.localScale =
        new Vector3(0.5f, (Mathf.Abs(TargetLine[1].transform.position.y - TargetLine[0].transform.position.y) / 3.3f), 0);
        UIEvent uievent = new UIEvent(0, float.Parse(TargetLine[0].name) * GameTime.secPerBeat,
        float.Parse(TargetLine[1].name) * GameTime.secPerBeat, "这是一段信息");
        UIEventsDic.Add(UIEvent, uievent);
        Data.root.OtherEvent.UIEvent.Add(uievent);
        //Debug.Log("UI放置完成");
    }
    public void LoadReourceRecEventData()
    {

        for (int i = 0; i < Data.root.NoteData.Count; i++)
        {
            if (Data.root.NoteData[i].Event.Count != 0)
            {
                for (int j = 0; j < Data.root.NoteData[i].Event.Count; j++)
                {
                    int type = Data.root.NoteData[i].Event[j].EventType;
                    float startTime = Data.root.NoteData[i].Event[j].start_time;
                    float endtime = Data.root.NoteData[i].Event[j].end_time;
                    GameObject RecEvent = Instantiate(GetEventGameObject(type), GetResourcePosition(type, startTime), GetEventGameObject(type).transform.rotation, FatherObject.transform);
                    RecEvent.name = "RecEvent";
                    RecEvent.transform.GetChild(1).gameObject.GetComponent<Transform>().transform.localScale =
                    new Vector3(0.5f, (Mathf.Abs(endtime - startTime) * 5.25f / 3.3f), 0);
                    EventDic.Add(RecEvent, Data.root.NoteData[i].Event[j]);
                }
            }
        }
        Ready = true;
    }
    private void LoadTimingGroup()
    {
        if(Data.root.OtherEvent.TimingGroup.Count!=0)
        for (int i = 0; i < Data.root.OtherEvent.TimingGroup.Count; i++)
        {
            GameObject Timing = Instantiate(TimingGroupObject, new Vector3(-2.05f, Data.root.OtherEvent.TimingGroup[i].time * 5.25f-4, 8), TimingGroupObject.transform.rotation, FatherObject.transform);
            TimingDic.Add(Timing, Data.root.OtherEvent.TimingGroup[i]);
        }
    }
    private void LoadUIEvent()
    {
        if (Data.root.OtherEvent.UIEvent.Count != 0)
            for (int i = 0; i < Data.root.OtherEvent.UIEvent.Count; i++)
        {
            float startTime = Data.root.OtherEvent.UIEvent[i].start_time;
            float endtime = Data.root.OtherEvent.UIEvent[i].end_time;
            GameObject UIEvent = Instantiate(UIeventObject, new Vector3(-3.55f,startTime*5.25f-4, 8), UIeventObject.transform.rotation, FatherObject.transform);
            UIEvent.transform.GetChild(1).gameObject.GetComponent<Transform>().transform.localScale = new Vector3(0.5f, (Mathf.Abs(endtime - startTime) * 5.25f / 3.3f), 0);
            UIEventsDic.Add(UIEvent, Data.root.OtherEvent.UIEvent[i]);
        }
    }
    public Vector3 GetResourcePosition(int type,float startTime)
    {
        switch (type)
        {
            case 0://大小
                {
                    return new Vector3(-8.05f, -4+startTime * 5.25f, 8);
                }
            case 1://位移
                {
                    return new Vector3(-6.55f, -4+startTime * 5.25f, 8);
                }
            case 2://透明度
                {
                    return new Vector3(-5.05f, -4+startTime * 5.25f, 8);
                }
            case 3://UI字幕
                {
                    return new Vector3(-3.55f, -4+startTime * 5.25f, 8);
                }
            case 4:
                {
                    return new Vector3(-2.05f, -4+startTime * 5.25f, 8);
                }
            default:return new Vector3(-8.05f, -4, 8);
        }
    }
    public GameObject GetEventGameObject()
    {
        GetChangeType();
        switch (type)
        {
            case 0://大小
                {
                    return SizeEventObject;
                }
            case 1://位移
                {
                    return PosEventObject;
                }
            case 2://透明度
                {
                    return alphaEventObject;
                }
            case 3://UI字幕
                {
                    return UIeventObject;
                }
                case 4:{
                    return TimingGroupObject;
                }
            default:return  SizeEventObject;
        }
    }
    public GameObject GetEventGameObject(int type)
    {
        switch (type)
        {
            case 0://大小
                {
                    return SizeEventObject;
                }
            case 1://位移
                {
                    return PosEventObject;
                }
            case 2://透明度
                {
                    return alphaEventObject;
                }
            case 3://UI字幕
                {
                    return UIeventObject;
                }
            case 4:
                {
                    return TimingGroupObject;
                }
            default: return SizeEventObject;
        }
    }
    public void SetPosition(GameObject RecEvent)
    {
        GetChangeType();
        switch (type)
        {
            case 0://大小
                {
                    RecEvent.transform.position = new Vector3(-8.05f,RecEvent.transform.position.y,RecEvent.transform.position.z);
                    break;
                }
            case 1://位移
                {
                    RecEvent.transform.position = new Vector3(-6.55f, RecEvent.transform.position.y, RecEvent.transform.position.z);
                    break;
                }
            case 2://透明度
                {
                    RecEvent.transform.position = new Vector3(-5.05f, RecEvent.transform.position.y, RecEvent.transform.position.z);
                    break;
                }
            case 3://UI事件
                {
                    RecEvent.transform.position = new Vector3(-3.55f, RecEvent.transform.position.y, RecEvent.transform.position.z);
                    break;
                }
            case 4://时间组
                {
                    RecEvent.transform.position = new Vector3(-2.05f, RecEvent.transform.position.y, RecEvent.transform.position.z);
                    break;
                }
        }
    }
 
    public void DeleteEvent()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == "RecEvent" && hit.collider.gameObject.activeSelf)
        {
            TargetReceiver.Event.Remove(EventDic[hit.collider.gameObject]);//移出当前接收器Event序列
            EventDic.Remove(hit.collider.gameObject);//移出字典
            Destroy(hit.collider.gameObject);//删除实例化的u2d对象
        }
        else if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == "UIEvent(Clone)" && hit.collider.gameObject.activeSelf)
        {
            Data.root.OtherEvent.UIEvent.Remove(UIEventsDic[hit.collider.gameObject]);
            UIEventsDic.Remove(hit.collider.gameObject);
            Destroy(hit.collider.gameObject);
        }
        else if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == "TimingGroup(Clone)" && hit.collider.gameObject.activeSelf)
        {
            Data.root.OtherEvent.TimingGroup.Remove(TimingDic[hit.collider.gameObject]);
            UIEventsDic.Remove(hit.collider.gameObject);
            Destroy(hit.collider.gameObject);
        }
    }
    public void GetChangeType()
    {
        type = TypeChoose.value;
        Debug.Log("选择索引" + type);
        FormulaType = formulaType.captionText.text;
        FormulaType_type = formulaType_Type.captionText.text;
    }
    public static void RefreshReceiverEventData()
    {
        TargetReceiver = ReceiverManager.ReceiverDic[ReceiverManager.TargetReceiver];
        GameObject[] Events= GameObject.FindGameObjectsWithTag("Event");
        foreach (GameObject n in Events)
        {
            n.SetActive(false);
            //Debug.Log("关闭一次");
        }
        // Debug.Log("关闭完成");
        foreach (KeyValuePair<GameObject, ReceiverEvent> kvp in EventDic)//遍历当前所有事件信息
        {
            foreach (ReceiverEvent e in TargetReceiver.Event)
            {
                if (kvp.Value == e)
                {
                    Debug.Log(kvp.Key.transform.position.x + "Foreach IN");
                    kvp.Key.SetActive(true);
                    break;
                }
            }
        }
        Debug.Log("事件列表刷新完成");
    }
    public static void ClearReceiverEventData()
    {
        {
            List<ReceiverEvent> note = new List<ReceiverEvent>(TargetReceiver.Event);
            try
            {
                foreach (KeyValuePair<GameObject,ReceiverEvent> kvp in EventDic)//众所周知，不要用Foreach修改集合，所以这可能需要修改
                {
                    for (int j = 0; j < TargetReceiver.Event.Count; j++)
                    {
                        if (kvp.Value == note[j])
                        {
                            EventDic.Remove(kvp.Key);
                            Destroy(kvp.Key);
                            //Debug.Log("清理完成");
                            break;
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }
            TargetReceiver.Event.Clear();
        }
    }
    public void SetDelete()
    {
        isDelete = true;
    }
    public void SetAdd()
    {
        isDelete = false;
    }
    public void SetClosePanel()
    {
        NoteDataPanel.SetActive(false);
    }
    public void SetCloseRecEventPanel()
    {
        EventDataPanel.SetActive(false);
    }
    public void SetCloseUIEventDataPanel()
    {
        UIEventDataPanel.SetActive(false);
    }
    public void SetCloseTimingGroupPanel()
    {
        TimingDataPanel.SetActive(false);
    }
    public void SetCloseRecPanel()
    {
        ReceiverDataPanel.SetActive(false);
    }
    public void SaveRecData()//保存数据
    {
        TargetRecEvent.formula = formulaType.captionText.text + formulaType_Type.captionText.text;
        TargetRecEvent.Target = Target.text;
    }
    public void SaveUIEventData()
    {
        TargetUIevent.start_time=float.Parse(UIEvent_StartTime.text);
         TargetUIevent.end_time=float.Parse(UiEvent_EndTime.text);
        TargetUIevent.meaasge=TargetString.text;
    }
    public void SaveTimingGroupData()
    {
        TargetTiming.time = float.Parse( StartTime.text);
        TargetTiming.newBPM=float.Parse( TimingNewBpm.text);
    }
    public void OpenRecDataPanel()//打开面板
    {
        EventType.text = TargetRecEvent.EventType.ToString();
        formulaType.captionText.text= TargetRecEvent.formula.ToString();
        formulaType_Type.captionText.text = TargetRecEvent.formula.ToString();
        startTime.text = TargetRecEvent.start_time.ToString();
        endTime.text = TargetRecEvent.end_time.ToString();
        Target.text = TargetRecEvent.Target;
        ReceiverDataPanel.SetActive(false);
        NoteDataPanel.SetActive(false);
        EventDataPanel.SetActive(true);
    }
    public void OpenTimingGroupPanel()
    {
        StartTime.text = TargetTiming.time.ToString();
        TimingNewBpm.text = TargetTiming.newBPM.ToString();
        ReceiverDataPanel.SetActive(false);
        NoteDataPanel.SetActive(false);
        EventDataPanel.SetActive(false);
        UIEventDataPanel.SetActive(false);
        TimingDataPanel.SetActive(true);
    }
    public void OpenUIEventDataPanel()
    {
        UIEvent_StartTime.text = TargetUIevent.start_time.ToString();
        UiEvent_EndTime.text = TargetUIevent.end_time.ToString();
        TargetString.text = TargetUIevent.meaasge;
        ReceiverDataPanel.SetActive(false);
        NoteDataPanel.SetActive(false);
        EventDataPanel.SetActive(false);
        UIEventDataPanel.SetActive(true);
        TimingDataPanel.SetActive(false);
    }
}
