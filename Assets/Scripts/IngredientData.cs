using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Scriptable Objects/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientName;
    public Sprite IngredientImage;
    public Sprite ingredientBlock;
    public bool[] blueprint = new bool[81];
}
