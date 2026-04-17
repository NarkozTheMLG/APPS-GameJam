using UnityEngine;

enum Color
{
    Red,
    Green,
    Blue,
    White
}

public class Block : MonoBehaviour
{
     
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D blockCollider;
    private Color color;
    private bool isActive;
    
    
    public void Init(int xPos, int yPos, float cellSize)
    {
        isActive = true;
        
        transform.localPosition = new Vector3(xPos * cellSize, yPos * cellSize, 0);
    }
   

    public void BreakBlock() {
        isActive = false;
        spriteRenderer.enabled = false; 
        if (blockCollider != null) blockCollider.enabled = false;
        
    }
    public void RestoreBlock() {
        isActive = true;
        spriteRenderer.enabled = true;
        if (blockCollider != null) blockCollider.enabled = true;
    }
    
}
