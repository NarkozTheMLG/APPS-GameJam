using UnityEngine;

public class TutorialSequence : MonoBehaviour
{
    [Header("Spell Buttons")]
    public GameObject rowColumnAttack;
    public GameObject create;
    public GameObject paint;
    public GameObject refresh;
    public GameObject breakSingle;

    //TODO
    [Header("points to press")]
    public GameObject point1;

    // Call this when the level starts
    void Start()
    {
        // Start Step 1
        TutorialManager.Instance.StartTutorialStep(breakSingle, "Click here to break a block!");
    }

    // Call this from the Real Move Button's OnClick event
    public void OnPlayerMoved()
    {
        TutorialManager.Instance.EndTutorialStep(breakSingle);

        // Start Step 2 immediately (or you can wait for a trigger)
        TutorialManager.Instance.StartTutorialStep(create, "Click here to create a block!");
    }

    //TODO CONTINUE
}
