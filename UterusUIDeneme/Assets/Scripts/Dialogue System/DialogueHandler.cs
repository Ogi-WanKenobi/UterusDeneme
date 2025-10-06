using System;
using TMPro;
using UnityEngine;
using Uterus;

public class DialogueHandler : MonoBehaviour
{
    
    [SerializeField] private DialogueFragment currentDialogueFragment;

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
        ClearPreviousDialogues();

        currentDialogueFragment = fragment;
        
        dialogueText.text = fragment.DialogueText;
        
        foreach (var choice in fragment.Choices)
        {
            Transform parentTR = GetParent(fragment, choice);
            
            GameObject choiceObject = Instantiate(choicePrefab, parentTR);
            
            choiceObject.GetComponentInChildren<TextMeshProUGUI>().text = choice.ChoiceText;
            
            DraggablePage draggablePage = choiceObject.GetComponentInChildren<DraggablePage>();
            ChoicePage choicePage = choiceObject.GetComponentInChildren<ChoicePage>();
            
            choicePage.SetChoice(choice);
            draggablePage.OnDragDropped += ChoiceSelected;

        }
        
    }

    private void ClearPreviousDialogues()
    {
        foreach (var currentParents in choiceParents)
        {
            foreach (var currentParent in currentParents.GetComponentsInChildren<Transform>())
                if(currentParent != currentParents)
                    Destroy(currentParent.gameObject);
        }
    }

    private Transform GetParent(DialogueFragment fragment, DialogueFragment.DialogueChoice choice)
    {
        int index = fragment.Choices.IndexOf(choice);
            
        if(index > choiceParents.Length - 1) index = choiceParents.Length - 1;
            
        return choiceParents[index];
    }

    private void ChoiceSelected(DraggablePage draggablePage)
    {
        ChoicePage choicePage = draggablePage.GetComponentInChildren<ChoicePage>();
        Destroy(draggablePage.gameObject);
        
        Init(choicePage.Choice.DialogueFragment);
    }

}
