using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CambiarImagen : MonoBehaviour {

	[Header("Icono del arbol que acompaña al comportamiento de los LEDS")] 
	public GameObject arbolIcono;
	Image imagenArbolIcono;
	//Set this in the Inspector
	public Sprite[] arregloSprite;

	int indexImg = 0;

	void Start()
	{
		//Extraigo la imagen del gameObject
		imagenArbolIcono = arbolIcono.GetComponent<Image>();
	}

	void Update()
	{
       
    }


	/// <summary>
	/// Función para cambiar el icono del arbol, dependiendo del estado prendido o apagado de los Leds..
	/// </summary>
	/// <param name="index">Parametro para indicar en el arreglo que imagen se va a cambiar..</param>
	/// <returns>No retorna nada.</returns>
	/// 
	//public void CambiarImagenIcono(int index){
		
	//	imagenArbolIcono.sprite = arregloSprite [index];
	//	print ("Imagen del arbol cambiado");
	//}

	/// <summary>
	/// Función para cambiar el icono del arbol. Esta función es dedicada solo cuando se detecten pisadas. 
	/// </summary>
	/// <returns>No retorna nada.</returns>
	/// 

	public void CambiarIconoP(){
        
        indexImg++;
        print(indexImg);
        if (indexImg >= 8)
        {
            indexImg = 8;
        }
        imagenArbolIcono.sprite = arregloSprite [indexImg];
		
	}

	/// <summary>
	/// Función para cambiar el icono del arbol solo cuando NO se detecten pisadas. 
	/// </summary> 
	/// <returns>No retorna nada.</returns>
	/// 
	public void CambiarIconoInac(){
       
        indexImg--;
        print(indexImg);
        if (indexImg <= 0)
        {
            indexImg = 0;
        }
		imagenArbolIcono.sprite = arregloSprite [indexImg];
		
	}


}
