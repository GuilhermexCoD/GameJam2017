﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySeconds : MonoBehaviour {
	public float delay;
	public GameObject p;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Destroy (this.gameObject,delay);
		
	}
}
