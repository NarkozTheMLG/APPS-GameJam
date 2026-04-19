using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct LevelData
{
    public Recipe[] OrderedRecipes;
    public float TimeLimit;
    public Sprite LevelTable;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public IngredientData[] allIngredients;
    public int CurrentLevel;

    [Header("Hart System")]
    public int TotalHartNumber;
    public float WaitTimeForHart;
    
    [Header("DO NOT TOUCH")]
    public int AvailableHart;
    public float timeLeftForNextHart;

    [Header("Level Info")]
    public LevelData[] AllLevelDatas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CurrentLevel = 1;
            AvailableHart = 3;

            if (WaitTimeForHart <= 0) WaitTimeForHart = 50;
            if (TotalHartNumber <= 0) TotalHartNumber = 3;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IngredientData[] getEnabledIngredients() 
    {
        int counter = 0;
        for (int i = 0; i < allIngredients.Length; i++)
        {
            if (allIngredients[i].ingredientEnabled) counter++;
        }

        IngredientData[] result = new IngredientData[counter];
        int index = 0;
        for (int i = 0; i < allIngredients.Length; i++) 
        {
            if (allIngredients[i].ingredientEnabled)
            {
                result[index] = allIngredients[i];
                index++;
            }
        }
        return result;
    }

    private void Update()
    {
        if (AvailableHart < TotalHartNumber) 
        {
            if (timeLeftForNextHart <= 0)
            {
                timeLeftForNextHart = WaitTimeForHart;
            }
            else 
            {
                timeLeftForNextHart -= Time.deltaTime;
                if (timeLeftForNextHart <= 0) AvailableHart++;
            }
        }
    }

    public void LoadLevel()
    {
        if (AvailableHart > 0)
        {
            if (CurrentLevel > AllLevelDatas.Length) {
                CurrentLevel = AllLevelDatas.Length;
            }

            if (CurrentLevel == 1) {
                SceneManager.LoadScene("Tutorial");
            }
            else
            {
                SceneManager.LoadScene("Level_1");
            }
                
        }
    }

    public void CloseLevelWin()
    {
        CurrentLevel++;
        SceneManager.LoadScene("GameEntery");
    }

    public void CloseLevelLose()
    {
        AvailableHart = Mathf.Max(0, AvailableHart - 1);
        SceneManager.LoadScene("GameEntery");
    }

    public void LoadSpellBook() => SceneManager.LoadScene("SpellBookScene");
    public void ReturnEnteryScene() => SceneManager.LoadScene("GameEntery");
}