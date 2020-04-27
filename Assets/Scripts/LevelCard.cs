using System;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public enum GameRulesType
{
    SimleTime,
    AddedTimeLeft
}
public class LevelCard : MonoBehaviour
{
    [SerializeField] private GameObject ScoreText;
    [SerializeField] private LevelModel model;
    public Action<LevelCard> clickCard;

    void Start()
    {
        List<GameObject> childs = gameObject.GetAllChilds();
        foreach (GameObject child in childs)
        {
            Text _text = child.GetComponent<Text>();
            if (_text != null)
            {
                _text.fontSize = (int) (Screen.width / 11.4);
            }
        }
    }
    void OnEnable()
    {
        ScoreText.GetComponent<Text>().text = model.GetTopPerLevel().ToString();
    }
    public void ClickToCard()
    {
        GameManager.instance.SelectLevel(model);
    }
}
