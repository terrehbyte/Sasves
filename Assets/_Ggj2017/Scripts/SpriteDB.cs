﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDB : MonoBehaviour {

    public static SpriteDB manager;

    public List<Sprite> spriteDB = new List<Sprite>();

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
