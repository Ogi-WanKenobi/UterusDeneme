using UnityEngine;

public class ChoicePage : MonoBehaviour
{
    
        
    [SerializeField] private DialogueFragment.DialogueChoice choice;
    public DialogueFragment.DialogueChoice Choice => choice;
    public void SetChoice(DialogueFragment.DialogueChoice choice) => this.choice = choice;

    
    
}
