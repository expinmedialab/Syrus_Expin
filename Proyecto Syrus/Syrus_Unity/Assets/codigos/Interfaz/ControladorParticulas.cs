using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControladorParticulas : MonoBehaviour {

	public GameObject parti;
	ParticleSystem particula;
	ParticleSystem.EmissionModule emision;
	ParticleSystem.MinMaxCurve velPart;
	public int numero = 15;
	public int velocidadParticulas = 10;
	//int valorACambiar = 32;
	// Use this for initialization
	void Start () {
		particula = parti.GetComponent<ParticleSystem> ();

	}

	
	// Update is called once per frame
	void Update () {
		

		if(Input.GetKeyUp(KeyCode.J)){
			
			//emisionParticulas ();
		}
	}

	///<summary>
	/// Método para el control de partículas de Humo.
	/// Controla solo la intencidad de partículas emitidas.	
	/// </summary>
	/// <param name="cantidadParticulas"> Cantidad de Partículas que emitirá</param>

	public void emisionParticulas(int cantidadParticulas){
		emision = particula.emission;

		if (cantidadParticulas == 0) {
			emision.rateOverTime = 32f;
		}

		if (cantidadParticulas == 1) {
			emision.rateOverTime = 28f;
		}

		if (cantidadParticulas == 2) {
			emision.rateOverTime = 24f;
		}

		if (cantidadParticulas == 3) {
			emision.rateOverTime = 20f;
		}

		if (cantidadParticulas == 4) {
			emision.rateOverTime = 16f;
		}

		if (cantidadParticulas == 5) {
			emision.rateOverTime = 12f;
		}

		if (cantidadParticulas == 6) {
			emision.rateOverTime = 8f;
		}

		if (cantidadParticulas == 7) {
			emision.rateOverTime = 4f;
		}

		if (cantidadParticulas == 8) {
			emision.rateOverTime = 0.5f;
		}

		//var mainModule = particula.main;
		//mainModule.maxParticles = 6000;

	}



}
