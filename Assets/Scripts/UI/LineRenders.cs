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
}
public class CutLine
{
    public float LineTime;//当前线的对应时间
}
