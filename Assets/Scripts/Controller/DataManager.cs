using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class DataManager
{   
    public  List<ReceiverMes> NoteData = new List<ReceiverMes>();
    public  List<OtherEvents> OtherEvent = new List<OtherEvents>();
}
public class Data
{
    public static DataManager data=new DataManager();
}

