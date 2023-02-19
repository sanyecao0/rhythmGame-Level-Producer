using System.Collections.Generic;
public class Receiver
{

    public float size;
    public float Position_x;
    public float Position_y;
    public int alpha;
    public List<Note> Note=new List<Note>();
    public List<ReceiverEvent> Event=new List<ReceiverEvent>();
    public Receiver(float size, int alpha, float x, float y)
    {
        this.size = size;
        this.alpha = alpha;
        this.Position_x = x;
        this.Position_y = y;
    }
    public Receiver() { }
}

public class Note
{
    public float start_time;
    public float degree;
    public int type;
    public float speed;
    public float end_time;
    public bool fake;
    public Note(float stime, float ftime, int type, float angle, float speed, bool isfake)//holdר构造方法
    {
        this.start_time = stime;
        this.end_time = ftime;
        this.type = type;
        this.degree = angle;
        this.speed = speed;
        this.fake = isfake;
    }
    public Note(float stime, int t, float angle, float speed, bool isfake)//普通note构造方法
    {
        this.start_time = stime;
        this.type = t;
        this.degree = angle;
        this.speed = speed;
        this.fake = isfake;
    }
    public Note() { }
}

public class ReceiverEvent
{
    public int EventType;
    public float start_time;
    public float end_time;
    public string formula;
    public string Target;
    public ReceiverEvent(int type, float BeginTime, float FinishTime, string formula, string Target)
    {
        this.EventType = type;
        this.start_time= BeginTime;
        this.end_time = FinishTime;
        this.formula = formula;
        this.Target = Target;
    }
    public ReceiverEvent() { }
}

public class NoteData
{

    public Receiver Receiver;
}

public class TimingGroup
{
    public float time;
    public float newBPM;
    public TimingGroup(float time, float newBPM)
    {
        this.time = time;
        this.newBPM = newBPM;
    }
    public TimingGroup() { }
}

public class UIEvent
{
    public float start_time;
    public float end_time;
    public int type;
    public string meaasge;
    public UIEvent(int type, float startTime, float endTime, string Message)
    {
        this.type = type;
        this.start_time = startTime;
        this.end_time = endTime;
        this.meaasge = Message;
    }
    public UIEvent() { }
}

public class OtherEvent
{
    public List<TimingGroup> TimingGroup=new List<TimingGroup>();
    public List<UIEvent> UIEvent=new List<UIEvent>();
}
public class Data
{
    public static Root root = new Root();
    public static LevelMessage levelMessage = new LevelMessage();
}

public class Root
{
    public   List<Receiver> NoteData=new List<Receiver>();
    public   OtherEvent OtherEvent=new OtherEvent();
}
public class LevelMessage
{
    public string TrackName;
    public string Artist;
    public string BPM;
    public string LevelDesign;
    public string BasicBPM;
    public string illustrator;
    public LevelMessage() { }
}

