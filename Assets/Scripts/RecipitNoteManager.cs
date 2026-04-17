using UnityEngine;
using UnityEngine.UI;

public class RecipitNoteManager : MonoBehaviour
{
    public Image shapeDisplay;
    public Image ingredientDisplay;

    // This function fills the UI with the data from the ScriptableObject
    public void DisplayRecipe(RepitNote data)
    {
        shapeDisplay.sprite = data.gridShapeImage;
        ingredientDisplay.sprite = data.realisticImage;
    }
}
