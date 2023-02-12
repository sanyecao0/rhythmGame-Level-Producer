using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tips : MonoBehaviour
{
    private readonly string[] tips = {"1！5！-C_ohm",
    "你看到这条Tips的时候，其实你正在看这条Tips（喜 -Jex_Tg",
    "From Renote import* -DaenerysX",
    "Errorrrrrrrrrrrrrrrrrrrrrrrrr（弹舌）你口水喷我脸上了（气 -Jex_Tg",
    "如果你看到这条tips,那你已被我盯上了（喵 -act音喵",
    "广告位招租 -DaenerysX",
    "冷知识:你看到这条tip时候会感到冷 -act音喵",
    "山鬼:来，我给你拉段三弦 -山鬼",
    "那个…我喜欢你 -DaenerysX",
    "游戏本体come s∞n（不是 -Jex_Tg",
    "冷知识：Jex和dx自设都是异瞳白毛（喜 -Jex_Tg",
    "上海是亚热带季风与湿润气候 -竹锦",
    "举个手，让我看看有几个ut厨 -竹光",
    "你醒辣？世界上除了牛肉拉面以外的食物全都消失辣！-咖喱乌冬",
    "你瞅啥喵？-あいわあP",
    "超绝最可爱あP酱，堂堂降临！-あいわあP",
    "想刀一个策划（？ -白莹莹莹莹",
    "冷知识:ink现实中是一个一个美术牲啊啊啊啊 -ink",
    "前面忘了，中间忘了，后面忘了 -咖喱乌冬",
    "全体目光向我看齐，我是个tips（ -白莹莹莹莹",
    "音游是让你用手指玩的而不是触手 -Fang（玩家）",
    "emm 别人为什么了在8月31内测，问就是你作业写没写完（喜 -Jex_Tg",
    "人们似乎只能说:幸福的生活吧！-C_ohm",
    "我们可以描述的任何东西，也可以是其他样子的。-C_ohm",
    "所有实际情况便是神明。 -C_ohm",
    "只有在'P v Q'有意义时,P。Q才有意义。-C_ohm"};
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
