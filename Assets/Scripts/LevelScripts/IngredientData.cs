using UnityEngine;

[System.Serializable]
public class ColorRow
{
    public BlockColors[] columns = new BlockColors[3];
}

[CreateAssetMenu(fileName = "Ingredient", menuName = "Scriptable Objects/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientName;
    public Sprite IngredientImage;
    public Sprite ingredientBlock;
    public Sprite notAvailableIngredientImage;
    public Sprite notAvailableIngredientBlockImage;
    public bool ingredientEnabled;
    
    public ColorRow[] rows = new ColorRow[3];
}