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
    public Vector3 bubbleOffset = new Vector3(0f, 50f, 0f);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartTutorialStep(GameObject targetElement, string instruction, bool useShade = true, Transform explicitBubbleLocation = null)
    {
        Time.timeScale = 0f;

        if (darkScreenPanel) darkScreenPanel.SetActive(useShade);

        if (tutorialBubble) tutorialBubble.SetActive(true);
        if (bubbleText) bubbleText.text = instruction;

        RectTransform bubbleRect = tutorialBubble.GetComponent<RectTransform>();
        if (bubbleRect != null)
        {
            if (explicitBubbleLocation != null)
            {
                bubbleRect.transform.position = explicitBubbleLocation.position;
            }
            else
            {
                bubbleRect.transform.position = targetElement.transform.position + bubbleOffset;
            }
        }

        Canvas targetCanvas = targetElement.GetComponent<Canvas>();
        if (targetCanvas != null)
        {
            targetCanvas.overrideSorting = useShade;
            targetCanvas.sortingOrder = useShade ? 101 : 0;

            GraphicRaycaster raycaster = targetElement.GetComponent<GraphicRaycaster>();
            if (raycaster == null && useShade)
            {
                targetElement.gameObject.AddComponent<GraphicRaycaster>();
            }
        }
    }

    public void EndTutorialStep(GameObject targetElement)
    {
        Time.timeScale = 1f;

        if (darkScreenPanel) darkScreenPanel.SetActive(false);
        if (tutorialBubble) tutorialBubble.SetActive(false);

        Canvas targetCanvas = targetElement.GetComponent<Canvas>();
        if (targetCanvas != null)
        {
            targetCanvas.overrideSorting = false;
        }
    }
}