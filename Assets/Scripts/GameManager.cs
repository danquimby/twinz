using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShowWindowType
{
    Menu,
    Game,
    None
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menuWindow;
    [SerializeField] private GameObject gameWindow;
    [SerializeField] private Text topScoreText;
    [SerializeField] private GameObject GameBoard;
    private int topScore;
    public static GameManager instance = null;
    public ShowWindowType showWindowType { get; private set; } = ShowWindowType.None;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        InitializeManager();
    }

    public void StartGame()
    {
        showWindow(ShowWindowType.Game);
    }

    public void EndGame(int current_score)
    {
        if (current_score > topScore)
        {
            topScore = current_score;
            PlayerPrefs.SetInt("topScore", topScore);
        }
        topScoreText.text = "Top Score: " + topScore;
        PlayerPrefs.GetInt("topScore");
        showWindow(ShowWindowType.Menu);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private void InitializeManager()
    {
        if (PlayerPrefs.HasKey("topScore"))
        {
            topScore = PlayerPrefs.GetInt("topScore");
        }
        else
        {
            topScore = 1;
            PlayerPrefs.SetInt("topScore", topScore);
        }
        topScoreText.text = "Top Score: " + topScore;
        showWindow(ShowWindowType.Menu);
    }
    private void showWindow(ShowWindowType type)
    {
        Debug.Log("showWindowType " + showWindowType);
        int newScore = -1;
        switch (showWindowType)
        {
            case ShowWindowType.Game:
                gameWindow.SetActive(false);
                newScore = GameBoard.GetComponent<BoardManager>().Score;
                break;
            case ShowWindowType.Menu:
                menuWindow.SetActive(false);
                break;
        }
        switch (type)
        {
            case ShowWindowType.Game:
                gameWindow.SetActive(true);
                GameBoard.GetComponent<BoardManager>().StartNewGame();
                break;
            case ShowWindowType.Menu:
                menuWindow.SetActive(true);
                break;
        }
        showWindowType = type;

    }
    
}