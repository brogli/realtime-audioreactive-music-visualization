using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConductor : MonoBehaviour
{

    public UserInputsProvider userInputsProvider;
    public IUserInputsUpdater userInputsUpdater;

    // Start is called before the first frame update
    void Start()
    {
        userInputsProvider = new UserInputsProvider();
        userInputsUpdater = new KeyboardUserInputsUpdater(userInputsProvider);
    }

    // Update is called once per frame
    void Update()
    {
        userInputsUpdater.UpdateUserInputs();
        
        foreach (MelodyKey melodyKey in userInputsProvider.melodyKeys.keys)
        {
            Debug.Log(melodyKey.IsPressed);
        }
    }
}
