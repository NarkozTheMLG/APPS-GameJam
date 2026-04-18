using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpellBootSlotDataManager : MonoBehaviour
{
    private IngredientData[] data;

    [Header("Spell Book Slots")]
    public RecipitNoteManager[] slots = new RecipitNoteManager[6];

    private int pageNumber;
    private int currentPage;

    [Header("Buttons")]
    public Button forward;
    public Button backward;

    [Header("Animation Settings")]
    public CanvasGroup pageCanvasGroup;
    public float fadeDuration = 0.2f;

    private bool isAnimating = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        data = GameManager.Instance.allIngredients;

        pageNumber = (data.Length / slots.Length);
        if (data.Length % slots.Length != 0) {
            pageNumber++;
        }
        
        currentPage = 0;
        backward.gameObject.SetActive(false);
        if (pageNumber > 1)
        {
            forward.gameObject.SetActive(true);
        }
        else {
            forward.gameObject.SetActive(false);
        }


        // initally put the first 6 ingredient images
        UpdatePage();
    }

    // Call this from somewhere in the screen
    public void AdvanceSlots()
    {
        if (isAnimating) return;

        if (currentPage < pageNumber - 1) {
            backward.gameObject.SetActive(true);
            currentPage++;
            if (currentPage == pageNumber - 1)
            {
                forward.gameObject.SetActive(false);
            }
            StartCoroutine(TransitionToPage(currentPage));
        }


    }

    // Call this from somewhere in the screen
    public void ReloadSlots()
    {
        if (isAnimating) return;

        if (currentPage == 0) {
            forward.gameObject.SetActive(true);
            currentPage--;
            if (currentPage == 0)
            {
                backward.gameObject.SetActive(false);
            }
            StartCoroutine(TransitionToPage(currentPage));
        } 
    }

    private IEnumerator TransitionToPage(int targetPage)
    {
        isAnimating = true; // Lock the buttons
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Lerp smoothly slides the alpha from 1 (solid) down to 0 (invisible)
            pageCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null; // Wait for the next frame
        }
        pageCanvasGroup.alpha = 0f; // Ensure it is perfectly invisible

        // 2. SWAP THE DATA WHILE IT IS INVISIBLE
        currentPage = targetPage;
        UpdatePage();

        // 3. FADE IN
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Lerp smoothly slides the alpha from 0 (invisible) up to 1 (solid)
            pageCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null; // Wait for the next frame
        }
        pageCanvasGroup.alpha = 1f; // Ensure it is perfectly solid

        isAnimating = false; // Unlock the buttons!
    }

    private void UpdatePage() {
        int index = currentPage * slots.Length;
        for (int i = 0; i < slots.Length; i++)
        {
            UpdateSingleSlot(i, index);
            index++;
        }
    }

    private void UpdateSingleSlot(int slotIndex, int recipeIndex)
    {
        if (data.Length > recipeIndex)
        {
            slots[slotIndex].gameObject.SetActive(true);
            slots[slotIndex].DisplayRecipe(data[recipeIndex]);
        }
        else {
            // there is no ingredient left no mather it is avilable or not
            slots[slotIndex].gameObject.SetActive(false);
        }
        
    }
}
