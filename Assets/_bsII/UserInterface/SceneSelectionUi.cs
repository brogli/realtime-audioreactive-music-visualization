using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneSelectionUi : MonoBehaviour, IUserInputsConsumer
{
    public VisualTreeAsset RowTemplate;
    public VisualTreeAsset ButtonContainerTemplate;
    public VisualTreeAsset IsReadyOverlayTemplate;
    public int AmountOfVisibleRows = 3;
    public int AmountOfButtonsPerRow = 3;
    public int RowHeightInPx = 120;

    private Dictionary<Button, int> _buttonToRowNumber = new Dictionary<Button, int>();
    private List<VisualElement> _rows = new List<VisualElement>();
    private LinkedList<Button> _buttons = new LinkedList<Button>();
    private LinkedListNode<Button> _currentlySelectedButtonNode;
    private LinkedListNode<Button> _currentlyPlayingButtonNode;
    private LinkedListNode<Button> _currentlyLoadingButtonNode;
    private int _currentlyVisibleRowsLowerBound = 0;
    private int _amountOfScenesToDisplay;
    private UserInputsModel _userInputsModel;
    private SceneHandler _sceneHandler;
    private TemplateContainer _isReadyOverlay;


    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _sceneHandler = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>();
        SubscribeUserInputs();
        _sceneHandler.EmitSceneIsReadyToActivate += HandleSceneIsReadyToActivate;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentlyLoadingButtonNode != null)
        {
            var currentlyLoadingElement = _currentlyLoadingButtonNode.Value.parent.Q<VisualElement>("is-loading");
            var currentRotation = _currentlyLoadingButtonNode.Value.parent.Q<VisualElement>("is-loading").style.rotate;
            currentlyLoadingElement.style.rotate = new StyleRotate(new Rotate((currentRotation.value.angle.value + Time.deltaTime * 70f) % 365));
        }
    }

    public void InitializeSceneSelectionUi(VisualElement catalogContainer)
    {
        SetupVisibleContent(catalogContainer);
        InitializeCurrentSelection();
    }

    private void SetupVisibleContent(VisualElement catalogContainer)
    {
        SetupButtons(catalogContainer);

        SetupIsReadyOverlay(catalogContainer);
    }

    private void SetupButtons(VisualElement catalogContainer)
    {
        Dictionary<int, Sprite> sprites = LoadSceneThumbnails();

        int amountOfScenesInBuildSettings = SceneManager.sceneCountInBuildSettings;
        HashSet<int> allBuildIndices = Enumerable.Range(0, amountOfScenesInBuildSettings).ToHashSet(); // 2nd arg in .Range is exclusive

        int currentButtonPerRowIndex = 0;
        int currentRowIndex = 0;
        TemplateContainer currentRow = null;
        foreach (var imageIndex in sprites.Keys.ToList())
        {
            if (!allBuildIndices.TryGetValue(imageIndex, out int buildIndex0))
            {
                // skip, since index can't be found in build indices
                continue;
            }

            if (currentButtonPerRowIndex == 0)
            {
                // add row
                currentRow = RowTemplate.Instantiate();
                currentRow.style.flexShrink = 0;
                currentRow.style.flexDirection = FlexDirection.Row;
                currentRow.style.transitionDuration = new List<TimeValue>() { new TimeValue(0.2f) };
                catalogContainer.Add(currentRow);
                _rows.Add(currentRow);
            }

            if (currentButtonPerRowIndex < AmountOfButtonsPerRow)
            {
                // add button
                var buttonContainer = ButtonContainerTemplate.Instantiate();
                buttonContainer.style.flexGrow = 1;
                buttonContainer.style.flexBasis = 0.333f;
                var button = buttonContainer.Q<Button>();
                allBuildIndices.TryGetValue(imageIndex, out int buildIndex);
                button.name = buildIndex.ToString();
                button.style.backgroundImage = new StyleBackground(sprites[imageIndex]);
                button.clicked += delegate () { HandleClick(button.name); };

                currentRow.Add(buttonContainer);
                _buttons.AddLast(button);
                _buttonToRowNumber.Add(button, currentRowIndex);

                currentButtonPerRowIndex++;
            }

            if (currentButtonPerRowIndex >= AmountOfButtonsPerRow)
            {
                currentButtonPerRowIndex = 0;
                currentRowIndex++;
            }
        }
    }

    private static Dictionary<int, Sprite> LoadSceneThumbnails()
    {
        return Resources
            .LoadAll("SceneScreenshots", typeof(Sprite))
            .Select(item => (Sprite)item)
            .OrderBy(item =>
            {
                return int.Parse(item.name).ToString("D3");
            })
            .ToDictionary(item => int.Parse(item.name), item => item);
    }

    private void SetupIsReadyOverlay(VisualElement catalogContainer)
    {
        _isReadyOverlay = IsReadyOverlayTemplate.Instantiate();
        _isReadyOverlay.style.position = Position.Absolute;
        _isReadyOverlay.style.top = 0;
        _isReadyOverlay.style.right = 0;
        _isReadyOverlay.style.bottom = 0;
        _isReadyOverlay.style.left = 0;
        _isReadyOverlay.style.visibility = Visibility.Hidden;
        catalogContainer.Add(_isReadyOverlay);
    }

    private void InitializeCurrentSelection()
    {
        _currentlySelectedButtonNode = _buttons.First;
        _currentlySelectedButtonNode.Value.AddToClassList("button-selected");
        _currentlyPlayingButtonNode = _buttons.First;
        _currentlyPlayingButtonNode.Value.parent.Q<VisualElement>("is-playing").style.visibility = Visibility.Visible;
    }

    private void HandleClick(string x)
    {
        Debug.Log("hello " + x);
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

            // translate all rows
            _rows.ForEach(row =>
            {
                var currentYtranslate = row.style.translate.value.y.value;
                row.style.translate = new Translate(0, -rowsToMove * RowHeightInPx + currentYtranslate);
            });
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


    public void SubscribeUserInputs()
    {
        _userInputsModel.LoadScene.EmitKeyTriggeredEvent += LoadScene;
        _userInputsModel.SelectNextScene.EmitKeyTriggeredEvent += HandleSelectNextScene;
        _userInputsModel.SelectPreviousScene.EmitKeyTriggeredEvent += HandleSelectPreviousScene;
        _userInputsModel.ActivateScene.EmitKeyTriggeredEvent += HandleActivateScene;
        _userInputsModel.SceneScroller.EmitTurnedOnEvent += HandleSceneScrollerOn;
    }

    private void HandleSceneScrollerOn()
    {
        Debug.Log(_userInputsModel.SceneScroller.FaderValue);
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.LoadScene.EmitKeyTriggeredEvent -= LoadScene;
        _userInputsModel.SelectNextScene.EmitKeyTriggeredEvent -= HandleSelectNextScene;
        _userInputsModel.SelectPreviousScene.EmitKeyTriggeredEvent -= HandleSelectPreviousScene;
        _userInputsModel.ActivateScene.EmitKeyTriggeredEvent -= HandleActivateScene;
    }

    private void HandleActivateScene()
    {
        if (_currentlyLoadingButtonNode == null)
        {
            // we haven't loaded a scene to activate
            return;
        }
        _currentlyPlayingButtonNode.Value.parent.Q<VisualElement>("is-playing").style.visibility = Visibility.Hidden;
        _currentlyPlayingButtonNode = _currentlyLoadingButtonNode;
        _currentlyPlayingButtonNode.Value.parent.Q<VisualElement>("is-playing").style.visibility = Visibility.Visible;
        _currentlyLoadingButtonNode.Value.parent.Q<VisualElement>("is-loading").style.visibility = Visibility.Hidden;
        _currentlyLoadingButtonNode = null;
        _isReadyOverlay.style.visibility = Visibility.Hidden;
        _sceneHandler.ActivateScene();
    }
    private void LoadScene()
    {
        if (_currentlyLoadingButtonNode != null)
        {
            // we're already loading a scene
            return;
        }
        _sceneHandler.LoadScene(int.Parse(_currentlySelectedButtonNode.Value.name));
        _currentlyLoadingButtonNode = _currentlySelectedButtonNode;
        _currentlyLoadingButtonNode.Value.parent.Q<VisualElement>("is-loading").style.visibility = Visibility.Visible;
    }

    void OnDisable()
    {
        UnsubscribeUserInputs();
        _sceneHandler.EmitSceneIsReadyToActivate -= HandleSceneIsReadyToActivate;
    }

    private void HandleSceneIsReadyToActivate()
    {
        _isReadyOverlay.style.visibility = Visibility.Visible;
    }
}

