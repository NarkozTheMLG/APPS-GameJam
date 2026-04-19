using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FoodVictoryEffect : MonoBehaviour
{
    public float flyDuration = 1.5f;
    public float waitTime = 2.5f;
    public float rotationSpeed = 360f;
    public float targetScale = 4f;
    public AudioClip victorySound;
    private AudioSource audioSource;

    
    public void StartVictorySequence(Vector3 startPos, Sprite foodSprite)
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<Image>().sprite = foodSprite;

        Vector3 centerPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        transform.position = startPos;
        transform.localScale = Vector3.one;

        StartCoroutine(VictoryRoutine(centerPos));
    }

    private IEnumerator VictoryRoutine(Vector3 centerPos)
    {
        float elapsed = 0;
        Vector3 startPos = transform.position;

        if (victorySound != null && audioSource != null)
            audioSource.PlayOneShot(victorySound);

        while (elapsed < flyDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / flyDuration);

            transform.position = Vector3.Lerp(startPos, centerPos, t);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * targetScale, t);
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene("MainMenu");
    }
}