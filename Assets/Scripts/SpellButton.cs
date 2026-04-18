using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    private int index;
    private bool isVertical;

    public void Init(int index, bool isVertical, float size)
    {
        this.index = index;
        this.isVertical = isVertical;
        GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SpellManager.Instance.castRowColSpell(index, isVertical);
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}