using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour {
	int var = 0;
	public void moverAOrigen()
	{
		iTween.MoveTo (gameObject, iTween.Hash("x", var));
		var += var + 5;
	}
}
