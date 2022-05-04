using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyed : MonoBehaviour
{
    private void Awake()
    {
        var objs = FindObjectsOfType<DontDestroyed>();
        int count = 0;
        foreach(var obj in objs)
        {
            if (obj.name.Equals(gameObject.name))
            {
                count++;
                if(count > 1)
                {
                    Destroy(gameObject);
                    Debug.Log(obj.name);
                }
                else
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }
    }
}
