using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Timer Display")]
    public TextMeshProUGUI TimerText;

    [Header("Live Data")]
    public float timeRemaining;
    public bool isGameActive = false;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        float levelTimeLimit = GameManager.Instance.AllLevelDatas[GameManager.Instance.CurrentLevel - 1].TimeLimit;
        timeRemaining = levelTimeLimit;

        isGameActive = true;
    }

    private void Update()
    {
        if (!isGameActive) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            TriggerGameOver();
        }
    }

    private void UpdateTimerUI()
    {
        if (TimerText != null)
        {
            TimerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        }
    }

    private void TriggerGameOver()
    {
        isGameActive = false;
        Debug.Log("Time Ran Out!");
        
        if (PatternScanner.Instance != null) PatternScanner.Instance.enabled = false;
        
    }
}