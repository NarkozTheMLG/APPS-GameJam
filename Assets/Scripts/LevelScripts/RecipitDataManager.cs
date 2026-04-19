using UnityEngine;
using UnityEngine.UI;

public class RecipitDataManager : MonoBehaviour
{
    public static RecipitDataManager Instance; 

    private IngredientData[] data;

    [Header("Note Slots")]
    public RecipitNoteManager[] slots = new RecipitNoteManager[3]; 

    private int currentIndex1 = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RefreshRequiredIngredients();
    }

    public void RefreshRequiredIngredients()
    {
        if (LevelGoalManager.Instance == null) return;

        data = LevelGoalManager.Instance.activeRecipes;

        if (data != null && data.Length > 0)
        {
            currentIndex1 = 0;
            UpdateSingleSlot(0, currentIndex1);
        }
    }

    public void AdvanceSlot1()
    {
        if (data == null || data.Length == 0) return;

        currentIndex1 = (currentIndex1 + 1) % data.Length;
        UpdateSingleSlot(0, currentIndex1);
    }

    private void UpdateSingleSlot(int slotIndex, int recipeIndex)
    {
        if (slots[slotIndex] != null)
        {
            slots[slotIndex].gameObject.SetActive(true);
            slots[slotIndex].DisplayRecipe(data[recipeIndex]);
        }
    }
}