using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Receiver : MonoBehaviour
{
    public SpriteRenderer sr;
    public double size;//�������Ŵ���
    public double alpha;//������ͼƬalphaֵ
    public List<NoteBase> NoteBase_List;
    public int state;//������״̬

    public Event SizeChange;//��С�����¼�
    public Event PositionChange;//λ�ø����¼�
    public Event AlphaChange;//͸�����޸��¼�
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// �������Ĵ�С�ı乫ʽ
    /// </summary>
    public void SizeChangeEvent(double BeginTime,double FinishTime,string formula,double TargetSize)
    {
   
    }
    /// <summary>
    /// ��������λ�øı䷽��
    /// </summary>
    public void PositionChangeEvent(double BeginTime, double FinishTime, string formula,double TargetX,double TargetY)
    {

    }
    /// <summary>
    /// ��������͸���ȸı䷽��
    /// </summary>
    public void AlphaChangeEvent(double BeginTime, double FinishTime, string formula,int TargetAlpha)
    {

    }
}
