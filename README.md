# LevelDesign手册

## 简介与配置：

本软件为某音乐游戏的制谱工具，已弃用

本软件使用Unity2021.3.14f1c1+C#开发。

C#脚本编辑工具为Visual studio社区版

请确保你的Unity版本一致。

# GameTime类

## 字段：

```c#
    public static float Basic_BPM=180;//基础BPM，默认值为180
    public static float BPM;//游戏内BPM
    public static float songPosition;//歌曲播放位置
    public static float songPosInBeats;//记录当前所在节拍
    public static float secPerBeat;//单一拍时长
    public static float AudioOffset;//谱面偏移
    public static int LineBeatCut=4;//节拍细拆
```

## 方法：

```c#
//直接绑定输入框使用即可
public void inputTime()//输入框调整播放位置
    {
        if(float.Parse(inputTimeString.text)>=0&& float.Parse(inputTimeString.text) <= songs.clip.length)
        {
            songs.time = float.Parse(inputTimeString.text);
        }
        else
        {
            inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
        }
        //Debug.Log(songs.time);
    } 
public void inputBeatut()//主输入调整节拍拆分
    {
        if(int.Parse(inputBeatCut.text)>=4&& int.Parse(inputBeatCut.text) <= 32)
        {
            LineBeatCut = int.Parse(inputBeatCut.text);
        }
        else
        {
            inputBeatCut.text = LineBeatCut.ToString();
        }
    }
    public void InputBasicBPM()//输入框输入基础BPM
    {

        try
        {
            Basic_BPM = float.Parse(inputBPM.text);
        }
        catch
        {
            Basic_BPM = 0;
        }

    }
    public void InputAudioOffset()//谱面偏移输入框赋值
    {
        try
        {
            AudioOffset = float.Parse(inputAudioOffset.text);
        }
        catch
        {
            AudioOffset = 0;
        }
    }
```

# NoteBase类

无论谱师在设计关卡时添加了何种音符，它们都会被作为一个NoteBase类对象保存

## 字段：

```c#
    public float start_time;//时间或开始时间
    public float Finish_time;//hold专属结束时间
    public int type;//音符种类
    /*0->红色click,1->白色click，2-》黑click 3-》drag，4-》hold 6-》flick*/
    public float degree;//角度
    public float speed;//默认速度
    public bool fake;//真假音符
```

## 方法：

包含两种构造方法

```c#
 public NoteBase(float stime, float ftime, int type, float angle, float speed, bool isfake)//hold专用构造方法
    {
        this.start_time = stime;
        this.Finish_time = ftime;
        this.type = type;
        this.degree = angle;
        this.speed = speed;
        this.fake = isfake;
    }
    public NoteBase(float stime, int t, float angle, float speed, bool isfake)//普通音符构造方法
    {
        this.start_time = stime;
        this.type = t;
        this.degree = angle;
        this.speed = speed;
        this.fake = isfake;
    }
```



# FileName类

调用windowsAPI，打开windows文件处理窗口

## 方法：

```c#
public class LocalDialog
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] FileName ofn);
    public static bool GetOFN([In, Out] FileName ofn)
    {
        return GetOpenFileName(ofn);//打开文件对话框
    }

    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] FileName ofn);
    public static bool GetSFN([In, Out] FileName ofn)
    {
        return GetSaveFileName(ofn);//保存文件对话框
    }
}
```

# OpenAndSaveLevel类

该类负责调用FileName类中的方法，以获取谱面文件的位置（后续需要改进以使用）

## 方法：

```c#
   public void openProject()
   {
        FileName openFileName = new FileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "relevel文件(*.relevel)\0*.xlsx";//暂时写成这样
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
        openFileName.title = "选择文件";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        if (LocalDialog.GetOpenFileName(openFileName))
         {
            Debug.Log(openFileName.file);
            Debug.Log(openFileName.fileTitle);
         }
        }
    public void saveProject()
    {
        FileName savefileName = new FileName();
        savefileName.structSize = Marshal.SizeOf(savefileName);
        savefileName.filter = "relevel文件(*.relevel)\0*.xlsx";//暂时写成这样
        savefileName.file = new string(new char[256]);
        savefileName.maxFile = savefileName.file.Length;
        savefileName.fileTitle = new string(new char[64]);
        savefileName.maxFileTitle = savefileName.fileTitle.Length;
        savefileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
        savefileName.title = "选择文件";
        savefileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        if (LocalDialog.GetSaveFileName(savefileName))
        {
            Debug.Log(savefileName.file);
            Debug.Log(savefileName.fileTitle);
        }
    }
```

# Receiver类

接收器的类，用于记录接收器的若干信息。

## 字段：

```c#
    public double size;//接收器放大倍率
    public int alpha;//接收器图片alpha值
    public double Position_x;//初始化坐标x
    public double Position_y;//初始化坐标y
    public  List<NoteBase> Note=new List<NoteBase>(); //该接收器音符列表 
```

## 方法：

包含构造方法：

```c#
public Receiver (double size,int alpha,double x,double y)
    {
        this.size = size;
        this.alpha = alpha;
        this.Position_x = x;
        this.Position_y = y;
    }
```

# NoteDataManager类

该类继承MonoBehaviour，是负责音符管理的类

## 字段：

