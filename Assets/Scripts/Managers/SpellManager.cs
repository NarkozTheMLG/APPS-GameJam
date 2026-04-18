using UnityEngine;
using System.Collections.Generic;

public enum WizardSpells
{
    BreakSingle,
    Paint,
    RowColumnAttack,
    Create
}

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance; 

    public WizardSpells currentSpell;
    public BlockColors currentColor;

    [SerializeField] private float breakSingleCD = 3.0f;
    [SerializeField] private float paintCD = 0.5f;
    [SerializeField] private float rowColCD = 1.5f;
    [SerializeField] private float createCD = 10f;

    private Dictionary<WizardSpells, float> nextReadyTime = new Dictionary<WizardSpells, float>();

    void Awake()
    {
        Instance = this;
        nextReadyTime[WizardSpells.BreakSingle] = 0;
        nextReadyTime[WizardSpells.Paint] = 0;
        nextReadyTime[WizardSpells.RowColumnAttack] = 0;
        nextReadyTime[WizardSpells.Create] = 0;

    }

    void Start()
    {
        currentSpell = WizardSpells.Paint;
        currentColor = BlockColors.Red;
        UpdateButtonVisibility();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeSpell(WizardSpells.BreakSingle);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeSpell(WizardSpells.Paint);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeSpell(WizardSpells.RowColumnAttack);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeSpell(WizardSpells.Create);
        if (Input.GetKeyDown(KeyCode.Space)) GridManagerSystem.Instance.ThanosSnapAll();
      //  if (Input.GetKeyDown(KeyCode.Alpha5)) GridManagerSystem.Instance.GenerateGrid();
    }

    public void ChangeSpell(WizardSpells newSpell)
    {
        currentSpell = newSpell;
        UpdateButtonVisibility();
    }

    public void UpdateButtonVisibility()
    {
        if (GridManagerSystem.ColumnButtons == null || GridManagerSystem.ColumnButtons.Length == 0 || GridManagerSystem.ColumnButtons == null) return;

        bool showSideButtons = (currentSpell == WizardSpells.RowColumnAttack);

        foreach (var btn in GridManagerSystem.ColumnButtons)
        {
            if (btn != null) btn.SetVisible(showSideButtons);
        }
        foreach (var btn in GridManagerSystem.RowButtons)
        {
            if (btn != null) btn.SetVisible(showSideButtons);
        }
    }

    public void castASpell(int xIndex, int yIndex)
    {
        if (Time.time < nextReadyTime[currentSpell]) return;
        if(currentSpell == WizardSpells.BreakSingle)
        {
            GridManagerSystem.Instance.BreakSingle(xIndex, yIndex);
            nextReadyTime[WizardSpells.BreakSingle] = Time.time + breakSingleCD;
        }
        else if(currentSpell == WizardSpells.Paint)
        {
            GridManagerSystem.Instance.Paint(xIndex, yIndex, currentColor);
            nextReadyTime[WizardSpells.Paint] = Time.time + paintCD;
        }
        else if(currentSpell == WizardSpells.Create)
        {
            if (GridManagerSystem.Grids[xIndex, yIndex].isActive) return;
            createABlock(xIndex, yIndex);
            nextReadyTime[WizardSpells.Create] = Time.time + createCD;
        }
    }

    public void castRowColSpell(int index, bool isVertical)
    {
        if (Time.time < nextReadyTime[WizardSpells.RowColumnAttack]) return;

        if (isVertical)
            GridManagerSystem.Instance.BreakColumn(index);
        else
            GridManagerSystem.Instance.BreakRow(index);

        nextReadyTime[WizardSpells.RowColumnAttack] = Time.time + rowColCD;
    }

    public void createABlock(int xIndex, int yIndex)
    {
        GridManagerSystem.Grids[xIndex, yIndex].RestoreBlock();
    }
}