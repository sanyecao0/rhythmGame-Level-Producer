using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tips : MonoBehaviour
{
    private readonly string[] tips = {"1��5��-C_ohm",
    "�㿴������Tips��ʱ����ʵ�����ڿ�����Tips��ϲ -Jex_Tg",
    "From Renote import* -DaenerysX",
    "Errorrrrrrrrrrrrrrrrrrrrrrrrr�����ࣩ���ˮ���������ˣ��� -Jex_Tg",
    "����㿴������tips,�����ѱ��Ҷ����ˣ��� -act����",
    "���λ���� -DaenerysX",
    "��֪ʶ:�㿴������tipʱ���е��� -act����",
    "ɽ��:�����Ҹ����������� -ɽ��",
    "�Ǹ�����ϲ���� -DaenerysX",
    "��Ϸ����come s��n������ -Jex_Tg",
    "��֪ʶ��Jex��dx���趼����ͫ��ë��ϲ -Jex_Tg",
    "�Ϻ������ȴ�������ʪ������ -���",
    "�ٸ��֣����ҿ����м���ut�� -���",
    "�������������ϳ���ţ�����������ʳ��ȫ����ʧ����-����ڶ�",
    "���ɶ����-�����濫P",
    "������ɰ���P�������ý��٣�-�����濫P",
    "�뵶һ���߻����� -��ӨӨӨӨ",
    "��֪ʶ:ink��ʵ����һ��һ���������������� -ink",
    "ǰ�����ˣ��м����ˣ��������� -����ڶ�",
    "ȫ��Ŀ�����ҿ��룬���Ǹ�tips�� -��ӨӨӨӨ",
    "��������������ָ��Ķ����Ǵ��� -Fang����ң�",
    "emm ����Ϊʲô����8��31�ڲ⣬�ʾ�������ҵдûд�꣨ϲ -Jex_Tg",
    "�����ƺ�ֻ��˵:�Ҹ�������ɣ�-C_ohm",
    "���ǿ����������κζ�����Ҳ�������������ӵġ�-C_ohm",
    "����ʵ��������������� -C_ohm",
    "ֻ����'P v Q'������ʱ,P��Q�������塣-C_ohm"};
    int r;
    public GameObject tipMessage;
    private void Awake()
    {
        GetTips();
        //Debug.Log(tips.Length);
    }
    public void GetTips()
    {
        r = Random.Range(0, tips.Length);
        (tipMessage).GetComponent<TMP_Text>().text = tips[r];
    }
}
