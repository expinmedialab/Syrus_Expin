using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour {
	public SerialPort serialPort;
	public GameObject imagen;
	// Use this for initialization
	void Start () {
		serialPort  = new SerialPort ("COM9", 9600);
		serialPort.Open ();
		serialPort.ReadTimeout = 1;
//		stream.Open ();
	}
	
	// Update is called once per frame
	void Update () {
		

	if (serialPort.IsOpen) {

			try
			{
				Debug.Log(serialPort.ReadLine());
			}
			catch (System.Exception){

			}
			if(Input.GetKeyUp(KeyCode.A)){
			serialPort.Write ("c");	

				imagen.GetComponent<Movimiento> ().moverAOrigen ();
	}
	else {
		print ("No está abierto la conexión");
	}
		
	}
}
}
