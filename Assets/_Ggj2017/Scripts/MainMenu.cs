using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void doExit()
    {
        #if UNITY_EDITOR
           UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void enterOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void enterCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void enterPlay()
    {
        SceneManager.LoadScene("Play");
    }

    public void enterCalibrate()
    {
        SceneManager.LoadScene("Calibrate");
    }
}
