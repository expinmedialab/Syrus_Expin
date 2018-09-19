using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deteccionPisada : MonoBehaviour {
	//Control arduino
	[SerializeField] GameObject ard;
	controlArduino comando;

	//Contadores de pisada
	int up = 0,d = 0,r = 0,l = 0,w = 0,a = 0;
	//bool prendido = false;
	//float contadorPrendido = 0;
	// Use this for initialization
	void Start () {
		comando = ard.GetComponent<controlArduino> ();

	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.UpArrow)){
			a++;

		}

		if(Input.GetKeyDown(KeyCode.DownArrow)){
			a++;
		}

		if(Input.GetKeyDown(KeyCode.RightArrow)){
			a++;
		}

		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			a++;
		}

		if(Input.GetKeyUp(KeyCode.W)){
			a++;
		}

		if(Input.GetKeyUp(KeyCode.A)){
			a++;
		}

		//Condición para apagar el extractor.
	/*	if (prendido) {
			contadorPrendido++;
			print (contadorPrendido);
			if(contadorPrendido >= 120){
				prendido = !prendido;
				contadorPrendido = 0f;
			}
		}
*/
	//	print (a+" "+w+" "+up+" "+d+" "+r+" "+l);
		cantidadPisadas ();
	}

	void cantidadPisadas(){
		if(a >= 6 || w >= 6 || up >= 6 || d >= 6 || r >= 6 || l >= 6){
			comando.controlFunciones ("pisada");

		//	prendido = true;
			a = 0;
		}
	}

}
