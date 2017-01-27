using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDB : MonoBehaviour {

    public static ColorDB manager;

    public List<Color> colorDB = new List<Color>();

	// Use this for initialization
	void Awake () {
		if(manager == null)
        {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(manager != this)
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
