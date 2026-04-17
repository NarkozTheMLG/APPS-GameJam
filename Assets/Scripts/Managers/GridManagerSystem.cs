using UnityEngine;
using UnityEngine.UI; 
public class GridManagerSystem : MonoBehaviour
{
    private const int ROWSIZE = 9;
    private const int COLUMNSIZE = 12;
    
    [Header("UI Grid Settings")]
    [SerializeField] private float cellSize = 100f; 
    [SerializeField] private float spacing = 5f; 
    [SerializeField] private GameObject blockPrefab;

    public static Block[,] Grids = new Block[ROWSIZE, COLUMNSIZE];

    private void GenerateBlocks()
    {
        // 1. Calculate the full width/height of the 9x12 grid
        float totalWidth = (ROWSIZE * cellSize) + ((ROWSIZE - 1) * spacing);
        float totalHeight = (COLUMNSIZE * cellSize) + ((COLUMNSIZE - 1) * spacing);

        // 2. The Bottom-Left starting point (relative to 0,0 center)
        float startX = (-totalWidth / 2f) + (cellSize / 2f);
        float startY = (-totalHeight / 2f) + (cellSize / 2f);

        for (int i = 0; i < ROWSIZE; i++)
        {
            for (int j = 0; j < COLUMNSIZE; j++) // FIXED: Ensure j loop is correct
            {
                // 3. Calculate position for this specific block
                float xPos = startX + (i * (cellSize + spacing));
                float yPos = startY + (j * (cellSize + spacing));

                // 4. Instantiate as child of THIS object
                GameObject newObj = Instantiate(blockPrefab, transform);
                
                RectTransform rt = newObj.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(xPos, yPos);
                rt.sizeDelta = new Vector2(cellSize, cellSize);

                // 5. Store in static array
                Block blockScript = newObj.GetComponent<Block>();
                blockScript.Init(i, j, cellSize); 
                Grids[i, j] = blockScript;
            }
        }
    }

    void Awake() => GenerateBlocks();
}