using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Scriptable Objects/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientName;
    public Sprite IngredientImage;
    public Sprite ingredientBlock;
    public Sprite notAvailableIngredientImage;
    public Sprite notAvailableIngredientBlockImage;
    public bool ingredientEnabled;
    public bool[] blueprint = new bool[81];
}
