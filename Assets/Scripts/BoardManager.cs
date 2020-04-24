using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int Score { get; private set; }
    void Start()
    {
        _openCards = new Card[2].ToList();
        _cards = new List<Card>();
        m_states = new Dictionary<float, StateItem>();
        _stateGame = StateGame.ready;
        Initialization();
        StartNewGame();
        
    }

    #region region of Coroutine 
    IEnumerator StartGame()
    {
        showBanner = 0;
        while (lettersOfBegin.Length > showBanner )
        {
            BannerText.text = lettersOfBegin[showBanner++];
            yield return new WaitForSeconds(1);
        }
        BannerText.text = string.Empty;
        _stateGame = StateGame.play;
        StartCoroutine(StateExecute());
        StartCoroutine(TimeLeft());
    }

    IEnumerator TimeLeft()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            slider.value = slider.value - 1;
            if ((int)slider.value == 0)
            {
                BannerText.text = "GAME OVER";
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator StateExecute()
    {
        while (true)
        {
            if (m_states.Count > 0)
            {
                foreach (KeyValuePair<float,StateItem> valuePair in m_states)
                {
                    Debug.Log("now " + Time.time + " execute " + valuePair.Key);
                    if (Time.time >= valuePair.Key)
                    {
                        ExecuteState(valuePair.Value);
                        m_states.Remove(valuePair.Key);
                        //todo execute
                        break;
                    }
                }
            }
            yield return null;
        }
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
                _cards.Add(_card);
            }
        }
    }

    public Card CreateCard(Vector2 position)
    {
        GameObject obj = Instantiate(cardPrefabs, position, Quaternion.identity);
        return obj.GetComponent<Card>();
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
        Debug.Log("event OpenCard()");
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
            case StateItem.StateItemType.Hiden:
                _openCards[0].Visible = false;
                _openCards[1].Visible = false;
                _openCards[0] = null;
                _openCards[1] = null;
                FoundPair++;
                Score += 1;
                UpdateScore();
                if (FoundPair == 7)
                {
                    BannerText.text = "WIN";
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

    private void StartNewGame()
    {
        Score = 0;
        StartNewLevel();
    }

    private void StartNewLevel()
    {
        slider.value = 100;
        FoundPair = 0;
        int[] ids = new int[_cards.Count];
        for (int i = 0; i < ids.Length; i++)
        {
            int id = Random.Range(1, 40);
            ids[i] = id;
            ids[++i] = id;
        }
        for (int i = 0; i < ids.Length; i++) {
            int temp = ids[i];
            int randomIndex = Random.Range(i, ids.Length);
            ids[i] = ids[randomIndex];
            ids[randomIndex] = temp;
        }

        int index = 0;
        foreach (Card _card in _cards)
        {
            _card.Visible = true;
            _card.InitCardId(ids[index++]);
        }
        
        StartCoroutine(StartGame());
        UpdateScore();
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