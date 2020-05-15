using System;
using UnityEngine;

public class Card : MonoBehaviour
{

    [SerializeField] private GameObject cardGameObject;
    private Vector2 position;
    private CardTurnOver _cardTurnOver;
    public Action<Card> clickCard;
    private int _cardId;

    public bool Visible
    {
        get
        {
            return gameObject.activeSelf;
        }
        set
        {
            gameObject.SetActive(value);
        }
    }
    public int CardId
    {
        get { return _cardId; }
        private set { _cardId = value; }
    }
    
    public void Initialization()
    {
        _cardTurnOver = GetComponent<CardTurnOver>();
        _cardTurnOver.Init(CardState.Front);
    }
    public void InitCardId(int id)
    {
        if (id > 0 && id <= 40)
        {
            _cardTurnOver.Init(CardState.Front);
            CardId = id;
            cardGameObject.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>("cards/" + id.ToString());
        }        
    }
    /// <summary>
    /// Show front card
    /// </summary>
    public void OpenCard()
    {
        GameManager.instance.MusicManager.OpenCard(0.15f);
        _cardTurnOver.StartFront();
    }
    /// <summary>
    /// Show back card
    /// </summary>
    public void CloseCard()
    {
        GameManager.instance.MusicManager.OpenCard(0.15f);
        _cardTurnOver.StartBack();
    }

    public void HideCard()
    {
        _cardTurnOver.HideCard(() =>
        {
            gameObject.SetActive(false);
        });
    }
    void Update()
    {
        if (GameManager.instance.GameBoard.IsGameOver) return;
        if (Input.GetMouseButtonDown(0) && _cardTurnOver.mCardState == CardState.Back)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null && hit.collider.gameObject.name == gameObject.name)
            {
                clickCard.Invoke(this);
            }
        } else if (Input.touchCount == 1 && _cardTurnOver.mCardState == CardState.Back)
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
