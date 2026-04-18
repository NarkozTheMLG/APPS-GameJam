using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject darkScreenPanel;
    public GameObject tutorialBubble;
    public TextMeshProUGUI bubbleText;

    private void Awake()
    {
        // If there is no instance yet, make this the instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // If another TutorialManager somehow exists, destroy this duplicate
            Destroy(gameObject);
        }
    }

    // Call this when a tutorial step starts
    public void StartTutorialStep(GameObject targetElement, string instruction)
    {
        Time.timeScale = 0f;
        darkScreenPanel.SetActive(true);
        tutorialBubble.SetActive(true);
        bubbleText.text = instruction;

        // Move bubble near the target (You may need to offset this so it doesn't cover the button!)
        tutorialBubble.transform.position = targetElement.transform.position + new Vector3(0, 100, 0);

        // 4. Pop the target element through the dark screen
        Canvas targetCanvas = targetElement.GetComponent<Canvas>();
        if (targetCanvas == null)
        {
            targetCanvas = targetElement.AddComponent<Canvas>();
            targetElement.AddComponent<GraphicRaycaster>(); // Needed for clicking
        }

        targetCanvas.overrideSorting = true;
        targetCanvas.sortingOrder = 101;
    }

    // Call this via an Event or Button OnClick when the player does the right thing
    public void EndTutorialStep(GameObject targetElement)
    {
        // 1. Unfreeze Time
        Time.timeScale = 1f;

        // 2. Hide tutorial UI
        darkScreenPanel.SetActive(false);
        tutorialBubble.SetActive(false);

        // 3. Push the target element back behind the dark screen
        if (targetElement.GetComponent<Canvas>() != null)
        {
            targetElement.GetComponent<Canvas>().overrideSorting = false;
        }
    }
}
