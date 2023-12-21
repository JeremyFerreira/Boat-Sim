using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utility_SceneMethodes : MonoBehaviour
{
    private bool isLoading;
    public void QuitApplication ()
    {
        Application.Quit();
    }

    public void LoadScene (int index)
    {
        if(isLoading) { return; }
        isLoading = true; 
        StartCoroutine(WaitForEndOfBlend(index));
    }

    IEnumerator WaitForEndOfBlend(int index)
    {

        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress >= 0.9f)
            {
                async.allowSceneActivation = true;

                isLoading = false;
            }
            yield return null;
        }
    }
}
