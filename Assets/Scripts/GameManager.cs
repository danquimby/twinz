using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Type window show
/// </summary>
public enum ShowWindowType
{
    Menu,
    Game,
    ChoiceLevel,
    None
}
/// <summary>
/// Game Manager
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject choiceLevelWindow;
    [SerializeField] private GameObject menuWindow;
    [SerializeField] private GameObject gameWindow;
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
        showWindow(ShowWindowType.ChoiceLevel);
    }
    /// <summary>
    /// Event game over
    /// </summary>
    /// <param name="current_score"> score for currently game</param>
    public void EndGame(LevelModel model, int current_score)
    {
        if (current_score > model.GetTopPerLevel())
            model.SaveTopPerLevel(current_score);
        showWindow(ShowWindowType.Menu);
    }
    /// <summary>
    /// Quit application
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    public void SelectLevel(LevelModel level)
    {
        GameBoard.GetComponent<BoardManager>().currentLevel = level;
        showWindow(ShowWindowType.Game);
    }
    /// <summary>
    /// First initialization of game manager 
    /// </summary>
    private void InitializeManager()
    {
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
                break;
            case ShowWindowType.Menu:
                menuWindow.SetActive(false);
                break;
            case ShowWindowType.ChoiceLevel:
                choiceLevelWindow.SetActive(false);
                break;
        }
        switch (type)
        {
            case ShowWindowType.ChoiceLevel:
                choiceLevelWindow.SetActive(true);
                break;
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