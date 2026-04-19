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

    [Header("Bubble Settings")]
    // Tweak these numbers in the Inspector! Positive X is right, Positive Y is up.
    public Vector3 bubbleOffset = new Vector3(50f, 50f, 0f);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartTutorialStep(GameObject targetElement, string instruction, bool useShade = true)
    {
        Time.timeScale = 0f;


        if (darkScreenPanel) darkScreenPanel.SetActive(useShade);
        if (tutorialBubble) tutorialBubble.SetActive(true);
        if (bubbleText) bubbleText.text = instruction;

        // --- NEW: CENTER THE BUBBLE ---
        RectTransform bubbleRect = tutorialBubble.GetComponent<RectTransform>();
        if (bubbleRect != null)
        {
            /*
            // 1. Ensure anchors are centered (optional but recommended to do in code)
            bubbleRect.anchorMin = new Vector2(0.5f, 0.5f);
            bubbleRect.anchorMax = new Vector2(0.5f, 0.5f);
            bubbleRect.pivot = new Vector2(0.5f, 0.5f);

            // 2. Set position to (0,0) relative to the center
            bubbleRect.anchoredPosition = Vector2.zero;
            */
            bubbleRect.transform.position = targetElement.transform.position + bubbleOffset;
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
