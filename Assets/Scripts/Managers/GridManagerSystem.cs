using UnityEngine;
using UnityEngine.UI;

public class GridManagerSystem : MonoBehaviour
{
    
    public static GridManagerSystem Instance; 


    
    private const int ROWSIZE = 9;
    private const int COLUMNSIZE = 12;

    [Header("Settings")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private float cellSize = 1000f;
    [SerializeField] private float spacing = 5f;

    public static Block[,] Grids = new Block[ROWSIZE, COLUMNSIZE];

    private void Awake()
    {
        Instance = this;
        GenerateGrid();
    }
    [SerializeField] private GameObject spellButtonPrefab;
    public static SpellButton[] ColumnButtons = new SpellButton[ROWSIZE];
    public static SpellButton[] RowButtons = new SpellButton[COLUMNSIZE];
    private void SpawnSideButtons(float startX, float startY, float cellSize)
    {
        // Spawn Top Buttons (Columns)
        for (int i = 0; i < ROWSIZE; i++)
        {
            GameObject btn = Instantiate(spellButtonPrefab, transform);
            RectTransform rt = btn.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = rt.pivot = Vector2.zero;

            float xPos = startX + (i * cellSize);
            float yPos = startY + (COLUMNSIZE * cellSize) + 20f; // 20px above grid

            rt.anchoredPosition = new Vector2(xPos, yPos);
            rt.sizeDelta = new Vector2(cellSize, cellSize);

            SpellButton script = btn.GetComponent<SpellButton>();
            script.Init(i, true, cellSize);
            ColumnButtons[i] = script;
        }

        // Spawn Left Buttons (Rows)
        for (int j = 0; j < COLUMNSIZE; j++)
        {
            GameObject btn = Instantiate(spellButtonPrefab, transform);
            RectTransform rt = btn.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = rt.pivot = Vector2.zero;

            float xPos = startX - cellSize - 20f; // 20px to the left
            float yPos = startY + (j * cellSize);

            rt.anchoredPosition = new Vector2(xPos, yPos);
            rt.sizeDelta = new Vector2(cellSize, cellSize);

            SpellButton script = btn.GetComponent<SpellButton>();
            script.Init(j, false, cellSize);
            RowButtons[j] = script;
        }
    }
    private void GenerateGrid()
    {
        float totalScreenWidth = 1080f;
        float margin = 160f;
        float availableWidth = totalScreenWidth - (margin * 2f); 
        float cellSize = availableWidth / ROWSIZE; 
        float startX = margin; 
        float totalHeight = COLUMNSIZE * cellSize;
        float startY = (1920f - totalHeight) / 2f; 

        for (int i = 0; i < ROWSIZE; i++)
        {
            for (int j = 0; j < COLUMNSIZE; j++)
            {
                GameObject newObj = Instantiate(blockPrefab, transform);
                RectTransform rt = newObj.GetComponent<RectTransform>();

                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.zero;
                rt.pivot = Vector2.zero;

                float xPos = startX + (i * cellSize);
                float yPos = startY + (j * cellSize);

                rt.anchoredPosition = new Vector2(xPos, yPos);
                rt.sizeDelta = new Vector2(cellSize, cellSize);

                Block blockScript = newObj.GetComponent<Block>();
                blockScript.Init(i, j, cellSize);
                Grids[i, j] = blockScript;
            }
        }
        SpawnSideButtons(startX, startY, cellSize);
    }
    
    public void BreakSingle(int x, int y)
    {
        if (IsInsideGrid(x, y) && Grids[x, y].isActive)
        {
            Grids[x, y].BreakBlock();
        }
    }

    public void BreakRow(int y)
    {
        for (int i = 0; i < ROWSIZE; i++)
        {
            if (IsInsideGrid(i, y) && Grids[i, y].isActive)
                Grids[i, y].BreakBlock();
        }
    }

    public void BreakColumn(int x)
    {
        for (int j = 0; j < COLUMNSIZE; j++)
        {
            if (IsInsideGrid(x, j) && Grids[x, j].isActive)
                Grids[x, j].BreakBlock();
        }
    }

    public void Paint(int x, int y, BlockColors newColor)
    {
        if (!IsInsideGrid(x, y) || !Grids[x, y].isActive) return;
        FloodFill(x, y, newColor);
    }

    private void FloodFill(int x, int y, BlockColors newColor)
    {
        Debug.Log("Flood Fill");
        if (!IsInsideGrid(x, y)) return;

        Block block = Grids[x, y];

        if (!block.isActive || block.GetColor() == newColor) return;
        
        block.SetColor(newColor);
        Debug.Log("Painting block at: " + x + "," + y);

        FloodFill(x + 1, y, newColor);
        FloodFill(x - 1, y, newColor);
        FloodFill(x, y + 1, newColor);
        FloodFill(x, y - 1, newColor);
    }

    private bool IsInsideGrid(int x, int y)
    {
        return (x >= 0 && x < ROWSIZE && y >= 0 && y < COLUMNSIZE);
    }
    
    
}