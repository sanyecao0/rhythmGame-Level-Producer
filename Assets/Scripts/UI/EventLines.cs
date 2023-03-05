using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLines : MonoBehaviour
{
    public static float SongCutNum;//歌曲小节数
    public LineRenderer lineRender1;
    public LineRenderer lineRender2;
    public LineRenderer lineRender3;
    public LineRenderer lineRender4;
    public LineRenderer lineRender5;
    public LineRenderer lineRender6;
    public LineRenderer BarCutLine;//一拍画一个这个线
    public LineRenderer SecCutLine;//小节拆分线
    public GameObject FatherObject;

    int BeatCut = GameTime.BeatCutCount;
    public static List<CutLine> linesData = new List<CutLine>();//存储所有线的时间信息
    void Start()
    {
        SongCutNum = (GameTime.songsLength * GameTime.Basic_BPM / 60f);//算出小节数,生成对应长度线
        lineRender1.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        lineRender2.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        lineRender3.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        lineRender4.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        lineRender5.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        lineRender6.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        //Debug.Log(SongCutNum);
        //SongCutNum = (GameTime.songsLength * GameTime.Basic_BPM / 60f);//算出小节数,生成对应长度线
        BeatCut = GameTime.BeatCutCount;
        LineDraw();
    }

    // Update is called once per frame
    void Update()
    {
        if (BeatCut != GameTime.BeatCutCount)
        {
            //需要一个清除旧的划分线的方法
            ClearOldLines();
            LineDraw();
        }
    }
    private void LineDraw()
    {
        float LineSpacing = lineRender1.transform.localScale.z / SongCutNum / GameTime.BeatCutCount;//线间距
        for (int i = 0; i < SongCutNum * GameTime.BeatCutCount; i++)
        {
            if (i % GameTime.BeatCutCount == 0)//画线
            {
                LineRenderer line = Instantiate(BarCutLine, new Vector3(-9, -4 + i * LineSpacing, 8),BarCutLine.transform.rotation, FatherObject.transform);
                line.transform.localScale += new Vector3(0, 0, 5);
                line.tag = "EventLine";
                CutLine c = new CutLine();
                c.LineTime = i * GameTime.secPerBeat / GameTime.BeatCutCount;//计算当前线的时间点
                linesData.Add(c);
                //Debug.Log(c.LineTime);
                line.name = i.ToString();
            }
            else
            {
                LineRenderer cutline = Instantiate(SecCutLine, new Vector3(-9, -4 + i * LineSpacing, 8), SecCutLine.transform.rotation, FatherObject.transform);
                cutline.transform.localScale += new Vector3(0, 0, 5);
                cutline.tag = "EventLine";
                CutLine c = new CutLine();
                c.LineTime = i * GameTime.secPerBeat / GameTime.BeatCutCount;//计算当前线的时间点
                linesData.Add(c);
                cutline.name = i.ToString();
            }
        }
    }

    void ClearOldLines()
    {
        BeatCut = GameTime.BeatCutCount;//与输入节拍同步
        linesData.Clear();

        bool isFatherActive = FatherObject.activeSelf;//存FatherObject的激活状态

        FatherObject.SetActive(true);
        GameObject[] lines = GameObject.FindGameObjectsWithTag("EventLine");
        for(int i = 0; i < lines.Length; i++)
        {
            Debug.Log("Event线清除完成");
            DestroyImmediate(lines[i]);
        }
        if (!isFatherActive)
        {
            FatherObject.SetActive(false);
        }
    }
}
