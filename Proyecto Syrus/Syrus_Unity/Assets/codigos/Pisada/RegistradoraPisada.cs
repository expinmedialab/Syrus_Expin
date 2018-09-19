using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistradoraPisada : MonoBehaviour {
    
    //Control arduino
    [SerializeField] GameObject ard;
    ControlArduinoRegistradora comando;

    //Variables para detectar pisadas
    int a;

    // Use this for initialization
    void Start () {
        comando = ard.GetComponent<ControlArduinoRegistradora>();
	}
	
	// Update is called once per frame
	void Update () {
        PisadaDetectada();
        if (a>=3)
        {            
            ActivarRegistradora();            
        }
	}

    public void PisadaDetectada()
    {
        //if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.G))
        //{
        //    a++;           
        //}

    }


    void ActivarRegistradora(){
        comando.ControlRegistradora(5);
        print("Active la registradora!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        a = 0;
    }
}
