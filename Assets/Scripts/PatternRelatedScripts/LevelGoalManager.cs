using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelGoalManager : MonoBehaviour
{
    public static LevelGoalManager Instance;

    [Header("UI Elements")]
    public Image foodDisplayIcon; 
    public TextMeshProUGUI foodProgressText; 

    [Header("Ingredient UI Slots")]
    public Image[] ingredientIcons;
    public Image[] checkmarkStatusUI;
    
    [Header("Status Sprites")]
    public Sprite pendingSprite;   
    public Sprite completedSprite; 

    [HideInInspector] public Recipe currentRecipe;
    [HideInInspector] public IngredientData[] activeRecipes;
    [HideInInspector] public bool[] isIngredientComplete;

    private int recipeIndex = 0; 
    private LevelData currentLevelData;

    private void Awake() => Instance = this;

    private void Start()
    {
        int levelIndex = Mathf.Clamp(GameManager.Instance.CurrentLevel - 1, 0, GameManager.Instance.AllLevelDatas.Length - 1);
        currentLevelData = GameManager.Instance.AllLevelDatas[levelIndex];
        
        LoadCurrentRecipe();
    }

    private void LoadCurrentRecipe()
    {
        if (recipeIndex < currentLevelData.OrderedRecipes.Length)
        {
            currentRecipe = currentLevelData.OrderedRecipes[recipeIndex];
            InitializeRecipeUI();
            UpdateProgressUI();
        }
        else
        {
            Debug.Log("LEVEL COMPLETE!");
        
            if (PatternScanner.Instance != null) 
                PatternScanner.Instance.enabled = false;

            GameManager.Instance.CloseLevelWin(); 
        }
    }

    private void InitializeRecipeUI()
    {
        if (currentRecipe == null) return;

        if (foodDisplayIcon != null) foodDisplayIcon.sprite = currentRecipe.FoodSprite;

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
if (RecipitDataManager.Instance != null)
    {
        RecipitDataManager.Instance.RefreshRequiredIngredients();
    }
    }

    public void IngredientMatched(int index)
    {
        if (index < 0 || index >= isIngredientComplete.Length) return;
        if (isIngredientComplete[index]) return;

        isIngredientComplete[index] = true;
        checkmarkStatusUI[index].sprite = completedSprite;

        if (CheckAllIngredientsDone())
        {
            recipeIndex++;
            LoadCurrentRecipe(); 
        }
    }

    private bool CheckAllIngredientsDone()
    {
        foreach (bool done in isIngredientComplete) if (!done) return false;
        return true;
    }

    private void UpdateProgressUI()
    {
        if (foodProgressText != null)
        {
            foodProgressText.text = $"{recipeIndex}/{currentLevelData.OrderedRecipes.Length}";
        }
    }
}