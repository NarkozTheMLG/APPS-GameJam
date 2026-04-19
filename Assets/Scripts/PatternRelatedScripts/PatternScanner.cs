using UnityEngine;

public class PatternScanner : MonoBehaviour
{
    
    [Header("Audio Settings")]
    public AudioSource audioSource;

    public AudioClip matchSound;
    
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.25f);
            audioSource.PlayOneShot(clip);    
        }
    }
    public static PatternScanner Instance;
    [Header("VFX Settings")]
    public GameObject flyPrefab; 
    public Canvas uiCanvas;    

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

        Debug.Log("You have matched: "+pattern.ingredientName);
        return true;
    }

 
    private void ProcessMatch(int startX, int startY, int recipeIndex)
    {
        PlaySFX(matchSound);
    
        // 1. Get positions directly from UI
        Vector3 spawnPos = GridManagerSystem.Grids[startX + 1, startY + 1].transform.position;
        Vector3 targetPos = LevelGoalManager.Instance.ingredientIcons[recipeIndex].transform.position;

        // 2. THE MULTI-STEP SPAWN
        // Step A: Create the clone in the scene first (no parent)
        GameObject flyObj = Instantiate(flyPrefab); 
    
        // Step B: Parent the CLONE (flyObj), not the prefab (flyPrefab)
        // We use SetParent(..., false) to keep the UI scale from exploding
        flyObj.transform.SetParent(uiCanvas.transform, false); 

        // 3. Setup the effect
        IngredientFlyEffect effect = flyObj.GetComponent<IngredientFlyEffect>();
        Sprite ingredientSprite = LevelGoalManager.Instance.activeRecipes[recipeIndex].IngredientImage;

        // 4. Grid Logic
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GridManagerSystem.Grids[startX + i, startY + j].BreakBlock();
            }
        }

        // 5. Start the fly
        effect.StartFly(spawnPos, targetPos, ingredientSprite, () => {
            LevelGoalManager.Instance.IngredientMatched(recipeIndex);
            SpellManager.Instance.UpdateArrowVisibility();
        });
    }
}