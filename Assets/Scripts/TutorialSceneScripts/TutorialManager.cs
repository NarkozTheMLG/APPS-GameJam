using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject darkScreenPanel;
    public GameObject gridDarkPannel; // ADDED: So the grid shade works!
    public GameObject tutorialBubble;
    public TextMeshProUGUI bubbleText;

    [Header("Fallback Settings")]
    // This is just a fallback in case you ever forget to assign an anchor
    public Vector3 bubbleOffset = new Vector3(0f, 50f, 0f);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // UPDATED: Now accepts the grid shade flag and the explicit Anchor location!
    public void StartTutorialStep(GameObject targetElement, string instruction, bool useShade = true, bool useGridShade = false, Transform explicitBubbleLocation = null)
    {
        Time.timeScale = 0f;

        if (darkScreenPanel) darkScreenPanel.SetActive(useShade);
        if (gridDarkPannel && useGridShade) gridDarkPannel.SetActive(useGridShade);

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
            targetCanvas.overrideSorting = useShade || useGridShade;
            targetCanvas.sortingOrder = (useShade || useGridShade) ? 101 : 0;
        }
    }

    public void EndTutorialStep(GameObject targetElement)
    {
        Time.timeScale = 1f;

        if (darkScreenPanel) darkScreenPanel.SetActive(false);
        if (gridDarkPannel) gridDarkPannel.SetActive(false); // Make sure grid shade turns off!
        if (tutorialBubble) tutorialBubble.SetActive(false);

        Canvas targetCanvas = targetElement.GetComponent<Canvas>();
        if (targetCanvas != null)
        {
            targetCanvas.overrideSorting = false;
        }
    }
}