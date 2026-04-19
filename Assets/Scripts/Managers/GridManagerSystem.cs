using UnityEngine;
using UnityEngine.UI;
using System.Collections; 
using System.Collections.Generic;

public class GridManagerSystem : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip paintSound;
    public static GridManagerSystem Instance; 

[Header("VFX")]

public GameObject arrowVFXPrefab; 
[Header("UI References")]
public Canvas mainUICanvas; 
    public const int ROWSIZE = 9;
    public const int COLUMNSIZE = 10;

    [Header("Settings")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private float cellSize = 1000f;
    [SerializeField] private float spacing = 5f;

    public static Block[,] Grids = new Block[ROWSIZE, COLUMNSIZE];
    [SerializeField] private GameObject spellButtonPrefab;
    public static SpellButton[] ColumnButtons = new SpellButton[ROWSIZE];
    public static SpellButton[] RowButtons = new SpellButton[COLUMNSIZE];
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
        
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.25f);
            audioSource.PlayOneShot(clip);    
        }
    }

    
    
    private void SpawnSideButtons(float startX, float startY, float cellSize)
    {
        for (int i = 0; i < ROWSIZE; i++)
        {
            GameObject btn = Instantiate(spellButtonPrefab, transform);
            RectTransform rt = btn.GetComponent<RectTransform>();
    
            rt.anchorMin = rt.anchorMax = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);

            float xPos = startX + (i * cellSize) + (cellSize / 2f);
            float yPos = startY + (COLUMNSIZE * cellSize) + 20f + (cellSize / 2f); 

            rt.anchoredPosition = new Vector2(xPos, yPos);
            rt.sizeDelta = new Vector2(cellSize, cellSize);

            rt.localEulerAngles = new Vector3(0, 0, -90f);

            SpellButton script = btn.GetComponent<SpellButton>();
            script.Init(i, true, cellSize);
            ColumnButtons[i] = script;
        }

        for (int j = 0; j < COLUMNSIZE; j++)
        {
            GameObject btn = Instantiate(spellButtonPrefab, transform);
            RectTransform rt = btn.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = rt.pivot = Vector2.zero;

            float xPos = startX - cellSize - 20f; 
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
            
                rt.pivot = new Vector2(0.5f, 0.5f);

                float xPos = startX + (i * cellSize) + (cellSize / 2f);
                float yPos = startY + (j * cellSize) + (cellSize / 2f);

                rt.anchoredPosition = new Vector2(xPos, yPos);
                rt.sizeDelta = new Vector2(cellSize, cellSize);

                Block blockScript = newObj.GetComponent<Block>();
                blockScript.Init(i, j, cellSize);
                Grids[i, j] = blockScript;
            }
        }
        SpawnSideButtons(startX, startY, cellSize);
        SpellManager.Instance.UpdateArrowVisibility();
    }
    
    public void BreakSingle(int x, int y)
    {
        if (IsInsideGrid(x, y) && Grids[x, y].isActive)
        {
            Grids[x, y].BreakBlock();
            SpellManager.Instance.UpdateArrowVisibility();
        }
    }

    public void BreakRow(int y)
    {
        StartCoroutine(AnimateRowBreak(y));
    }


    public void BreakColumn(int x)
    {
        StartCoroutine(AnimateColumnBreak(x));
    }




