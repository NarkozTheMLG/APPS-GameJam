using UnityEngine;
using UnityEngine.UI;

public class OrderUIManager : MonoBehaviour
{
    public static OrderUIManager Instance;

    public Image FoodImage;
    public Image ingredient1;
    public Image ingredient2;
    public Image ingredient3;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void UpdateDisplay(Sprite foodSprite, IngredientData[] ingredientsNeeded)
    {
        FoodImage.sprite = foodSprite;
        ingredient1.sprite = ingredientsNeeded[0].IngredientImage;
        ingredient2.sprite = ingredientsNeeded[1].IngredientImage;
        ingredient3.sprite = ingredientsNeeded[2].IngredientImage;
    }
}
