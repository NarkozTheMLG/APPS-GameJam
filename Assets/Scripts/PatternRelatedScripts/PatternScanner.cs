using UnityEngine;

public class PatternScanner : MonoBehaviour
{
    public static PatternScanner Instance;

    void Awake()
    {
        Instance = this;
    }
    public void ScanForMatches()
    {
        LevelGoalManager goals = LevelGoalManager.Instance;

        for (int recipeIndex = 0; recipeIndex < 3; recipeIndex++)
        {
            if (goals.isIngredientComplete[recipeIndex]) continue;

            IngredientData pattern = goals.activeRecipes[recipeIndex];

            for (int x = 0; x <= GridManagerSystem.ROWSIZE - 3; x++)
            {
                for (int y = 0; y <= GridManagerSystem.COLUMNSIZE - 3; y++)
                {
                    if (CheckIndividualMatch(x, y, pattern))
                    {
                        ProcessMatch(x, y, recipeIndex);
                        return; 
                    }
                }
            }
        }
    }
    private bool CheckIndividualMatch(int startX, int startY, IngredientData pattern)
    {
        Debug.Log("CheckIndividualMatch for "+pattern.ingredientName);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Block gridBlock = GridManagerSystem.Grids[startX + i, startY + j];
                BlockColors requiredColor = pattern.rows[j].columns[i];

                if (requiredColor == BlockColors.None)
                {
                    if (gridBlock.isActive) return false;
                }
                else
                {
                    if (!gridBlock.isActive || gridBlock.GetColor() != requiredColor) return false;
                }
            }
        }

        Debug.Log("RETURNED TRUE");
        return true;
    }

    private void ProcessMatch(int startX, int startY, int recipeIndex)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GridManagerSystem.Grids[startX + i, startY + j].BreakBlock();
            }
        }

        LevelGoalManager.Instance.IngredientMatched(recipeIndex);
        SpellManager.Instance.UpdateArrowVisibility();
    }
}