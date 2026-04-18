using UnityEngine;
using UnityEngine.UI;

public class SpellUIHandler : MonoBehaviour
{
    [Header("Settings")]
    public Color selectedColor = Color.yellow;
    public Color normalColor = Color.white;
    
    [Header("References")]
    public GameObject paintSubMenu; 
    private Button[] allButtons;

    void Awake()
    {
        allButtons = GetComponentsInChildren<Button>(true);
    }

    public void SelectSpell(Button clickedButton)
    {
        SpellManager.Instance.UpdateArrowVisibility();
        foreach (Button btn in allButtons)
        {
            btn.image.color = normalColor;
        }
        clickedButton.image.color = selectedColor;

        if (System.Enum.TryParse(clickedButton.name, out BlockColors pickedColor))
        {
            SpellManager.Instance.currentColor = pickedColor;
            SpellManager.Instance.ChangeSpell(WizardSpells.Paint);
        
            paintSubMenu.SetActive(true);
        }
        else if (clickedButton.name == "Paint")
        {
            paintSubMenu.SetActive(!paintSubMenu.activeSelf);
            SpellManager.Instance.ChangeSpell(WizardSpells.Paint);
        }
        else 
        {
            paintSubMenu.SetActive(false);
        
            if (System.Enum.TryParse(clickedButton.name, out WizardSpells spell))
            {
                SpellManager.Instance.ChangeSpell(spell);
                SpellManager.Instance.UpdateArrowVisibility();

            }
        }
    }
}