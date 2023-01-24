using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenders : MonoBehaviour
{
    private float SongCutNum;//����С����
    public LineRenderer lineRenderL;
    public LineRenderer BarCutLine;//һ�Ļ�һ�������
    public LineRenderer SecCutLine;//С�ڲ����
    public GameObject FatherObject;

    public static List<CutLine> linesData=new List<CutLine>();//�洢�����ߵ�ʱ����Ϣ

    int BeatCut = GameTime.BeatCutCount;
    void Start()
    {
        SongCutNum = (GameTime.songsLength * GameTime.Basic_BPM / 60f);//���С����,���ɶ�Ӧ������
        BeatCut = GameTime.BeatCutCount;
        LineDraw();
    }
    private void Update()
    {
        if (BeatCut != GameTime.BeatCutCount)
        {
            //��Ҫһ������ɵĻ����ߵķ���
            LineDraw();
        }
           
    }
    private void LineDraw()
    {
        lineRenderL.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        float LineSpacing=lineRenderL.transform.localScale.z/ SongCutNum /GameTime.BeatCutCount;//�߼��
        for (int i = 0; i < SongCutNum * GameTime.BeatCutCount; i++)
        {
            if (i % GameTime.BeatCutCount == 0)//����
            {
                LineRenderer line = Instantiate(BarCutLine, new Vector3(-8, -4 + i * LineSpacing, 15), BarCutLine.transform.rotation, FatherObject.transform);
                CutLine c = new CutLine();
                c.LineTime = i * GameTime.secPerBeat/GameTime.BeatCutCount;//���㵱ǰ�ߵ�ʱ���
                linesData.Add(c);
                //Debug.Log(c.LineTime);
                line.name = i.ToString();
            }
            else
            {
                LineRenderer cutline = Instantiate(SecCutLine, new Vector3(-8, -4 + i  *LineSpacing, 15), SecCutLine.transform.rotation, FatherObject.transform);
                CutLine c = new CutLine();
                c.LineTime = i * GameTime.secPerBeat / GameTime.BeatCutCount;//���㵱ǰ�ߵ�ʱ���
                linesData.Add(c);
                //Debug.Log(c.LineTime);
                cutline.name = i.ToString();
            }
        }
    }
}
public class CutLine
{
    public float LineTime;//��ǰ�ߵĶ�Ӧʱ��
}
