using UnityEngine;

public class SpellBookManager : MonoBehaviour
{
    public void CloseSpellBook()
    {
        GameManager.Instance.ReturnEnteryScene();
    }

}
