using System;
using UnityEngine;

public class BoxUI : MonoBehaviour
{

    [SerializeField]
    private GameFlow gameFlow;

    [SerializeField]
    private BoxHeadwearUI boxHeadwearUI;

    [SerializeField]
    private BoxItemsUI boxItemsUI;

    [SerializeField]
    private BoxInstructionsUI boxInstructionsUI;

    private void Start()
    {
        boxHeadwearUI.OnOptionSelected += OnHeadwearSelected;
        boxItemsUI.OnOptionSelected += OnItemSelected;
        boxInstructionsUI.OnOptionSelected += OnInstructionSelected;
    }


    private void OnHeadwearSelected(CoverOptionUI coverOptionUI, CoverOptionData data)
    {
        gameFlow.ChangeHeadwearOption(data);
    }

    private void OnItemSelected(CoverOptionUI coverOptionUI, CoverOptionData data)
    {
        gameFlow.ChangeItemOption(data);
    }

    private void OnInstructionSelected(CoverOptionUI coverOptionUI, CoverOptionData data)
    {
        gameFlow.ChangeInstructionOption(data);
    }




}
