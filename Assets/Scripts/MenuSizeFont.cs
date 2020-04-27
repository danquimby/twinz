using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
class MenuSizeFontItem
{
    public GameObject gameObject;
    public float coefficient;
}

/// <summary>
/// Resizing font for application
/// </summary>
public class MenuSizeFont : MonoBehaviour
{
    [SerializeField] private MenuSizeFontItem[] Items;

    void Start()
    {
        foreach (MenuSizeFontItem item in Items)
        {
            if (item.gameObject == null) continue;
            List<GameObject> objects = item.gameObject.GetAllChilds();
            if (objects.Count > 0)
            {
                foreach (GameObject o in objects)
                {
                    Text text = o.GetComponent<Text>();
                    if (text != null)
                    {
                        text.fontSize = (int) (Screen.width / item.coefficient);
                    }
                }
            }
            else
            {
                Text text = item.gameObject.GetComponent<Text>();
                if (text != null)
                {
                    text.fontSize = (int) (Screen.width / item.coefficient);
                }
            }
        }
    }
}