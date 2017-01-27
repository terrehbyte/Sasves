using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackHandler : MonoBehaviour
{
    public UnityEvent OnBackPressed = new UnityEvent();

    public void Back()
    {
        OnBackPressed.Invoke();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void SwitchScenes(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void ToggleGameObject(GameObject target)
    {
        target.SetActive(!target.activeSelf);
    }
}