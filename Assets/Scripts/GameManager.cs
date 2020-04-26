using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Type window show
/// </summary>
public enum ShowWindowType
{
    Menu,
    Game,
    None
}
/// <summary>
/// Game Manager
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menuWindow;
    [SerializeField] private GameObject gameWindow;
    [SerializeField] private Text topScoreText;
    [SerializeField] private GameObject GameBoard;
    private int topScore;
    private CardPackManager _cardPackManager;
    public static GameManager instance = null;
    public ShowWindowType showWindowType { get; private set; } = ShowWindowType.None;

    public CardPackManager cardPackManager
    {
        get
        {
            if (_cardPackManager == null)
                _cardPackManager = GetComponent<CardPackManager>();
            return _cardPackManager;
        }
    }

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
    /// <summary>
    /// Event start new game
    /// </summary>
    public void StartGame()
    {
        showWindow(ShowWindowType.Game);
    }
    /// <summary>
    /// Event game over
    /// </summary>
    /// <param name="current_score"> score for currently game</param>
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
    /// <summary>
    /// Quit application
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
    /// <summary>
    /// First initialization of game manager 
    /// </summary>
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
    /// <summary>
    /// Switcher of windows
    /// </summary>
    /// <param name="type"></param>
    private void showWindow(ShowWindowType type)
    {
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