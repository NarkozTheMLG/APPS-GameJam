using UnityEngine;
using UnityEngine.UI;

public class SpellCooldownUI : MonoBehaviour
{
    [SerializeField] private WizardSpells spellType;
    [SerializeField] private Image cooldownOverlay; 

    void Update()
    {
        float remaining = SpellManager.Instance.GetRemainingCooldown(spellType);
        float total = SpellManager.Instance.GetTotalCooldown(spellType);

        if (remaining > 0)
        {
            cooldownOverlay.enabled = true;
            cooldownOverlay.fillAmount = remaining / total; 
        }
        else
        {
            cooldownOverlay.enabled = false;
        }
    }
}