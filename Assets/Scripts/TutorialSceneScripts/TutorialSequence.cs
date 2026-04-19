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

    [Header("Anchors")]
    public Transform timerAnchor;
    public Transform customerRatioAnchor;
    public Transform OrderedRecipitAnchor;
    public Transform spellBookAnchor;
    public Transform[] spellAnchors = new Transform[5];
    public Transform gridAnchor;

    [Header("Gameplay Elements")]
    public GameObject gridCanvas;

    [Header("UI Elements")]
    public GameObject timerUI;
    public GameObject CustomerRatio;
    public GameObject OrderedRecipit;
    public GameObject SpellNotes;

    private bool isTransitioning = false;
    private GameObject[] stepButtons;

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
        "Now click on a laser pointer",
        "Now click on a block to paint"
    };

    private int currentStepIndex = 0;

    IEnumerator Start()
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

        // --- THE CINEMATIC INTRO ---

        // Passed: true (main shade), false (grid shade), timerAnchor
        TutorialManager.Instance.StartTutorialStep(timerUI, "Keep an eye on the timer!", true, timerAnchor);
        yield return new WaitForSecondsRealtime(3f);
        TutorialManager.Instance.EndTutorialStep(timerUI);
        yield return new WaitForSecondsRealtime(0.5f);

        // Passed: customerRatioAnchor
        TutorialManager.Instance.StartTutorialStep(CustomerRatio, "You can see the number of waiting customers here.", true, customerRatioAnchor);
        yield return new WaitForSecondsRealtime(3f);
        TutorialManager.Instance.EndTutorialStep(CustomerRatio);
        yield return new WaitForSecondsRealtime(0.5f);

        // Passed: OrderedRecipitAnchor
        TutorialManager.Instance.StartTutorialStep(OrderedRecipit, "The orders shown in here.", true, OrderedRecipitAnchor);
        yield return new WaitForSecondsRealtime(3f);
        TutorialManager.Instance.EndTutorialStep(OrderedRecipit);
        yield return new WaitForSecondsRealtime(0.5f);

        // Passed: spellBookAnchor
        TutorialManager.Instance.StartTutorialStep(SpellNotes, "You can check my spell notes if you forget the ingredient spells!", true,spellBookAnchor);
        yield return new WaitForSecondsRealtime(3f);
        TutorialManager.Instance.EndTutorialStep(SpellNotes);
        yield return new WaitForSecondsRealtime(0.5f);

        // --- END CINEMATIC INTRO ---

        // Start the normal spell tutorial sequence
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

        // Passed: spellAnchors array!
        TutorialManager.Instance.StartTutorialStep(currentButton, spellStepInstructions[currentStepIndex], true,spellAnchors[currentStepIndex]);
    }

    public void OnSpellSelected()
    {
        if (isTransitioning) return;

        // INSTANT LOCK: Prevent double-clicking!
        stepButtons[currentStepIndex].GetComponent<Button>().interactable = false;

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

        TutorialManager.Instance.StartTutorialStep(gridCanvas, BlockStepInstructions[currentStepIndex], false, gridAnchor);

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