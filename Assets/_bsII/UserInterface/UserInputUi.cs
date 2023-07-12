using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UserInputUi : MonoBehaviour
{
    private Label _clockOffsetValue;
    private UserInputsModel _userInputsModel;

    // Start is called before the first frame update
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;
        _clockOffsetValue = root.Q<Label>("clock-offset-value");

        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
    }

    // Update is called once per frame
    void Update()
    {
        _clockOffsetValue.text = _userInputsModel.BeatClockOffset.FaderValueNormalizedBetweenMinusAndPlusPointFive.ToString("0.0");
    }
}
