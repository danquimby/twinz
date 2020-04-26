using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    [SerializeField] private GameObject ScoreText;
    [SerializeField] private string Tag;
    public Action<LevelCard> clickCard;

    public int Score
    {
        get
        {
            if (!PlayerPrefs.HasKey(tag))
            {
                PlayerPrefs.SetInt(tag, 0);
            }
            return PlayerPrefs.GetInt(tag);
        }
        set
        {
            if (value > Score)
                PlayerPrefs.SetInt(tag, value);
        }
    }

    void Update()
    {
        ScoreText.GetComponent<Text>().text = Score.ToString();       
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null && hit.collider.gameObject.name == gameObject.name)
            {
                clickCard.Invoke(this);
            }
        } else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit);
                if(hit.collider != null && hit.collider.gameObject.name == gameObject.name)
                {
                    clickCard?.Invoke(this);
                }
            }
        }    
    }
}
