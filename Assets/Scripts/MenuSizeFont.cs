using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Resizing font for application
/// </summary>
public class MenuSizeFont : MonoBehaviour
{
    [SerializeField] private GameObject[] textTop;
    void Start()
    {
        foreach (GameObject o in textTop)
        {
            o.GetComponent<Text>().fontSize = Screen.width / 10;    
        }

    }

}
