using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnteryPageManager : MonoBehaviour
{
    public static EnteryPageManager Instance { get; private set; }

    [Header("Callender for currentLevel")]
    public TextMeshProUGUI CurretnLevel;


    [Header("Hart Objects")]
    public Image[] Harts;
    public Sprite emptyHart;
    public Sprite fullHart;


    void Start()
    {
        CurretnLevel.text = "" + GameManager.Instance.CurrentLevel;
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
}
