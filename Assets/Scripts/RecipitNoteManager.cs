using UnityEngine;
using UnityEngine.UI;

public class RecipitNoteManager : MonoBehaviour
{
    public Image shapeDisplay;
    public Image ingredientDisplay;

    // This function fills the UI with the data from the ScriptableObject
    public void DisplayRecipe(IngredientData data)
    {
        shapeDisplay.sprite = data.IngredientImage;
        ingredientDisplay.sprite = data.ingredientBlock;
    }
}
