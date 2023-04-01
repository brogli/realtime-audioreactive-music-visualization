using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UiBehavior : MonoBehaviour
{
    public VisualTreeAsset RowTemplate;
    public VisualTreeAsset ButtonContainerTemplate;
    public int AmountOfVisibleRows = 3;
    public int AmountOfButtonsPerRow = 3;

    // Start is called before the first frame update
    void Start()
    {
        
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
        Dictionary<string, Sprite> sprites = Resources
            .LoadAll("SceneScreenshots", typeof(Sprite))
            .Select(item => (Sprite)item)
            .ToDictionary(item => item.name, item=> item);

        var amountOfScenes = sprites.Count();
        int sceneIndex = 0;
        var amountOfRows = amountOfScenes / AmountOfButtonsPerRow;
        if (amountOfScenes % AmountOfButtonsPerRow != 0)
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
                var button = buttonContainer.Q<Button>();
                button.name = sceneIndex.ToString();
                button.text = sceneIndex.ToString();
                button.style.backgroundImage = new StyleBackground(sprites[sceneIndex.ToString("D2")]);
                button.clicked += delegate () { HandleClick(button.name); };
                row.Add(buttonContainer);
                sceneIndex++;
            }
        }
    }



    private void HandleClick(string x)
    {
        Debug.Log("hello " + x);
    }
}
