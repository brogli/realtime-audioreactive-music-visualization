using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class UiBehavior : MonoBehaviour, IUserInputsConsumer
{
    public VisualTreeAsset RowTemplate;
    public VisualTreeAsset ButtonContainerTemplate;
    public int AmountOfVisibleRows = 3;
    public int AmountOfButtonsPerRow = 3;
    public StyleSheet buttonSelected;
    public int RowHeightInPx = 120;

    private Dictionary<int, Button> _sceneIndexToButton = new Dictionary<int, Button>();
    private Dictionary<Button, int> _buttonToRowNumber = new Dictionary<Button, int>();
    private LinkedList<Button> _buttons = new LinkedList<Button>();
    private LinkedListNode<Button> _currentlySelectedButtonNode;
    private int _currentlyVisibleRowsLowerBound = 0;
    private int _amountOfScenes;
    private UserInputsModel _userInputsModel;

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        SubscribeUserInputs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;
        VisualElement catalogContainer = root.Q<VisualElement>("catalog-container");
        SetupButtons(catalogContainer);
        InitializeCurrentSelection();




    }

    void OnDisable()
    {
        UnsubscribeUserInputs();
    }

    private void InitializeCurrentSelection()
    {
        _currentlySelectedButtonNode = _buttons.First;
        _currentlySelectedButtonNode.Value.AddToClassList("button-selected");
    }


    private void SetupButtons(VisualElement catalogContainer)
    {
        Dictionary<string, Sprite> sprites = Resources
            .LoadAll("SceneScreenshots", typeof(Sprite))
            .Select(item => (Sprite)item)
            .ToDictionary(item => item.name, item => item);

        _amountOfScenes = sprites.Count();
        int sceneIndex = 0;
        var amountOfRows = _amountOfScenes / AmountOfButtonsPerRow;
        if (_amountOfScenes % AmountOfButtonsPerRow != 0)
        {
            amountOfRows += 1;
        }

        for (int i = 0; i < amountOfRows; i++)
        {
            var row = RowTemplate.Instantiate();
            row.style.flexShrink = 0;
            row.style.flexDirection = FlexDirection.Row;
            catalogContainer.Add(row);

            for (int j = 0; j < AmountOfButtonsPerRow; j++)
            {
                var buttonContainer = ButtonContainerTemplate.Instantiate();
                buttonContainer.style.flexGrow = 1;
                buttonContainer.style.flexBasis = 0.333f;
                var button = buttonContainer.Q<Button>();
                button.name = sceneIndex.ToString();
                button.text = sceneIndex.ToString();
                button.style.backgroundImage = new StyleBackground(sprites[sceneIndex.ToString("D2")]);
                button.clicked += delegate () { HandleClick(button.name); };

                row.Add(buttonContainer);
                _sceneIndexToButton.Add(sceneIndex, button);
                _buttons.AddLast(button);
                _buttonToRowNumber.Add(button, i);

                sceneIndex++;
            }
        }
    }

    private void HandleClick(string x)
    {
        Debug.Log("hello " + x);
    }

    public void SubscribeUserInputs()
    {
        _userInputsModel.SelectNextScene.EmitKeyTriggeredEvent += HandleSelectNextScene;
        _userInputsModel.SelectPreviousScene.EmitKeyTriggeredEvent += HandleSelectPreviousScene;
    }

    private void HandleSelectPreviousScene()
    {
        var doesPreviousExist = _currentlySelectedButtonNode.Previous != null;
        if (!doesPreviousExist)
        {
            return;
        }
        _currentlySelectedButtonNode.Value.RemoveFromClassList("button-selected");
        _currentlySelectedButtonNode = _currentlySelectedButtonNode.Previous;
        _currentlySelectedButtonNode.Value.AddToClassList("button-selected");
        ScrollToButtonIfNeeded(_currentlySelectedButtonNode.Value);
    }

    private void ScrollToButtonIfNeeded(Button button)
    {
        int rowNumberOfButton = _buttonToRowNumber[button];
        int currentlyVisibleRowsUpperBound = _currentlyVisibleRowsLowerBound + AmountOfVisibleRows - 1;
        bool isRowNumberInVisibleRows = rowNumberOfButton >= _currentlyVisibleRowsLowerBound && rowNumberOfButton <= currentlyVisibleRowsUpperBound;
        if (!isRowNumberInVisibleRows)
        {
            int rowsToMove = 0;
            if (rowNumberOfButton > currentlyVisibleRowsUpperBound)
            {
                // move down (visually)
                rowsToMove = rowNumberOfButton - currentlyVisibleRowsUpperBound;
                _currentlyVisibleRowsLowerBound += rowsToMove;
            }
            else
            {
                // move up (visually)
                rowsToMove = rowNumberOfButton - _currentlyVisibleRowsLowerBound;
                _currentlyVisibleRowsLowerBound += rowsToMove; 
            }

            // translate all buttons
            var currentNode = _buttons.First;
            while (currentNode != null)
            {
                var currentYtranslate = currentNode.Value.style.translate.value.y.value;
                currentNode.Value.style.translate = new Translate(0, -rowsToMove * RowHeightInPx + currentYtranslate);
                currentNode = currentNode.Next;
            }
        }
    }

    private void HandleSelectNextScene()
    {
        var doesNextExist = _currentlySelectedButtonNode.Next != null;
        if (!doesNextExist)
        {
            return;
        }
        _currentlySelectedButtonNode.Value.RemoveFromClassList("button-selected");
        _currentlySelectedButtonNode = _currentlySelectedButtonNode.Next;
        _currentlySelectedButtonNode.Value.AddToClassList("button-selected");
        ScrollToButtonIfNeeded(_currentlySelectedButtonNode.Value);
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.SelectNextScene.EmitKeyTriggeredEvent -= HandleSelectNextScene;
        _userInputsModel.SelectPreviousScene.EmitKeyTriggeredEvent -= HandleSelectPreviousScene;
    }
}
