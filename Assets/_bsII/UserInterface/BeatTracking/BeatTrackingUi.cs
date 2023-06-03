using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BeatTrackingUi : MonoBehaviour
{
    private ProgressBar _fourInFourProgress;

    private ProgressBar _oneInFourProgress;
    private ProgressBar _oneInEightProgress;
    private MusicInputsModel _musicInputsModel;

    // Start is called before the first frame update
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();
        _fourInFourProgress = root.Q<ProgressBar>("4-4");
        _oneInFourProgress = root.Q<ProgressBar>("1-4");
        _oneInEightProgress = root.Q<ProgressBar>("1-8");
    }

    // Update is called once per frame
    void Update()
    {
        _fourInFourProgress.value = _musicInputsModel.FourInFourValue;
        _oneInFourProgress.value = _musicInputsModel.OneInFourValue;
        _oneInEightProgress.value = _musicInputsModel.OneInEightValue;
    }
}
