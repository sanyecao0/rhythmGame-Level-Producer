using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenders : MonoBehaviour
{
    private float SongCutNum;//歌曲小节数
    public LineRenderer lineRenderL;
    public LineRenderer BarCutLine;//一拍画一个这个线
    public LineRenderer SecCutLine;//小节拆分线
    public GameObject FatherObject;

    public static List<CutLine> linesData=new List<CutLine>();//存储所有线的时间信息

    int BeatCut = GameTime.BeatCutCount;
    void Start()
    {
        SongCutNum = (GameTime.songsLength * GameTime.Basic_BPM / 60f);//算出小节数,生成对应长度线
        BeatCut = GameTime.BeatCutCount;
        LineDraw();
    }
    private void Update()
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
        lineRenderL.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
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
                //Debug.Log(c.LineTime);
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
public class CutLine
{
    public float LineTime;//当前线的对应时间
}
