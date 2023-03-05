using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLines : MonoBehaviour
{
    public static float SongCutNum;//����С����
    public LineRenderer lineRender1;
    public LineRenderer lineRender2;
    public LineRenderer lineRender3;
    public LineRenderer lineRender4;
    public LineRenderer lineRender5;
    public LineRenderer lineRender6;
    public LineRenderer BarCutLine;//һ�Ļ�һ�������
    public LineRenderer SecCutLine;//С�ڲ����
    public GameObject FatherObject;

    int BeatCut = GameTime.BeatCutCount;
    public static List<CutLine> linesData = new List<CutLine>();//�洢�����ߵ�ʱ����Ϣ
    void Start()
    {
        SongCutNum = (GameTime.songsLength * GameTime.Basic_BPM / 60f);//���С����,���ɶ�Ӧ������
        lineRender1.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        lineRender2.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        lineRender3.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        lineRender4.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        lineRender5.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        lineRender6.transform.localScale += new Vector3(0, 0, SongCutNum * 7 - 1);
        //Debug.Log(SongCutNum);
        //SongCutNum = (GameTime.songsLength * GameTime.Basic_BPM / 60f);//���С����,���ɶ�Ӧ������
        BeatCut = GameTime.BeatCutCount;
        LineDraw();
    }

    // Update is called once per frame
    void Update()
    {
        if (BeatCut != GameTime.BeatCutCount)
        {
            //��Ҫһ������ɵĻ����ߵķ���
            ClearOldLines();
            LineDraw();
        }
    }
    private void LineDraw()
    {
        float LineSpacing = lineRender1.transform.localScale.z / SongCutNum / GameTime.BeatCutCount;//�߼��
        for (int i = 0; i < SongCutNum * GameTime.BeatCutCount; i++)
        {
            if (i % GameTime.BeatCutCount == 0)//����
            {
                LineRenderer line = Instantiate(BarCutLine, new Vector3(-9, -4 + i * LineSpacing, 8),BarCutLine.transform.rotation, FatherObject.transform);
                line.transform.localScale += new Vector3(0, 0, 5);
                line.tag = "EventLine";
                CutLine c = new CutLine();
                c.LineTime = i * GameTime.secPerBeat / GameTime.BeatCutCount;//���㵱ǰ�ߵ�ʱ���
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
                c.LineTime = i * GameTime.secPerBeat / GameTime.BeatCutCount;//���㵱ǰ�ߵ�ʱ���
                linesData.Add(c);
                cutline.name = i.ToString();
            }
        }
    }

    void ClearOldLines()
    {
        BeatCut = GameTime.BeatCutCount;//���������ͬ��
        linesData.Clear();

        bool isFatherActive = FatherObject.activeSelf;//��FatherObject�ļ���״̬

        FatherObject.SetActive(true);
        GameObject[] lines = GameObject.FindGameObjectsWithTag("EventLine");
        for(int i = 0; i < lines.Length; i++)
        {
            Debug.Log("Event��������");
            DestroyImmediate(lines[i]);
        }
        if (!isFatherActive)
        {
            FatherObject.SetActive(false);
        }
    }
}
