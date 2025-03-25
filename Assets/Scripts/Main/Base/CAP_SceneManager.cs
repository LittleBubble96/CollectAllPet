using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CAP_SceneManager : Singleton<CAP_SceneManager>
{
    public Coroutine LoadScene(string sceneName, System.Action callback = null, System.Action<float> progressCallback = null)
    {
        return GameManager.Instance.StartCoroutine(LoadSceneAsync(sceneName, callback, progressCallback));
    }

    
    private IEnumerator LoadSceneAsync(string sceneName, System.Action callback,System.Action<float> progressCallback)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            progressCallback?.Invoke(asyncOperation.progress);
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
        callback?.Invoke();
    }
}