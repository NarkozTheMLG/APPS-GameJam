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
{
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
        if (!isActive) return;
        SpellManager.Instance.castASpell(GridX, GridY);
    }
    public void BreakBlock() 
    {
        isActive = false;
        blockImage.enabled = false; 
        
        blockImage.raycastTarget = false;

    }

    public void RestoreBlock() 
    {
        isActive = true;
        blockImage.enabled = true;
        blockImage.raycastTarget = true;
        
    }
}