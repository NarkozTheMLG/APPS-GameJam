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
    private Dictionary<WizardSpells, float> totalCooldownDurations = new Dictionary<WizardSpells, float>();

    void Awake()
    {
        Instance = this;
        
        foreach (WizardSpells spell in System.Enum.GetValues(typeof(WizardSpells)))
        {
            nextReadyTime[spell] = 0;
            totalCooldownDurations[spell] = 1.0f; 
        }
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
    }

    public void ChangeSpell(WizardSpells newSpell)
    {
        currentSpell = newSpell;
        UpdateButtonVisibility();
    }

    public void UpdateButtonVisibility()
    {
        if (GridManagerSystem.ColumnButtons == null || GridManagerSystem.ColumnButtons.Length == 0) return;

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
        if (Time.time < nextReadyTime.GetValueOrDefault(currentSpell, 0)) return;

        if(currentSpell == WizardSpells.BreakSingle)
        {
            GridManagerSystem.Instance.BreakSingle(xIndex, yIndex);
            RecordCooldown(WizardSpells.BreakSingle, breakSingleCD);
        }
        else if(currentSpell == WizardSpells.Paint)
        {
            if (GridManagerSystem.Grids[xIndex,yIndex].GetColor() == currentColor) return;
            GridManagerSystem.Instance.Paint(xIndex, yIndex, currentColor);
            RecordCooldown(WizardSpells.Paint, paintCD); 
        }
        else if(currentSpell == WizardSpells.Create)
        {
            if (GridManagerSystem.Grids[xIndex, yIndex].isActive) return;
            createABlock(xIndex, yIndex);
            RecordCooldown(WizardSpells.Create, createCD); 
        }
    }

    public void castRowColSpell(int index, bool isVertical)
    {
        if (Time.time < nextReadyTime.GetValueOrDefault(WizardSpells.RowColumnAttack, 0)) return;

        if (isVertical)
        {
            bool shoot=false;
            for (int i = 0; i < GridManagerSystem.ROWSIZE; i++)
                if (GridManagerSystem.Grids[index, i].isActive)
                {
                    shoot = true;
                    break;
                }
            if (!shoot) return;
            GridManagerSystem.Instance.BreakColumn(index);
        }
        else
        {
            bool shoot=false;
            Debug.Log(index);
            for (int i = 0; i < GridManagerSystem.COLUMNSIZE; i++)
                if (GridManagerSystem.Grids[i, index].isActive)
                {
                    shoot = true;
                    break;
                }
            if (!shoot) return;
            GridManagerSystem.Instance.BreakRow(index);
        }

        RecordCooldown(WizardSpells.RowColumnAttack, rowColCD);
        UpdateArrowVisibility();
    }

    public void createABlock(int xIndex, int yIndex)
    {
        GridManagerSystem.Grids[xIndex, yIndex].RestoreBlock();
    }
    
    // --- UI HELPERS ---

    public float GetRemainingCooldown(WizardSpells spell)
    {
        if (!nextReadyTime.ContainsKey(spell)) return 0;
        float remaining = nextReadyTime[spell] - Time.time;
        return Mathf.Max(0, remaining);
    }

    private void RecordCooldown(WizardSpells spell, float duration)
    {
        nextReadyTime[spell] = Time.time + duration;
        totalCooldownDurations[spell] = duration; 
    }

    public float GetTotalCooldown(WizardSpells spell)
    {
        if (totalCooldownDurations.ContainsKey(spell)) 
            return totalCooldownDurations[spell];
    
        return 1.0f; 
    }
    
    public void UpdateArrowVisibility()
    {
        if (GridManagerSystem.ColumnButtons == null || GridManagerSystem.RowButtons == null) return;

        bool isAttackMode = (currentSpell == WizardSpells.RowColumnAttack);

        for (int i = 0; i < GridManagerSystem.ROWSIZE; i++)
        {
            bool hasActiveBlock = false;
            for (int j = 0; j < GridManagerSystem.COLUMNSIZE; j++)
            {
                if (GridManagerSystem.Grids[i, j].isActive)
                {
                    hasActiveBlock = true;
                    break;
                }
            }
        
            if (GridManagerSystem.ColumnButtons[i] != null)
            {
                GridManagerSystem.ColumnButtons[i].SetVisible(isAttackMode && hasActiveBlock);
            }
        }

        for (int j = 0; j < GridManagerSystem.COLUMNSIZE; j++)
        {
            bool hasActiveBlock = false;
            for (int i = 0; i < GridManagerSystem.ROWSIZE; i++)
            {
                if (GridManagerSystem.Grids[i, j].isActive)
                {
                    hasActiveBlock = true;
                    break;
                }
            }

            if (GridManagerSystem.RowButtons[j] != null)
            {
                GridManagerSystem.RowButtons[j].SetVisible(isAttackMode && hasActiveBlock);
            }
        }
    }
}