using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    [SerializeField] EnemyPooling _enemyPooling;
    [SerializeField] EnemySpawner _enemySpawner;
    [SerializeField] DoorTrigger _doorTrigger;
    public GameObject _player;

    private bool m_gameStatus;

    private int score;
    private int highScore;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        Application.targetFrameRate = 120;
    }

    private void Start()
    {
        score = 0;

        if(PlayerPrefs.HasKey("highscore"))
        {
            highScore = PlayerPrefs.GetInt("highscore");
        }
        else
        {
            highScore = 0;
        }

        PutGameStatus(true);
        UIManager.Instance.StartMenu();
    }

    public void StartGame()
    {
        PutGameStatus(true);
        _enemyPooling.enabled = true;
        _enemySpawner.enabled = true;
        _doorTrigger.GameStartCloseDoor();
    }

    public void PutGameStatus(bool status)
    {
        m_gameStatus = status;
    }

    public bool GetGameStatus()
    {
        return m_gameStatus;
    }

    public void UpdateScore()
    {
        score++;

        if(highScore < score)
        {
            highScore = score;
            PlayerPrefs.SetInt("highscore", highScore);
        }

        UIManager.Instance.UpdateScore(score);
    }
}
