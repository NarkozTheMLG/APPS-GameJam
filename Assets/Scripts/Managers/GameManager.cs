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
    // TODO: Hartlar için süre sistemi eklenecek.
    public int Harts;

    [Header("Level info")]
    public LevelData[] AllLevelDatas;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CurrentLevel = 1;
            Harts = 3;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel()
    {
        if (CurrentLevel > AllLevelDatas.Length)
        {
            // All Levels are done
            CurrentLevel = AllLevelDatas.Length - 1;
        }

        SceneManager.LoadScene("LevelScene");
    }

    public void CloseLevelWin() {
        CurrentLevel++;
        SceneManager.LoadScene("GameEntery");
    }

    public void CloseLevelLose()
    {
        Harts--;
        if (Harts < 0) { 
            Harts = 0;
        }

        // TODO: Hartlar için süre sistemi eklenecek.
        
        SceneManager.LoadScene("GameEntery");
    }
}
