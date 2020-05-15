using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public enum CardState
{
    None,
    Front,
    Moved,
    Back
}
public class CardTurnOver : MonoBehaviour{
    public GameObject mFront;// card front
    public GameObject mBack;// card back
    public CardState mCardState = CardState.Front;//The current state of the card is front or back?
    public float mTime = 0.3f;
    private bool isActive = false;//true means that the flip is being executed and cannot be interrupted

    /// <summary>
    /// Initialize the card angle, according to mCardState
    /// </summary>
    public void Init(CardState state)
    {
        mCardState = state;
        if(mCardState==CardState.Front)
        {
            //If it starts from the front, rotate the back 90 degrees so that you can't see the back.
            mFront.transform.eulerAngles = Vector3.zero;
            mBack.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            //Start from the back, the same reason
            mFront.transform.eulerAngles = new Vector3(0, 90, 0);
            mBack.transform.eulerAngles = Vector3.zero;
        }
    }

    /// <summary>
    /// Leave the interface called by the outside world
    /// </summary>
    public void StartBack()
    {
        if (isActive)
            return;
        StartCoroutine(ToBack());
    }
    /// <summary>
    /// Leave the interface called by the outside world
    /// </summary>
    public void StartFront()
    {
        if (isActive)
            return;
        StartCoroutine(ToFront());
    }
    public void HideCard(Action finish)
    {
        if (isActive)
            return;
        StartCoroutine(ToHide(finish));
    }
    /// <summary>
    /// flip to the back
    /// </summary>
	IEnumerator ToBack()
    {
        isActive = true;
        mCardState = CardState.Moved;
        mFront.transform.DORotate(new Vector3(0, 90, 0), mTime);
        for (float i = mTime; i >= 0; i -= Time.deltaTime)
            yield return 0;
        mBack.transform.DORotate(new Vector3(0, 0, 0), mTime);
        isActive = false;
        mCardState = CardState.Back;

    }
    /// <summary>
    /// flip to the front
    /// </summary>
    IEnumerator ToFront()
    {
        isActive = true;
        mCardState = CardState.Moved;
        mBack.transform.DORotate(new Vector3(0, 90, 0), mTime);
        for (float i = mTime; i >= 0; i -= Time.deltaTime)
            yield return 0;
        mFront.transform.DORotate(new Vector3(0, 0, 0), mTime);
        isActive = false;
        mCardState = CardState.Front;
    }
    IEnumerator ToHide(Action finish)
    {
        isActive = true;
        mCardState = CardState.Moved;
        mFront.transform.DORotate(new Vector3(0, 90, 0), mTime/2);
        for (float i = mTime; i >= 0; i -= Time.deltaTime)
            yield return 0;
        isActive = false;
        finish?.Invoke();
    }
    
}

