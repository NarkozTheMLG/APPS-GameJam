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
    private string[] stepInstructions = {
        "Select the Break Spell!",
        "Great! Now select the Create Spell!",
        "Powerful! Try the Row & Column Attack!", // New Step
        "Awesome! Let's try the Paint Spell!",
        "Last one! Select the Refresh Spell!"
    };

    private int currentStepIndex = 0;

    void Start()
    {
        stepButtons = new GameObject[] {
            breakSingle,
            create,
            rowColumnAttack, // Inserted here
            paint,
            refresh
        };

        // Lock all buttons initially
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
        TutorialManager.Instance.StartTutorialStep(currentButton, stepInstructions[currentStepIndex]);
    }

    public void OnSpellSelected()
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionToGridStep());
    }

    private IEnumerator TransitionToGridStep()
    {
        isTransitioning = true;

        GameObject currentButton = stepButtons[currentStepIndex];
        TutorialManager.Instance.EndTutorialStep(currentButton);

        yield return new WaitForSecondsRealtime(0.1f);

        // Grid instruction - no shade used here so they can see the blocks!
        TutorialManager.Instance.StartTutorialStep(gridCanvas, "Now click on a block!", false);

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