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


    [Header("Fallback Settings")]
    // Tweak these numbers in the Inspector! Positive X is right, Positive Y is up.
    public Vector3 bubbleOffset = new Vector3(0f, 50f, 0f);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // REMOVED useGridShade. Now it only takes the target, text, shade bool, and anchor!
    public void StartTutorialStep(GameObject targetElement, string instruction, bool useShade = true, Transform explicitBubbleLocation = null)
    {
        Time.timeScale = 0f;

        if (darkScreenPanel) darkScreenPanel.SetActive(useShade);

        if (tutorialBubble) tutorialBubble.SetActive(true);
        if (bubbleText) bubbleText.text = instruction;

        // --- POSITION THE BUBBLE ---
        RectTransform bubbleRect = tutorialBubble.GetComponent<RectTransform>();
        if (bubbleRect != null)
        {
            // IF we provided a specific anchor, go exactly there!
            if (explicitBubbleLocation != null)
            {
                bubbleRect.transform.position = explicitBubbleLocation.position;
            }
            // OTHERWISE, use the old math fallback
            else
            {
                bubbleRect.transform.position = targetElement.transform.position + bubbleOffset;
            }
        }

        // --- HANDLE HIGHLIGHTING ---
        Canvas targetCanvas = targetElement.GetComponent<Canvas>();
        if (targetCanvas != null)
        {
            targetCanvas.overrideSorting = useShade;
            targetCanvas.sortingOrder = useShade ? 101 : 0;
        }
    }

    public void EndTutorialStep(GameObject targetElement)
    {
        // Unfreeze Time
        Time.timeScale = 1f;

        // Hide tutorial UI
        if (darkScreenPanel) darkScreenPanel.SetActive(false);
        if (tutorialBubble) tutorialBubble.SetActive(false);

        // Push the target element back behind the dark screen
        Canvas targetCanvas = targetElement.GetComponent<Canvas>();
        if (targetCanvas != null)
        {
            targetCanvas.overrideSorting = false;
        }
    }
}