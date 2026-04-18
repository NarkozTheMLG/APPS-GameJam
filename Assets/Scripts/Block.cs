using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using System.Collections; 

public enum BlockColors
{
    Red,
    Green,
    Blue,
    White
}

public class Block : MonoBehaviour,IPointerClickHandler
{ [SerializeField] private GameObject burstPrefab; 
    [SerializeField] private Sprite shardSprite;
    [SerializeField] private Image blockImage; 
    
    private BlockColors color;
    public bool isActive;
    public BlockColors GetColor() => color;


    public void SetColor(BlockColors newColor)
    {
        this.color = newColor; 
    
        StopAllCoroutines();
        StartCoroutine(AnimateTransformation(newColor));
    }

    private IEnumerator AnimateTransformation(BlockColors newColor)
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = new Vector3(1.3f, 1.3f, 1.3f); 
        float duration = 0.1f; // Fast and snappy
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rt.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            yield return null;
        }

        ApplyVisualColor(newColor);

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rt.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
            yield return null;
        }

        rt.localScale = originalScale;
    }

    private void ApplyVisualColor(BlockColors newColor)
    {
        switch (newColor)
        {
            case BlockColors.Red: 
                blockImage.color = Color.red; 
                break;
            case BlockColors.Green: 
                blockImage.color = Color.green; 
                break;
            case BlockColors.Blue: 
                blockImage.color = Color.blue; 
                break;
            case BlockColors.White: 
                blockImage.color = Color.white; 
                break;
        }
    }
    public int GridX { get; private set; }
    public int GridY { get; private set; }

    public void Init(int x, int y, float cellSize)
    {
        color = BlockColors.Green;
        SetColor(color);
        isActive = true;
        GridX = x;
        GridY = y;
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isActive)
        {
            if (SpellManager.Instance.currentSpell == WizardSpells.Create)
            {
                SpellManager.Instance.castASpell(GridX, GridY);
            }
            return;
        }
        SpellManager.Instance.castASpell(GridX, GridY);
    }
    

public void BreakBlock() 
{
    if (!isActive) return; 

    isActive = false;
    if (burstPrefab != null)
    {
        GameObject burstObj = Instantiate(burstPrefab, transform.parent);
        burstObj.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        burstObj.GetComponent<UIParticleBurst>().Play(blockImage.color,shardSprite);
    }
    blockImage.color = new Color(0, 0, 0, 0); 
    blockImage.raycastTarget = true; 
}

    public void RestoreBlock() 
    {
        isActive = true;
        blockImage.raycastTarget = true;
    
        SetColor(BlockColors.White); 
    }
    
    
    public void ThanosDisappear()
    {
        isActive = false;
        StopAllCoroutines();
        StartCoroutine(AnimateSnap());
    }

    private IEnumerator AnimateSnap()
    {
        RectTransform rt = GetComponent<RectTransform>();
        float duration = 0.5f;
        float elapsed = 0f;
    
        Vector3 targetScale = new Vector3(1.4f, 1.4f, 1.4f);
    
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (t < 0.2f) 
                rt.localScale = Vector3.Lerp(Vector3.one, targetScale, t / 0.2f);
            else 
                rt.localScale = Vector3.Lerp(targetScale, Vector3.zero, (t - 0.2f) / 0.8f);

            Color c = blockImage.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            blockImage.color = c;

            yield return null;
        }

        rt.localScale = Vector3.zero;
    }
}