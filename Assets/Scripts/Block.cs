using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;

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
    
        switch (newColor)
        {
            case BlockColors.Red:
                blockImage.color = UnityEngine.Color.red;
                break;
            case BlockColors.Green:
                blockImage.color = UnityEngine.Color.green;
                break;
            case BlockColors.Blue:
                blockImage.color = UnityEngine.Color.blue;
                break;
            case BlockColors.White:
                blockImage.color = UnityEngine.Color.white;
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