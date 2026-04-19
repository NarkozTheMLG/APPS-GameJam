using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialSequence : MonoBehaviour
{
    [Header("Spell Buttons")]
    public GameObject rowColumnAttack;
    public GameObject create;
    public GameObject paint;
    public GameObject refresh;
    public GameObject breakSingle;

    [Header("Gameplay Elements")]
    public GameObject gridCanvas;

    private bool isTransitioning = false;
    private GameObject[] stepButtons;

    // --- UPDATED: Added the RowColumn instruction before Paint ---
    private string[] spellStepInstructions = {
        "Select the Break Spell!",
        "Great! Now select the Create Spell!",
        "Powerful! Try the Row & Column Attack!",
        "Awesome! Let's try the Paint Spell and Choose a color!",
        "Last one! Select the Refresh Spell!"
    };

    private string[] BlockStepInstructions = {
        "Now click on a block!",
        "Now click on the empty place",
        "Now clik on a lazer pointer",
        "Now click on a block to paint"
    };

    private int currentStepIndex = 0;

    void Start()
    {
        stepButtons = new GameObject[] {
            breakSingle,
            create,
            rowColumnAttack,
            paint,
            refresh
        };

        foreach (GameObject btn in stepButtons)
        {
            if (btn != null) btn.GetComponent<Button>().interactable = false;
        }

        // Start the first step
        StartCurrentStep();
    }

    private void StartCurrentStep()
    {
        if (currentStepIndex >= stepButtons.Length)
        {
            FinishTutorial();
            return;
        }

        GameObject currentButton = stepButtons[currentStepIndex];
        currentButton.GetComponent<Button>().interactable = true;

        // This will now use the center-screen logic we set up in TutorialManager
        TutorialManager.Instance.StartTutorialStep(currentButton, spellStepInstructions[currentStepIndex]);
    }

    public void OnSpellSelected()
    {
        if (isTransitioning) return;

        if (currentStepIndex == stepButtons.Length - 1)
        {
            StartCoroutine(FinishTutorialSequence());
        }
        else
        {
            StartCoroutine(TransitionToGridStep());
        }
    }

    private IEnumerator FinishTutorialSequence()
    {
        isTransitioning = true;

        GameObject currentButton = stepButtons[currentStepIndex];
        TutorialManager.Instance.EndTutorialStep(currentButton);

        // Give it a tiny fraction of a second to look smooth
        yield return new WaitForSecondsRealtime(0.1f);

        // Call your existing finish method
        FinishTutorial();
    }

    private IEnumerator TransitionToGridStep()
    {
        isTransitioning = true;

        GameObject currentButton = stepButtons[currentStepIndex];
        TutorialManager.Instance.EndTutorialStep(currentButton);

        yield return new WaitForSecondsRealtime(0.1f);

        TutorialManager.Instance.StartTutorialStep(gridCanvas, BlockStepInstructions[currentStepIndex], false);


        isTransitioning = false;
    }

    public void OnGridActionCompleted()
    {
        if (isTransitioning) return;
        StartCoroutine(WaitAndShowNextStep());
    }

    private IEnumerator WaitAndShowNextStep()
    {
        isTransitioning = true;
        TutorialManager.Instance.EndTutorialStep(gridCanvas);

        yield return new WaitForSecondsRealtime(0.1f);

        currentStepIndex++;
        StartCurrentStep();

        isTransitioning = false;
    }

    public void FinishTutorial()
    {
        foreach (GameObject btn in stepButtons)
        {
            if (btn != null) btn.GetComponent<Button>().interactable = true;
        }
        Destroy(gameObject);
    }
}