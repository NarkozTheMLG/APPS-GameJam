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

    public void StartFly(Vector3 startPos, Vector3 targetPos, Sprite sprite, System.Action onComplete)
    {
        GetComponent<Image>().sprite = sprite;
        transform.SetAsLastSibling();

        transform.position = new Vector3(startPos.x, startPos.y, 0);
        transform.localScale = Vector3.one;

        StartCoroutine(FlyRoutine(targetPos, onComplete));
    }

    private IEnumerator FlyRoutine(Vector3 targetPos, System.Action onComplete)
    {
        float duration = 1.0f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.localScale = Vector3.one * (1f + Mathf.Sin(t * Mathf.PI) * 0.4f);
            yield return null;
        }

        // --- SOUND LOGIC GOES HERE ---
        if (arriveSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(arriveSound);
        }

        // Hide the image so it looks like it "entered" the icon while the sound finishes
        GetComponent<Image>().enabled = false;

        onComplete?.Invoke();

        // Wait for the sound to finish before deleting the object
        if (arriveSound != null) {
            yield return new WaitForSeconds(arriveSound.length);
        }

        Destroy(gameObject);
    }
}