using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnteryPageManager : MonoBehaviour
{
    public static EnteryPageManager Instance { get; private set; }

    [Header("Callender for currentLevel")]
    //public TextMeshProUGUI CurretnLevel;
    public Image CurrentLevelCallender;


    [Header("Hart Objects")]
    public Image[] Harts;
    public Sprite emptyHart;
    public Sprite fullHart;


    void Start()
    {
        // if there is a tuttorial we need to modify the index for the sprite
        int temp = GameManager.Instance.CurrentLevel;
        if (temp <= 0){
            temp = 1;
        }

        CurrentLevelCallender.sprite = GameManager.Instance.AllLevelDatas[temp - 1].LevelTable;
    }

    private void Update()
    {
        int index = 0;
        for (; index < GameManager.Instance.AvailableHart && index < Harts.Length; index++)
        {
            Harts[index].sprite = fullHart;
        }
        for (; index < Harts.Length; index++)
        {
            Harts[index].sprite = emptyHart;
        }
    }

    public void LoadLevel()
    {
        GameManager.Instance.LoadLevel();
    }

    public void LoadSpellBook() {
        GameManager.Instance.LoadSpellBook();
    }
}
