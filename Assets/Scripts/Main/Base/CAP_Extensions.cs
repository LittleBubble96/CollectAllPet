using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public static class CAP_Extensions
{
    //扩展task to Coroutine
    public static IEnumerator AsCoroutine(this System.Threading.Tasks.Task task)
    {
        while (!task.IsCompleted)
        {
            yield return null;
        }
        if (task.IsFaulted)
        {
            throw task.Exception;
        }
    }
    
    //扩展 Coroutine to Task
    public static System.Threading.Tasks.Task AsTask(this IEnumerator coroutine)
    {
        var tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
        GameManager.Instance.StartCoroutine(RunCoroutine(coroutine, tcs));
        return tcs.Task;
    }
    
    private static IEnumerator RunCoroutine(IEnumerator coroutine, TaskCompletionSource<bool> tcs) {
        yield return coroutine;
        tcs.SetResult(true);
    }
}