using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ejemplo : MonoBehaviour {

	public GameObject[] carlos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for(int i=0; i < carlos.Length; i++){
			print ("Mi varlos es: "+i);
			print ("Lo que hay en "+i+" es: "+carlos[i].name);
		}
	}
}
