using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FpsCounter : MonoBehaviour
{
    public int AmountOfIterations = 10;
    private Label _fpsValue;
    private int iterationCounter = 0;
    private float frameTimeAccumulation = 0;

    // Start is called before the first frame update
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;
        _fpsValue = root.Q<Label>("fps-value");
    }

    // Update is called once per frame
    void Update()
    {
        frameTimeAccumulation += Time.smoothDeltaTime;
        
        iterationCounter++;
        if (iterationCounter >= AmountOfIterations)
        {
            var averageFrameTimeInMs = (frameTimeAccumulation / AmountOfIterations);
            var fps = 1f / averageFrameTimeInMs;
            _fpsValue.text = Mathf.FloorToInt(fps).ToString();
            iterationCounter = 0;
            frameTimeAccumulation = 0;
        }
    }
}
