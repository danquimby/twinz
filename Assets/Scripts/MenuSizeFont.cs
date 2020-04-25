using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSizeFont : MonoBehaviour
{
    [SerializeField] private GameObject[] textTop;
    void Start()
    {
        Debug.Log("width " + Screen.width);
        Debug.Log("height " + Screen.height);
        foreach (GameObject o in textTop)
        {
            o.GetComponent<Text>().fontSize = Screen.width / 10;    
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
