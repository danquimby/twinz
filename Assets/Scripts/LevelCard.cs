using System;
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

    void OnEnable()
    {
        ScoreText.GetComponent<Text>().text = model.GetTopPerLevel().ToString();
    }
    public void ClickToCard()
    {
        GameManager.instance.SelectLevel(model);
    }
}
