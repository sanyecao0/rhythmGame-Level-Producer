using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public GameObject MessageBox;
    public InputField EventType;
    public InputField start_Time;
    public InputField end_Time;
    public InputField formula;
    public InputField Target;

    public static ReceiverMes TargetReceiver;//当前编辑的接收器

    public void AddReceiverEvent()
    {
        int eventType = int.Parse(EventType.text);
        float startTime = float.Parse(start_Time.text);
        float endTime = float.Parse(end_Time.text);
        ReceiverEvent e=new ReceiverEvent(eventType,startTime,endTime,formula.text,Target.text);
        TargetReceiver.Receiver.Event.Add(e);
    }
}
