using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class OpenAndSaveLevel: MonoBehaviour
{
        public void openProject()
        {
            FileName openFileName = new FileName();
            openFileName.structSize = Marshal.SizeOf(openFileName);
            openFileName.filter = "relevel文件(*.relevel)\0*.xlsx";
            openFileName.file = new string(new char[256]);
            openFileName.maxFile = openFileName.file.Length;
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
            openFileName.title = "选择文件";
            openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
            if (LocalDialog.GetOpenFileName(openFileName))
            {
                Debug.Log(openFileName.file);
                Debug.Log(openFileName.fileTitle);
            }
        }
    public void saveProject()
    {
        FileName savefileName = new FileName();
        savefileName.structSize = Marshal.SizeOf(savefileName);
        savefileName.filter = "relevel文件(*.relevel)\0*.xlsx";//暂时写成这样
        savefileName.file = new string(new char[256]);
        savefileName.maxFile = savefileName.file.Length;
        savefileName.fileTitle = new string(new char[64]);
        savefileName.maxFileTitle = savefileName.fileTitle.Length;
        savefileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
        savefileName.title = "选择文件";
        savefileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        if (LocalDialog.GetSaveFileName(savefileName))
        {
            Debug.Log(savefileName.file);
            Debug.Log(savefileName.fileTitle);
        }
    }

}
