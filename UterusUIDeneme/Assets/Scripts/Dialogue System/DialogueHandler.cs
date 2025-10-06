using System;
using TMPro;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private Transform[] choiceParents;
    [SerializeField] private GameObject choicePrefab;
    
    [SerializeField] private DialogueFragment initialDialogueFragment;

    public void Awake()
    {
        Init(initialDialogueFragment);
    }

    public void Init(DialogueFragment fragment)
    {
        dialogueText.text = fragment.DialogueText;

        foreach (var currentParents in choiceParents)
        {
            foreach (var currentParent in currentParents.GetComponentsInChildren<Transform>())
                if(currentParent != currentParents)
                    Destroy(currentParent.gameObject);
        }

        foreach (var choice in fragment.Choices)
        {
            int index = fragment.Choices.IndexOf(choice);
            
            if(index > choiceParents.Length - 1) index = choiceParents.Length - 1;
            
            Transform parentTR = choiceParents[index];
            
            GameObject choiceObject = Instantiate(choicePrefab, parentTR);
            
            choiceObject.GetComponentInChildren<TextMeshProUGUI>().text = choice.ChoiceText;
            
            // TODO add accept logic here
            // call Init from this one with the respected DialogueFragment from Choice
            
        }
        
    }


}