```
public GameObject BlackClick;
    public GameObject RedClick;
    public GameObject WhiteClick;
    public GameObject Flick;
    public GameObject Hold;
    public GameObject Drag;

	public GameObject Note;
	public GameObject FatherObject;
	public static int index;//音符索引
	public static float Angle=0;//生成角度,默认为0°即正上方
	public InputField inputAngle;//绑定角度输入框
	public bool NoteDelete = false;
	Vector3 mousePosition, targetPosition;

	public Dictionary<GameObject,Receiver> ReciverDic=new Dictionary<GameObject, Receiver>();//存储接收器信息
	public Dictionary<GameObject, NoteBase> NoteDic = new Dictionary<GameObject, NoteBase>();//存储note信息
	private Receiver TargetReciver;//当前编辑的接收器
```

## 方法：

```c#
	void NoteInput()//放置音符方法
    {
        if (index != 4)//非hold音符统一采用此方式
        {
			GameObject note = Instantiate(Note, Note.transform.position, Note.transform.rotation, FatherObject.transform);
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
        }
	}
	void NoteInput_Delete()//删除音符方法
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
	Vector3 GetNotePos()//坐标转换方法
	{//坐标转换
		Vector3 imagePos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle(Note.transform.parent as RectTransform, Input.mousePosition, null, out imagePos);
		return imagePos;
	}
	private void NoteChoose()//根据ui按钮修改来决定当前放置的音符
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
	private int SetPosition(GameObject NOTE)//自动吸附功能实现
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

//以下为按钮修改值所绑定的方法
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

	public void SetAngle()//设定放置音符的角度的方法
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
```

# LineRenders类

负责为音符编辑界面划线的类

## 字段：

```c#
    private float SongCutNum;//歌曲小节数
    public LineRenderer lineRenderL;//左侧时间线
    public LineRenderer lineRenderR;//右侧时间线
    public LineRenderer BarCutLine;//一拍画一个这个线
    public LineRenderer SecCutLine;//小节拆分线
    public GameObject FatherObject;//所有线和2d音符对象的父物体

    public static List<CutLine> linesData=new List<CutLine>();//存储所有线的时间信息

    int BeatCut = GameTime.BeatCutCount;
```

## 方法：

```c#
    private void LineDraw()//动态划线方法
    {
        lineRenderL.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        lineRenderR.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        float LineSpacing=lineRenderL.transform.localScale.z/ SongCutNum /GameTime.BeatCutCount;//线间距
        for (int i = 0; i < SongCutNum * GameTime.BeatCutCount; i++)
        {
            if (i % GameTime.BeatCutCount == 0)//画线
            {
                LineRenderer line = Instantiate(BarCutLine, new Vector3(-8, -4 + i * LineSpacing, 15), BarCutLine.transform.rotation, FatherObject.transform);
                CutLine c = new CutLine();
                c.LineTime = i * GameTime.secPerBeat/GameTime.BeatCutCount;//计算当前线的时间点
                linesData.Add(c);
                //Debug.Log(c.LineTime);
                line.name = i.ToString();
            }
            else
            {
                LineRenderer cutline = Instantiate(SecCutLine, new Vector3(-8, -4 + i  *LineSpacing, 15), SecCutLine.transform.rotation, FatherObject.transform);
                CutLine c = new CutLine();
                c.LineTime = i * GameTime.secPerBeat / GameTime.BeatCutCount;//计算当前线的时间点
                linesData.Add(c);
                cutline.name = i.ToString();
            }
        }
    }
    
    void ClearOldLines()//更新新节拍拆分方法
    {
        BeatCut = GameTime.BeatCutCount;//与输入节拍同步
        linesData.Clear();

        bool isFatherActive = FatherObject.activeSelf;//存FatherObject的激活状态
        
        FatherObject.SetActive(true);
        lineRenderL.transform.localScale = new Vector3(1f, 1f, 1f);
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Line");
        foreach (var oldlines in lines)
        {
            DestroyImmediate(oldlines);
        }
        if (!isFatherActive)
        {
            FatherObject.SetActive(false);
        }
    }
}
```

# CutLine类

辅助为时间赋值的线类...

## 字段：

```c#
public float LineTime;//当前线的对应时间
```

# FollowPlayer类

鼠标滑动编辑界面的功能依赖此类

## 字段：

```c#
    float scale = 0.2f;
    bool isPlay = false;
```

## 方法：

```c#
    private void MoveUP()
    {
        transform.position += new Vector3(0, 1 * scale);
        songs.time = songs.time - 0.2f / 5.25f;
        inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
        Debug.Log("减时间");//如果进入编辑界面时没有点击播放按钮，那么该方法无效
        Debug.Log(songs.time);
    }
    private void MoveDown()
    {
        transform.position -= new Vector3(0, 1 * scale);
        songs.time = songs.time + 0.2f / 5.25f;
        inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
        Debug.Log("加时间");//如果进入编辑界面时没有点击播放按钮，那么该方法无效
        Debug.Log(songs.time);
    }
   public void MusicPlay()
    {
        isPlay = true;
    }
    public void MusicPause()
    {
        isPlay = false;
    }
```

# TimingGroup类

存储时间组信息，该类待完善

# Events类

存储接收器事件信息，待完善

# OtherEvents类

存储其他事件信息，待完善
