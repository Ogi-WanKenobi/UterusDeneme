using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DialogueFragment", menuName = "iBright/DialogueFragment")]
public class DialogueFragment : ScriptableObject
{
        
        
        [TextArea]
        [SerializeField] private string dialogueText;
        public string DialogueText => dialogueText;
        

        [SerializeField] private List<DialogueChoice> choices = new List<DialogueChoice>(2);
        public List<DialogueChoice> Choices => choices; 
        
        [System.Serializable]
        public class DialogueChoice
        {
                
                [TextArea]
                [SerializeField] private string choiceText;
                public string ChoiceText => choiceText;

                [SerializeField] private DialogueFragment dialogueFragment = null;
                public DialogueFragment DialogueFragment => dialogueFragment;
                
                [SerializeField] private UnityEvent onChoiceSelected = new UnityEvent();
                public UnityEvent OnChoiceSelected => onChoiceSelected;

        }
        
}
