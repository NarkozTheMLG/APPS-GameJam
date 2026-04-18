using UnityEngine;
using System.Collections;

public class TutorialSequence : MonoBehaviour
{
    [Header("Spell Buttons")]
    public GameObject rowColumnAttack;
    public GameObject create;
    public GameObject paint;
    public GameObject refresh;
    public GameObject breakSingle;

    // NEW: Add a slot for your Grid Canvas so we can highlight it
    [Header("Gameplay Elements")]
    public GameObject gridCanvas;

    private bool isTransitioning = false;

    void Start()
    {
        // 1. Point at the spell button
        TutorialManager.Instance.StartTutorialStep(breakSingle, "Select the Break Spell!");
    }

    // --- TRIGGERED BY THE UI BUTTON ---
    public void OnSpellSelected()
    {
        if (isTransitioning) return;

        // 2. Point at the grid! 
        TutorialManager.Instance.StartTutorialStep(gridCanvas, "Now click on a block to break it!");
    }

    // --- TRIGGERED BY YOUR GRID DETECTOR ---
    public void OnGridActionCompleted()
    {
        if (isTransitioning) return;

        // Start the timer to let the spell animation play
        StartCoroutine(WaitAndShowCreateStep());
    }

    private IEnumerator WaitAndShowCreateStep()
    {
        isTransitioning = true;
        TutorialManager.Instance.EndTutorialStep(gridCanvas);

        // Wait for the spell animation (breaking, painting, or creating)
        yield return new WaitForSeconds(1f);

        TutorialManager.Instance.StartTutorialStep(create, "Great! Now select the Create Spell!");
        isTransitioning = false;
    }
}
