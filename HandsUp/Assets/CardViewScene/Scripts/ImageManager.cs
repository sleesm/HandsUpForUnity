using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public RawImage cardImg;
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
    }
}
