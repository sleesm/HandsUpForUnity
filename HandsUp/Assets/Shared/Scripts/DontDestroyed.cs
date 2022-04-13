using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyed : MonoBehaviour
{
    private void Awake()
    {
        var obj = FindObjectsOfType<DontDestroyed>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
