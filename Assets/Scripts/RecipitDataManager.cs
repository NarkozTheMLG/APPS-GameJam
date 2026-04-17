using UnityEngine;
using UnityEngine.UI;

public class RecipitDataManager : MonoBehaviour
{
    [Header("Data")]
    public IngredientData[] allRecipes;

    [Header("Note Slots")]
    public RecipitNoteManager[] slots = new RecipitNoteManager[3];
    public Button Button;

    private int currentIndex = 0;

    void Start()
    {
        UpdateRecipeDisplay();
    }

    public void UpdateRecipeDisplay()
    {
        for (int i = 0; i < 3; i++)
        {
            int recipeIndex = currentIndex + i;

            if (recipeIndex < allRecipes.Length)
            {
                slots[i].gameObject.SetActive(true); // Turn the slot on
                slots[i].DisplayRecipe(allRecipes[recipeIndex]); // Feed it the data
            }
            else
            {
                // If there are no more recipes (e.g., we have 7 total and are on the last page)
                slots[i].gameObject.SetActive(false); // Hide the empty slot
            }
        }

        // Optional: Turn off the buttons if you are at the start or end of the list
        Button.interactable = (currentIndex > 0);
    }

    // Link this to your "Next Arrow" UI Button OnClick event
    public void PageForward()
    {
        if (currentIndex + 3 < allRecipes.Length)
        {
            currentIndex += 3;
            UpdateRecipeDisplay();
        }
    }

    // Link this to your "Prev Arrow" UI Button OnClick event
    public void PageBackward()
    {
        if (currentIndex - 3 >= 0)
        {
            currentIndex -= 3;
            UpdateRecipeDisplay();
        }
    }
}

