using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TemporarySceneHandler : MonoBehaviour, IUserInputsConsumer
{
    private UserInputsModel _userInputsModel;

    private bool _isSceneActivationAllowed = false;
    private bool IsSceneActivationAllowed
    {
        get
        {
            bool isAllowed = _isSceneActivationAllowed;
            if (isAllowed)
            {
                Debug.Log("resetting to false");
                _isSceneActivationAllowed = false;
            }
            return isAllowed;
        }
        set
        {
            _isSceneActivationAllowed = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        SubscribeUserInputs();
    }

    public void OnApplicationQuit()
    {
        UnsubscribeUserInputs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            //m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
            Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                //m_Text.text = "Press the space bar to continue";
                Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%, trying to activate");
                //Wait to you press the space key to activate the Scene
                if (IsSceneActivationAllowed)
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void LoadScene(float index)
    {
        StartCoroutine(LoadSceneAsync((int)index));
    }

    public void SubscribeUserInputs()
    {
        _userInputsModel.LoadScene.EmitKeyTriggeredEventWithValue += LoadScene;
        _userInputsModel.ActivateScene.EmitKeyTriggeredEvent += () => IsSceneActivationAllowed = true;
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.LoadScene.EmitKeyTriggeredEventWithValue -= LoadScene;
        _userInputsModel.ActivateScene.EmitKeyTriggeredEvent -= () => IsSceneActivationAllowed = true;
    }
}
