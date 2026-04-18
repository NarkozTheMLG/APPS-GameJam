using UnityEngine;
using UnityEngine.UI;

public class LevelGoalManager : MonoBehaviour
{
    public static LevelGoalManager Instance;

    [Header("The Level Goal")]
    public Recipe currentRecipe;

    [Header("Main UI Elements")]
    public Image foodDisplayIcon; // The big image of the final dish
    
    [Header("Ingredient UI Slots")]
    public Image[] ingredientIcons;      // The 3 icons showing WHAT to make
    public Image[] checkmarkStatusUI;    // The 3 icons showing IF it's done

    [Header("Status Sprites")]
    public Sprite pendingSprite;   // e.g., an empty box or red 'X'
    public Sprite completedSprite; // e.g., a green checkmark

    [HideInInspector] public IngredientData[] activeRecipes;
    [HideInInspector] public bool[] isIngredientComplete;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        if (currentRecipe == null) return;

        if (foodDisplayIcon != null)
            foodDisplayIcon.sprite = currentRecipe.FoodSprite;

        activeRecipes = currentRecipe.ingredientsNeeded;
        isIngredientComplete = new bool[activeRecipes.Length];

        for (int i = 0; i < ingredientIcons.Length; i++)
        {
            if (i < activeRecipes.Length)
            {
                ingredientIcons[i].gameObject.SetActive(true);
                checkmarkStatusUI[i].gameObject.SetActive(true);
                
                ingredientIcons[i].sprite = activeRecipes[i].IngredientImage;
                checkmarkStatusUI[i].sprite = pendingSprite;
            }
            else
            {
                ingredientIcons[i].gameObject.SetActive(false);
                checkmarkStatusUI[i].gameObject.SetActive(false);
            }
        }
    }

    public void IngredientMatched(int index)
    {
        if (index < 0 || index >= isIngredientComplete.Length || index >= checkmarkStatusUI.Length) 
            return;

        if (isIngredientComplete[index]) return;

        isIngredientComplete[index] = true;
        checkmarkStatusUI[index].sprite = completedSprite;

        Debug.Log($"Checkmark updated for: {activeRecipes[index].ingredientName}");

        if (CheckAllDone())
        {
            Debug.Log("LEVEL COMPLETE: " + currentRecipe.recipeName);
        }
    }

    private bool CheckAllDone()
    {
        foreach (bool done in isIngredientComplete) if (!done) return false;
        return true;
    }
}