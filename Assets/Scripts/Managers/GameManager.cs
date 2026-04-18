using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [Header("Timer Display")]
    public TextMeshProUGUI TimerText;

    [Header("Customer Ratio Display")]
    public TextMeshProUGUI CustomerRatio;



    [Header("All Ingredients for Ingredient Notes")]
    public IngredientData[] allIngredients;

    [Header("Level Settings")]
    public Recipe[] OrderedRecepies;
    public float levelTimeLimit = 300f; // 5 minutes in seconds

    [Header("Live Data (Don't touch in Inspector)")]
    public int CurrentOrderIndex = 0;
    public int currentIngredientIndex = 0; // Tracks which of the 3 ingredients the player is carving
    public float timeRemaining;
    public bool isGameActive = false;

    private void Awake()
    {
        // Set up the Singleton safely
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        timeRemaining = levelTimeLimit;
        CurrentOrderIndex = 0;
        currentIngredientIndex = 0;
        isGameActive = true;

        LoadCustomerOrder();
    }

    private void Update()
    {
        if (!isGameActive) return;

        // Count down the global timer
        timeRemaining -= Time.deltaTime;


        float currentTimer = GameManager.Instance.timeRemaining;
        int displaySeconds = Mathf.CeilToInt(currentTimer);
        TimerText.text = "Time Left: " + displaySeconds.ToString();

        CustomerRatio.text = "" + (int) CurrentOrderIndex / OrderedRecepies.Length + " / " + OrderedRecepies.Length;

        if (timeRemaining <= 0)
        {
            TriggerGameOver();
        }
    }

    public void LoadCustomerOrder()
    {
        if (CurrentOrderIndex >= OrderedRecepies.Length)
        {
            TriggerWin();
            return;
        }

        Recipe currentFood = OrderedRecepies[CurrentOrderIndex];
        //IngredientData currentTarget = currentFood.ingredientsNeeded[currentIngredientIndex];

        // TODO: Tell the UI to show this food
        if (OrderUIManager.Instance != null)
        {
            OrderUIManager.Instance.UpdateDisplay(currentFood.FoodSprite, currentFood.ingredientsNeeded);
        }

        // TODO: Tell the GridManager to check against this specific ingredient shape
    }

    // The Grid Manager will call this function when the player successfully carves a shape!
    public void OnShapeCarvedSuccessfully()
    {

        //TODO FIX THIS PART

        /*
        currentIngredientIndex++;

        // Did we finish all 3 ingredients for this food?
        if (currentIngredientIndex >= 3)
        {
            Debug.Log("Meal complete! Next customer coming up.");
            CurrentOrderIndex++;
            currentIngredientIndex = 0; // Reset back to the first ingredient
            LoadCustomerOrder();
        }
        else
        {
            Debug.Log("Ingredient done! Moving to ingredient " + (currentIngredientIndex + 1));
            LoadCustomerOrder();
        }
        */
    }

    private void TriggerGameOver()
    {
        // TODO TRANSECXT TO MAIN PAGE
        isGameActive = false;
        Debug.Log("TIME IS UP! GAME OVER.");
    }

    private void TriggerWin()
    {
        // TODO TRANSECXT TO MAIN PAGE
        isGameActive = false;
        Debug.Log("LEVEL COMPLETE! All customers served.");
    }
}
