using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance { get; private set; }

    [Header("Timer Display")]
    public TextMeshProUGUI TimerText;

    [Header("Customer Ratio Display")]
    public TextMeshProUGUI CustomerRatio;

    [Header("Level Settings")]
    private Recipe[] OrderedRecepies;
    private float levelTimeLimit;

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
        CurrentOrderIndex = 0;
        currentIngredientIndex = 0;
        // load the orders for the given 
        OrderedRecepies = GameManager.Instance.AllLevelDatas[GameManager.Instance.CurrentLevel - 1].OrderedRecipes;
        // load the level time limit
        levelTimeLimit = GameManager.Instance.AllLevelDatas[GameManager.Instance.CurrentLevel - 1].TimeLimit;
        timeRemaining = levelTimeLimit;

        isGameActive = true;
        LoadCustomerOrder();
    }


    private void Update()
    {
        if (!isGameActive) return;

        // Count down the global timer
        timeRemaining -= Time.deltaTime;


        float currentTimer = LevelManager.Instance.timeRemaining;
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

        //Tell the UI to show this food
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

        isGameActive = false;
        GameManager.Instance.CloseLevelLose();
    }

    private void TriggerWin()
    {

        isGameActive = false;
        GameManager.Instance.CloseLevelWin();
    }
}
