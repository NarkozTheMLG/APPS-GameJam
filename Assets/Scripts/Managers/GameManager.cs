using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public struct LevelData
{
    public Recipe[] OrderedRecipes;
    public float TimeLimit;
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
    


    [Header("DO NOT TOUCH")]
    public float timeLeftForNextHart;
    

    [Header("Level info")]
    public LevelData[] AllLevelDatas;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CurrentLevel = 1;
            AvailableHart = 3;

            if (WaitTimeForHart <= 0) {
                WaitTimeForHart = 50;
            }

            if (TotalHartNumber <= 0) {
                TotalHartNumber = 3;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IngredientData[] getEnabledIngredients() {
        int counter = 0;
        for (int i = 0; i < allIngredients.Length; i++)
        {
            if (allIngredients[i].ingredientEnabled)
            {
                counter++;
            }
        }

        IngredientData[] result = new IngredientData[counter];

        int index = 0;
        for (int i = 0; i < allIngredients.Length; i++) {
            if (allIngredients[i].ingredientEnabled)
            {
                result[index] = allIngredients[i];
            }
        }
        return result;
    }

    private void Update()
    {
        // there are some emoty harts
        if (AvailableHart < TotalHartNumber) {
            // start the timer for a hart
            if (timeLeftForNextHart <= 0)
            {
                timeLeftForNextHart = WaitTimeForHart;
            }

            // continue the previous timer
            else {
                timeLeftForNextHart = timeLeftForNextHart - Time.deltaTime;
                // now one hart completed its time
                if (timeLeftForNextHart <= 0)
                {
                    AvailableHart++;
                }
            }
        }
    }

    public void LoadLevel()
    {
        // TODO if there is no harts spawn a warning
        if (AvailableHart > 0) {
            if (CurrentLevel > AllLevelDatas.Length)
            {
                // All Levels are done
                CurrentLevel = AllLevelDatas.Length;
            }

            SceneManager.LoadScene("Level_1");
        }

    }

    public void LoadSpellBook() {
        SceneManager.LoadScene("SpellBookScene");
    }

    public void ReturnEnteryScene() {
        SceneManager.LoadScene("GameEntery");
    }

    public void CloseLevelWin()
    {
        CurrentLevel++;
        SceneManager.LoadScene("GameEntery");
    }

    public void CloseLevelLose()
    {
        AvailableHart--;
        if (AvailableHart < 0)
        {
            AvailableHart = 0;
        }

        SceneManager.LoadScene("GameEntery");
    }
}
