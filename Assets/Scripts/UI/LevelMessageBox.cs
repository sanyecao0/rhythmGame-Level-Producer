using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMessageBox : MonoBehaviour
{
    public InputField TrackName;
    public InputField Artist;
    public InputField BPM;
    public InputField LevelDesign;
    public InputField BasicBPM;
    public InputField illustrator;
    public GameObject LevelMesPanel;
    private void Start()
    {
        if (LevelReadAndWrite.SetLevelMessageBox)
        {
            Artist.text = Data.levelMessage.Artist;
            BasicBPM.text = Data.levelMessage.BasicBPM;
            BPM.text = Data.levelMessage.BPM;
            TrackName.text = Data.levelMessage.TrackName;
            illustrator.text = Data.levelMessage.illustrator;
            LevelMesPanel.SetActive(true);
        }
    }
}

