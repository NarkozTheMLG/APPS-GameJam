using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IngredientFlyEffect : MonoBehaviour
{
    private RectTransform rectTransform;
    private AudioSource audioSource;
    public AudioClip arriveSound; 

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartFly(Vector2 screenPoint, Vector3 targetWorldPos, Canvas canvas, Sprite sprite, System.Action onComplete)
    {
        GetComponent<Image>().sprite = sprite;
        transform.SetAsLastSibling();

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, canvas.worldCamera, out localPoint);
        
        rectTransform.anchoredPosition = localPoint;

        StartCoroutine(FlyRoutine(targetWorldPos, onComplete));
    }

    private IEnumerator FlyRoutine(Vector3 targetWorldPos, System.Action onComplete)
    {
        float duration = 1.5f; 
        float elapsed = 0f;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            transform.position = Vector3.Lerp(startPos, targetWorldPos, t);
            transform.localScale = Vector3.one * (1f + Mathf.Sin(t * Mathf.PI) * 0.4f);
            yield return null;
        }

        if (arriveSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(arriveSound);
            Debug.Log("Playing Arrive Sound");
        }

        GetComponent<Image>().enabled = false;

        onComplete?.Invoke();

        if (arriveSound != null)
        {
            yield return new WaitForSeconds(arriveSound.length);
        }

        Destroy(gameObject);
    }
}