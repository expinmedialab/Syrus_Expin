using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfazController : MonoBehaviour {

	public Image imagenProgressBar;
	public Text textoPorcentajeProgressBar;

	private int porcentajeProgressBar;

	// Use this for initialization
	void Start () {
		porcentajeProgressBar = 0;
	}
	
	// Update is called once per frame
	void Update () {
		imagenProgressBar.fillAmount = ((float)porcentajeProgressBar) / 100;
		textoPorcentajeProgressBar.text = porcentajeProgressBar + " %";

		if(porcentajeProgressBar < 100)
		{
			porcentajeProgressBar++;

		}
		
	}
}
