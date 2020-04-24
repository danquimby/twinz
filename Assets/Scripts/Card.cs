using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Card : MonoBehaviour
{
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
    public CardState cardState
    {
        get
        {
            if (_cardTurnOver != null)
            {
                return _cardTurnOver.mCardState;
            }
            Debug.LogError("ошиибка инициализации ");
            return CardState.None;
        }
    }

    public void InitCardId(int id)
    {
        if (id > 0 && id <= 40)
        {
            CardId = id;
            try
            {
                _cardTurnOver.mFront.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite =
                    Resources.Load<Sprite>("cards/" + id.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }        
    }
    void Start()
    {
        _cardTurnOver = GetComponent<CardTurnOver>();
        _cardTurnOver.Init(CardState.Back);
    }

    public void OpenCard()
    {
        
        Debug.Log("OpenCard() " );
        _cardTurnOver.StartFront();
    }

    public void CloseCard()
    {
        Debug.Log("OpenCard()");
        _cardTurnOver.StartBack();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _cardTurnOver.mCardState == CardState.Back)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null && hit.collider.gameObject.name == gameObject.name)
            {
                clickCard.Invoke(this);
            }
        }else
        if (Input.touchCount == 1 && _cardTurnOver.mCardState == CardState.Back)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit);
                if(hit.collider != null && hit.collider.gameObject.name == gameObject.name)
                {
                    Debug.Log ("Target Position: " + hit.collider.gameObject.name);
                }
                // TODO begin
                Debug.Log("touch begin");
                clickCard?.Invoke(this);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                // TODO end
                Debug.Log("touch end");
            }
        }    
    }
}
