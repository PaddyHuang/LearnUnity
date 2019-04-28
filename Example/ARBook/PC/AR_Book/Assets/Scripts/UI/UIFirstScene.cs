using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFirstScene : MonoBehaviour
{
    public void OnPlayEnd()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(1);
    }
}