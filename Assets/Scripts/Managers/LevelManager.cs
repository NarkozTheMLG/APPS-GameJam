using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Timer Display")]
    public TextMeshProUGUI TimerText;

    [Header("Live Data")]
    public float timeRemaining;
    public bool isGameActive = false;
    public Image tableDisplay; 

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        int currentIdx = GameManager.Instance.CurrentLevel;
        
        if (currentIdx >= GameManager.Instance.AllLevelDatas.Length) 
            currentIdx = GameManager.Instance.AllLevelDatas.Length - 1;

        LevelData data = GameManager.Instance.AllLevelDatas[currentIdx];

        timeRemaining = data.TimeLimit;
        isGameActive = true;
        
        if (tableDisplay != null) tableDisplay.sprite = data.LevelTable;


        
        Debug.Log("Timer and Table Set for Level Index: " + currentIdx);
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
        if (TimerText != null) TimerText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }

    private void TriggerGameOver()
    {
        isGameActive = false;
        if (PatternScanner.Instance != null) PatternScanner.Instance.enabled = false;
    }
}