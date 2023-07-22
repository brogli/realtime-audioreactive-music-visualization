using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomTextoverlay : MonoBehaviour
{
    [SerializeField]
    private UIDocument ui;

    [SerializeField]
    List<string> textsToDisplay = new();

    private string _previouslyDisplayedText;

    private Label _randomTextLabel;



    private void Start()
    {

        _previouslyDisplayedText = textsToDisplay[Random.Range(0, textsToDisplay.Count)];
    }
    public void ActivateTextoverlay ()
    {
        ui.enabled = true;

        VisualElement root = ui.rootVisualElement;
        _randomTextLabel = root.Q<Label>("random-text");

        var textsWithoutLastOne = textsToDisplay
            .Where(x => !x.Equals(_previouslyDisplayedText))
            .ToList();
        
        
        int index = Random.Range(0, textsWithoutLastOne.Count);

        _randomTextLabel.text = textsWithoutLastOne[index];
        _previouslyDisplayedText = textsWithoutLastOne[index];
        Debug.Log(_previouslyDisplayedText);

    }

    public void DeactivateTextOverlay()
    {
        ui.enabled = false;
    }
}
