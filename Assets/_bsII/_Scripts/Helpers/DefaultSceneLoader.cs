#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoadAttribute]
public static class DefaultSceneLoader
{
    static DefaultSceneLoader()
    {
        EditorApplication.playModeStateChanged += LoadDefaultScene;
    }

    static void LoadDefaultScene(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        if (state == PlayModeStateChange.EnteredPlayMode && EditorSceneManager.GetActiveScene() != EditorSceneManager.GetSceneByBuildIndex(0) &&
            EditorSceneManager.GetActiveScene().name != "buildTriangleExplosion")
        {
            EditorSceneManager.LoadScene(0);
        }
    }
}
#endif
