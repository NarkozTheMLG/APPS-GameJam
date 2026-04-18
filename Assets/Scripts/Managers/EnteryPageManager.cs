using UnityEngine;
using TMPro;

public class EnteryPageManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Callender for currentLevel")]
    public TextMeshProUGUI CurretnLevel;


    void Start()
    {
        CurretnLevel.text = "" + GameManager.Instance.CurrentLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
