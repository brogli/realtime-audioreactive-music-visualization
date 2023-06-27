using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public delegate void SceneIsReadyToActivate();
    public event SceneIsReadyToActivate EmitSceneIsReadyToActivate;
    private bool _isEmitSceneReadyEvocable = true;

    private bool _isSceneActivationAllowed = false;
    private bool IsSceneActivationAllowed
    {
        get
        {
            // consume "true" and reset to "false"
            bool isAllowed = _isSceneActivationAllowed;
            if (isAllowed)
            {
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
                if (_isEmitSceneReadyEvocable)
                {
                    EmitSceneIsReadyToActivate?.Invoke();
                    _isEmitSceneReadyEvocable = !_isEmitSceneReadyEvocable;
                }
                //Wait to you press the space key to activate the Scene
                if (IsSceneActivationAllowed)
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        _isEmitSceneReadyEvocable = !_isEmitSceneReadyEvocable;
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    public void ActivateScene()
    {
        IsSceneActivationAllowed = true;
    }
}
