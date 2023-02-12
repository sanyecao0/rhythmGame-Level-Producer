using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

public class ArtResourceManager : MonoBehaviour
{
    public Image image;
    private void Awake()
    {
        StartCoroutine(GetImgAndOGG());
    }
    IEnumerator GetImgAndOGG()
    {
            TextureToSprite(ReturnImgByte(LevelReadAndWrite.CoverPath));
            yield return new WaitForEndOfFrame();
    }
    private void TextureToSprite(byte[] ImgByte)
    {
        Texture2D texture2D = new Texture2D(1080, 1920);

        texture2D.LoadImage(ImgByte);
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
    }
    private byte[] ReturnImgByte(string CoverPath)
    {
        FileStream fs = new FileStream(CoverPath, FileMode.Open);

        byte[] imgByte = new byte[fs.Length];
        fs.Read(imgByte, 0, imgByte.Length);
        fs.Close();
        return imgByte;
    }
}
