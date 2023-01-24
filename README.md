# RenoteLevelDesign手册

## 简介与配置：

本软件为音乐游戏Renote的制谱工具。

本软件使用Unity2021.3.14+C#开发。

请确保你的Unity版本一致。

我们使用CODING来托管项目文件

我们的仓库地址：https://e.coding.net/renote/renote-editor/RENOTE_Editor.git

配置远程仓库方法：

1:在你需要保存工程文件的文件夹内鼠标右键，选择“git bash here”，打开git控制台。

2：输入 **git init** 回车，初始化项目，把这个项目变成一个Git可以管理的仓库，这时项目根目录出现 **.git** 隐藏文件夹。没事请不要手动修改这个目录里面的文件。

3： 输入 **git remote add origin https://e.coding.net/renote/renote-editor/RENOTE_Editor.git**

配置远程仓库地址，将本地仓库跟远程仓库连接起来。

4：在你完成工程的各类修改后，你需要及时提交变更至仓库，完毕。

5：提交前，你需要做这些操作：

​     <u>**1：至少删除工程文件中的Library文件夹**</u>

​      <u>**2：若提交时出现了 git提示“warning: LF will be replaced by CRLF“且你使用Windows电脑操作，你需要在git中输入：**</u>

​      <u>**git config --global core.autocrlf true**</u>

然后正常提交即可。

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
    public static float songsLength;//编辑界面的竖Line利用这个完成动态创建长度
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

# NoteBase抽象类

是所有音符类型的父类，NoteBase类继承MonoBehaviour，其他音符类则继承该类

## 字段：

```c#
    public double time;//记录音符的理论时间位置
    public int NoteType;//记录音符类型
    public Sprite texture;//记录音符贴图
```

该类后续会继续扩充

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
    public SpriteRenderer sr;
    public double size;//接收器放大倍率
    public double alpha;//接收器图片alpha值
    public List<NoteBase> NoteBase_List;//存储该接收器内所有音符信息
    public int state;//接收器状态
```

## 方法：

```c#
待补充
```

