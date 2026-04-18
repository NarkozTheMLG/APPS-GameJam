using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;


public class RecipitDataManager : MonoBehaviour
{
    private IngredientData[] data;

    [Header("Note Slots")]
    public RecipitNoteManager[] slots = new RecipitNoteManager[3];

    private int currentIndex1 = 0;
    private int currentIndex2 = 1;
    private int currentIndex3 = 2;

    void Start()
    {
        // show just the enabled ingredients
        data = GameManager.Instance.getEnabledIngredients();

        if (data.Length > 0) UpdateSingleSlot(0, currentIndex1);
        if (data.Length > 1) UpdateSingleSlot(1, currentIndex2);
        if (data.Length > 2) UpdateSingleSlot(2, currentIndex3);
    }

    public void AdvanceSlot1()
    {
        if (data.Length == 0) return;

        currentIndex1 = (currentIndex1 + 1);
        if (currentIndex1 >= data.Length) {
            currentIndex1 = currentIndex1 - data.Length;
        }
        UpdateSingleSlot(0, currentIndex1);
    }

    // Link this to Button 2's OnClick event in the Inspector
    public void AdvanceSlot2()
    {
        if (data.Length == 0) return;

        currentIndex2 = (currentIndex2 + 1);
        if (currentIndex2 >= data.Length)
        {
            currentIndex2 = currentIndex2 - data.Length;
        }
        UpdateSingleSlot(1, currentIndex2);
    }

    // Link this to Button 3's OnClick event in the Inspector
    public void AdvanceSlot3()
    {
        if (data.Length == 0) return;

        currentIndex3 = (currentIndex3 + 1);
        if (currentIndex3 >= data.Length)
        {
            currentIndex3 = currentIndex3 - data.Length;
        }

        UpdateSingleSlot(2, currentIndex3);
    }

    // Helper function to update just one specific slot on the screen
    private void UpdateSingleSlot(int slotIndex, int recipeIndex)
    {
        slots[slotIndex].gameObject.SetActive(true);
        slots[slotIndex].DisplayRecipe(data[recipeIndex]);
    }
}

