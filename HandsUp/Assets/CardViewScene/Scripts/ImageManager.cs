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
            if(!string.IsNullOrEmpty(file))
            {
                FileInfo selected = new FileInfo(file);

                if (selected.Length > 50000000)
                {
                    return;
                }

                //StartCoroutine(LoadingImg(file));
                
                if (!string.IsNullOrEmpty(file))
                {
                    StartCoroutine(LoadingImg(file));
                }
            }
            
        });
    }

    IEnumerator LoadingImg(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);

        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(fileData);

        if (GameObject.Find("Canvas").transform.Find("CustomPage").gameObject.activeSelf == true)
            cardImg = GameObject.Find("Canvas").transform.Find("CustomPage/CustomCardPage/CardImg").GetComponent<RawImage>();
        else if (GameObject.Find("Canvas").transform.Find("PopUpPages/EditCardPopUp").gameObject.activeSelf == true)
            cardImg = GameObject.Find("Canvas").transform.Find("PopUpPages/EditCardPopUp/CardImg").GetComponent<RawImage>();

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
