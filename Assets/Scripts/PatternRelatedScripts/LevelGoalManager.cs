using UnityEngine;

public class LevelGoalManager : MonoBehaviour
{
    public static LevelGoalManager Instance;

    [Header("The Level Goal")]
    public Recipe currentRecipe;

    public IngredientData[] activeRecipes;
    public bool[] isIngredientComplete;
    
    [Header("UI Indicator Boxes")]
    public UnityEngine.UI.Image[] uiIndicatorBoxes= new UnityEngine.UI.Image[3];
    public Color pendingColor = Color.red;
    public Color completedColor = Color.green;

    private void Awake()
    {
        Instance = this;
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        if (currentRecipe == null)
        {
            Debug.LogError("No Recipe assigned to LevelGoalManager!");
            return;
        }

        activeRecipes = currentRecipe.ingredientsNeeded;
        isIngredientComplete = new bool[activeRecipes.Length];

        for (int i = 0; i < uiIndicatorBoxes.Length; i++)
        {
            if (i < activeRecipes.Length)
            {
                uiIndicatorBoxes[i].gameObject.SetActive(true);
                uiIndicatorBoxes[i].color = pendingColor;
            }
            else
            {
                uiIndicatorBoxes[i].gameObject.SetActive(false);
            }
        }
    }

    public void IngredientMatched(int index)
    {
        if (isIngredientComplete[index]) return;

        isIngredientComplete[index] = true;
        Debug.Log("INDEX:"+index);
        uiIndicatorBoxes[index].color = completedColor;

        if (CheckAllDone())
        {
            Debug.Log(currentRecipe.recipeName + " is ready!");
        }
    }

    private bool CheckAllDone()
    {
        foreach (bool done in isIngredientComplete) if (!done) return false;
        return true;
    }
}