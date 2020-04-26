using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StateItem
{
    public enum StateItemType
    {
        Open,
        Close,
        Hiden,
        NewLevel,
        Show
    }
    //public Card card;
    public StateItemType stateItemType;
}
public class BoardManager : MonoBehaviour
{
    private enum StateGame
    {
        ready,
        play,
        level_finished,
        gameover
    }
    [SerializeField] private Text BannerText;
    [SerializeField] private Text ScoreField;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject FolderCards;
    [SerializeField] private GameObject cardPrefabs;
    private string[] lettersOfBegin = new string[5]{"Ready","3","2","1", "Go"};
    private int showBanner = 0; // для старта
    private List<Card> _cards;
    private List<Card> _openCards;
    private StateGame _stateGame;
    private Dictionary<float, StateItem> m_states;
    private int FoundPair; // сколько пар найдено 14
    private bool IsTimeLeftWorking = false; 
    private bool IsExecuterWorking = false;
    private int previousLevelTimeLeft;
    private int currentDecreaseTimeLeft = 0;
    public LevelModel currentLevel;
    
    public int Score { get; private set; }
    void Start()
    {
        _openCards = new Card[2].ToList();
        _cards = new List<Card>();
        m_states = new Dictionary<float, StateItem>();
        Initialization();
        
    }

    #region region of Coroutine 
    IEnumerator StartGame()
    {
        showBanner = 0;
        while (lettersOfBegin.Length > showBanner )
        {
            if (lettersOfBegin.Length - 1 == showBanner)
            {
                foreach (Card card in _cards)
                {
                    card.CloseCard();
                }
            }
            BannerText.text = lettersOfBegin[showBanner++];
            yield return new WaitForSeconds(1);
        }
        BannerText.text = string.Empty;
        _stateGame = StateGame.play;
        if (!IsExecuterWorking)
        StartCoroutine(StateExecute());
        if (!IsTimeLeftWorking)
        StartCoroutine(TimeLeft());
    }

    IEnumerator TimeLeft()
    {
        IsTimeLeftWorking = true;
        while (_stateGame != StateGame.gameover)
        {
            if (_stateGame == StateGame.play)
            {
                yield return new WaitForSeconds(1);
                slider.value--;
                if ((int)slider.value == 0)
                {
                    BannerText.text = "GAME OVER";
                    yield return new WaitForSeconds(2);
                    _stateGame = StateGame.gameover;
                    GameManager.instance.EndGame(currentLevel, Score);
                }
            } else if (_stateGame == StateGame.level_finished)
            { // calculation score
                BannerText.text = "You Win";
                slider.value--;
                Score++;
                UpdateScore();
                if ((int) slider.value == 0)
                {
                    _stateGame = StateGame.ready;
                    AddState(3, StateItem.StateItemType.NewLevel);
                }                
            }
            yield return null;
        }
        IsTimeLeftWorking = false;
    }
    IEnumerator StateExecute()
    {
        IsExecuterWorking = true;
        while (_stateGame != StateGame.gameover)
        {
            if (m_states.Count > 0)
            {
                foreach (KeyValuePair<float,StateItem> valuePair in m_states)
                {
                    if (Time.time >= valuePair.Key)
                    {
                        ExecuteState(valuePair.Value);
                        m_states.Remove(valuePair.Key);
                        break;
                    }
                }
            }
            yield return null;
        }
        IsExecuterWorking = false;
    }
    #endregion
    public void Initialization()
    {
        if (FolderCards != null)
        {
            List<GameObject> cards  = FolderCards.GetAllChilds();
            foreach (GameObject card in cards)
            {
                Card _card = card.GetComponent<Card>(); 
                _card.clickCard += (ClickToCard);
                _card.Initialization();                
                _cards.Add(_card);
            }
        }
    }

    private void ClickToCard(Card card)
    {
        if (_stateGame != StateGame.play) return;
        if (_openCards[0] != null && _openCards[1] != null) return;
        
        if (_openCards[0] == null)
        {
            _openCards[0] = card;
        } else if (_openCards[1] == null)
        {
            _openCards[1] = card;
            if (_openCards[1].CardId != _openCards[0].CardId)
                AddState(2.0f, StateItem.StateItemType.Close);
            else
                AddState(1.0f, StateItem.StateItemType.Hiden);
        }
        card.OpenCard();
        
    }

    private void AddState(float offset_seconds, StateItem.StateItemType type)
    {
        m_states.Add(Time.time + offset_seconds, new StateItem
        {
            stateItemType = type
        });
    }

    private void ExecuteState(StateItem item)
    {
        switch (item.stateItemType)
        {
            case StateItem.StateItemType.NewLevel:
                StartNewLevel();
                break;
            case StateItem.StateItemType.Hiden:
                _openCards[0].Visible = false;
                _openCards[1].Visible = false;
                _openCards[0] = null;
                _openCards[1] = null;
                FoundPair++;
                Score += Random.Range(1,4); // randomize prize
                UpdateScore();
                if (FoundPair == 7)
                {
                    previousLevelTimeLeft = (int)slider.value;
                    _stateGame = StateGame.level_finished;
                }
                break;
            case StateItem.StateItemType.Open:
                //item.card.OpenCard();
                break;
            case StateItem.StateItemType.Close:
                _openCards[0].CloseCard();
                _openCards[0] = null;
                _openCards[1].CloseCard();
                _openCards[1] = null;
                break;
        }
    }

    private void UpdateScore()
    {
        ScoreField.text = "  Score:" + Score.ToString();
    }
    
    public void StartNewGame()
    {
        slider.maxValue = currentLevel.timeLeft;
        Score = 0;
        currentDecreaseTimeLeft = -1;
        previousLevelTimeLeft = -1;
        _stateGame = StateGame.ready;
        GameManager.instance.cardPackManager.InitNewSet();
        StartNewLevel();
    }

    private void StartNewLevel()
    {
        if (currentLevel.gameRulesType == GameRulesType.SimleTime)
        {
            currentDecreaseTimeLeft += currentLevel.decreaseTimeLeft;
            slider.value = currentDecreaseTimeLeft > currentLevel.minimumTimeLeft ? 
                currentLevel.minimumTimeLeft : currentLevel.timeLeft - currentDecreaseTimeLeft;
        }
        else
        {
            if (previousLevelTimeLeft > 0)
                slider.value = previousLevelTimeLeft < currentLevel.minimumTimeLeft
                    ? currentLevel.minimumTimeLeft
                    : previousLevelTimeLeft + currentLevel.addTimeLeft;
            else
                slider.value = currentLevel.timeLeft;
            Debug.Log("slider.value " + slider.value);

        }

        
        FoundPair = 0;
        int[] ids = GameManager.instance.cardPackManager.GetSet();
        int index = 0;
        foreach (Card _card in _cards)
        {
            _card.Visible = true;
            _card.InitCardId(ids[index++]);
        }
        StartCoroutine(StartGame());
    }
}
public static class ClassExtension
{
    public static List<GameObject> GetAllChilds(this GameObject Go)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i< Go.transform.childCount; i++)
        {
            list.Add(Go.transform.GetChild(i).gameObject);
        }
        return list;
    }
}