private IEnumerator AnimateRowBreak(int y)
{
    GameObject arrow = Instantiate(arrowVFXPrefab, mainUICanvas.transform);


    for (int i = 0; i < ROWSIZE; i++)
    {
        if (IsInsideGrid(i, y))
        {
            arrow.transform.position = Grids[i, y].transform.position;

            if (Grids[i, y].isActive)
            {
                Grids[i, y].BreakBlock();
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    Destroy(arrow);
}
private IEnumerator AnimateColumnBreak(int x)
{
    if (mainUICanvas == null) yield break;

    GameObject arrow = Instantiate(arrowVFXPrefab, mainUICanvas.transform);
    arrow.transform.SetAsLastSibling();
    
    arrow.transform.localRotation = Quaternion.Euler(0, 0, -90); 

    for (int j = COLUMNSIZE - 1; j >= 0; j--)
    {
        if (IsInsideGrid(x, j))
        {
            arrow.transform.position = Grids[x, j].transform.position;

            if (Grids[x, j].isActive)
            {
                Grids[x, j].BreakBlock();
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    Destroy(arrow);
    SpellManager.Instance.UpdateArrowVisibility();
}

    public void Paint(int x, int y, BlockColors newColor)
    {
        if (!IsInsideGrid(x, y) || !Grids[x, y].isActive) return;
            PlaySFX(paintSound);
        StopAllCoroutines();
        StartCoroutine(AnimatedFloodFill(x, y, newColor));
    }

    private IEnumerator AnimatedFloodFill(int x, int y, BlockColors newColor)
    {
        BlockColors targetColor = Grids[x, y].GetColor();
        if (targetColor == newColor) yield break;
        Queue<Vector2Int> nodes = new Queue<Vector2Int>();
        nodes.Enqueue(new Vector2Int(x, y));
            PlaySFX(paintSound);

        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        visited.Add(new Vector2Int(x, y));

        while (nodes.Count > 0)
        {
            Vector2Int current = nodes.Dequeue();
            Block block = Grids[current.x, current.y];
            
            block.SetColor(newColor);

            yield return new WaitForSeconds(0.01f); 

            Vector2Int[] neighbors = {
                new Vector2Int(current.x + 1, current.y),
                new Vector2Int(current.x - 1, current.y),
                new Vector2Int(current.x, current.y + 1),
                new Vector2Int(current.x, current.y - 1)
            };

            foreach (var next in neighbors)
            {
                if (IsInsideGrid(next.x, next.y) && !visited.Contains(next))
                {
                    Block nextBlock = Grids[next.x, next.y];
                    if (nextBlock.isActive && nextBlock.GetColor() == targetColor)
                    {
                        visited.Add(next);
                        nodes.Enqueue(next);
                        SpellManager.Instance.UpdateArrowVisibility();
                    }
                }
            }
        }
    }
    public bool canSnap = true; 
    public void ThanosSnapAll()
    {
        if (!canSnap) return;
        StartCoroutine(SnapSequence());
    }
    private IEnumerator SnapSequence()
    {
        canSnap = false;
        yield return StartCoroutine(SnapSpread(Random.Range(0,ROWSIZE), Random.Range(0,COLUMNSIZE)));
        yield return new WaitForSeconds(0.35f); 
        GenerateGrid();
        yield return new WaitForSeconds(0.25f); 
        canSnap = true;
    }
    private IEnumerator SnapSpread(int startX, int startY)
    {
        Queue<Vector2Int> nodes = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        nodes.Enqueue(new Vector2Int(startX, startY));
        visited.Add(new Vector2Int(startX, startY));

        while (nodes.Count > 0)
        {
            int count = nodes.Count;
        
            for (int i = 0; i < count; i++)
            {
                Vector2Int current = nodes.Dequeue();
                Block block = Grids[current.x, current.y];

                if (block.isActive)
                {
                    block.ThanosDisappear();
                }

                Vector2Int[] neighbors = {
                    new Vector2Int(current.x + 1, current.y),
                    new Vector2Int(current.x - 1, current.y),
                    new Vector2Int(current.x, current.y + 1),
                    new Vector2Int(current.x, current.y - 1)
                };

                foreach (var next in neighbors)
                {
                    if (IsInsideGrid(next.x, next.y) && !visited.Contains(next))
                    {
                        visited.Add(next);
                        nodes.Enqueue(next);
                    }
                }
            }
            yield return new WaitForSeconds(0.05f); 
        }
    }
    
    
    private bool IsInsideGrid(int x, int y)
    {
        return (x >= 0 && x < ROWSIZE && y >= 0 && y < COLUMNSIZE);
    }
    
    
}