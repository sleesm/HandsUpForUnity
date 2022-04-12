using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyed : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
