using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UiBehavior : MonoBehaviour
{
    public VisualTreeAsset RowTemplate;
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

        VisualElement mainContainre = root.Q<VisualElement>("catalog-container");



        for (int i = 0; i < 4; i++)
        {
            var blah = RowTemplate.Instantiate();
            blah.style.flexShrink = 0;

            mainContainre.Add(blah);
            var button0 = blah.Q<Button>("button0");
            button0.clicked += delegate () { HandleClick(button0.name); };
            var button1 = blah.Q<Button>("button1");
            button1.clicked += delegate () { HandleClick(button1.name); };
            var button2 = blah.Q<Button>("button2");
            button2.clicked += delegate () { HandleClick(button2.name); };
        }


        var sprite = Resources.Load<Sprite>("SceneScreenshots/image01");
        Debug.Log(sprite);
        //bla2h.Q<Button>("element0").style.backgroundImage = new StyleBackground(sprite);
    }

    private void HandleClick(string x)
    {
        Debug.Log("hello " + x);
    }
}
