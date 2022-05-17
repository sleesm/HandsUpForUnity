using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public RawImage cardImg;
    private string currentImgByte;

    public void OnClickImgLoad()
    {
        NativeGallery.GetImageFromGallery((file) =>
        {
            FileInfo selected = new FileInfo(file);

            if (selected.Length > 50000000)
            {
                return;
            }

            if (!string.IsNullOrEmpty(file))
            {
                StartCoroutine(LoadingImg(file));
            }
        });
    }

    IEnumerator LoadingImg(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);

        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(fileData);
        cardImg.texture = texture;

        string jpgBase64 = System.Convert.ToBase64String(fileData);
        currentImgByte = jpgBase64;

    }

    public void InitImage()
    {
        cardImg.texture = null;
    }

    public string GetCurrentImgByte()
    {
        return currentImgByte;
    }
}
