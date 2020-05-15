using Assets.Scripts;
using UnityEngine;

/// <summary>
/// Type window show
/// </summary>
public enum ShowWindowType
{
    Menu,
    Game,
    ChoiceLevel,
    Settings,
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
    [SerializeField] private GameObject settingsUiWindow;
    [SerializeField] private GameObject gameUiWindow;
    [SerializeField] private BoardManager GameBoard;
    public MusicManager MusicManager;
    public bool isGame { private set; get; } = false;
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
        isGame = true;
        showWindow(ShowWindowType.ChoiceLevel);
    }

    public void BackToMenu()
    {
        EndGame(null, 0);
    }
    /// <summary>
    /// Event game over
    /// </summary>
    /// <param name="current_score"> score for currently game</param>
    public void EndGame(LevelModel model, int current_score)
    {
        isGame = false;
        MusicManager.TransitionToMenu();
        if (model != null)
        {
            if (current_score > model.GetTopPerLevel())
                model.SaveTopPerLevel(current_score);
        }
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
        GameBoard.currentLevel = level;
        showWindow(ShowWindowType.Game);
    }

    public void CloseSettings()
    {
        if (isGame)
        {
            showWindowType = ShowWindowType.Settings;
            gameWindow.SetActive(true);
            gameUiWindow.SetActive(true);
            settingsUiWindow.SetActive(false);
        }
        else
        {
            showWindow(ShowWindowType.Menu);
        }
    }

    public void OpenSettings()
    {
        showWindow(ShowWindowType.Settings);
    }
    /// <summary>
    /// First initialization of game manager 
    /// </summary>
    private void InitializeManager()
    {
        settingsUiWindow.GetComponent<Settings>().StartValueMusic();
        showWindow(ShowWindowType.Menu);
    }
    /// <summary>
    /// Switcher of windows
    /// </summary>
    /// <param name="type"></param>
    private void showWindow(ShowWindowType type)
    {
        Debug.Log("type " + type);
        int newScore = -1;
        switch (showWindowType)
        {
            case ShowWindowType.Game:
                gameWindow.SetActive(false);
                gameUiWindow.SetActive(false);
                break;
            case ShowWindowType.Menu:
                menuWindow.SetActive(false);
                break;
            case ShowWindowType.ChoiceLevel:
                choiceLevelWindow.SetActive(false);
                break;
            case ShowWindowType.Settings:
                settingsUiWindow.SetActive(false);
                break;
        }
        switch (type)
        {
            case ShowWindowType.ChoiceLevel:
                choiceLevelWindow.SetActive(true);
                break;
            case ShowWindowType.Game:
                gameWindow.SetActive(true);
                gameUiWindow.SetActive(true);
                GameBoard.StartNewGame();
                MusicManager.TransitionToGame();
                break;
            case ShowWindowType.Menu:
                MusicManager.TransitionToMenu();
                menuWindow.SetActive(true);
                break;
            case ShowWindowType.Settings:
                settingsUiWindow.SetActive(true);
                gameUiWindow.SetActive(false);
                gameWindow.SetActive(false);
                break;

        }
        showWindowType = type;
    }
}