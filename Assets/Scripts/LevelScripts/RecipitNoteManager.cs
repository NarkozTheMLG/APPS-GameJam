using UnityEngine;
using UnityEngine.UI;

public class RecipitNoteManager : MonoBehaviour
{
    public Image shapeDisplay;
    public Image ingredientDisplay;

    // This function fills the UI with the data from the ScriptableObject
    public void DisplayRecipe(IngredientData data)
    {
        if (data.ingredientEnabled)
        {
            shapeDisplay.sprite = data.IngredientImage;
            ingredientDisplay.sprite = data.ingredientBlock;
        }
        else {
            shapeDisplay.sprite = data.notAvailableIngredientImage;
            ingredientDisplay.sprite = data.notAvailableIngredientBlockImage;
        }
    }
}
