using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class controladorInterfaz : MonoBehaviour {

	// Use this for initialization
	public Image imagenProgressBar;
	public Text textoPorcentajeProgressBar;
    public Text textoPPM1, textoPPM2;

    public GameObject texp1, texp2;

	//private int porcentajeProgressBar;
    [SerializeField]
    private bool estadoEfectoPuntos = false;
    [SerializeField]
    private bool actualizarTextoPorcentaje = false;

    [SerializeField]
    float porcentaje;
    
    bool animPor = false;
    float contPorcentaje = 0;

    //Variables para la animación de Espera
    float contEspera = 0f;
    bool estaActivoEspera = false;

    // Use this for initialization
    void Start () {
        //porcentajeProgressBar = 0;
        imagenProgressBar.fillAmount = 0f;

    }

	// Update is called once per frame
	void Update () {
        //   CambiarTextoPorcentaje();
        if (actualizarTextoPorcentaje)
        {
            CambiarTextoPorcentaje();
            actualizarTextoPorcentaje = !actualizarTextoPorcentaje;
        }

        if (estadoEfectoPuntos)
        {
            estaActivoEspera = !estaActivoEspera;
            EsperaPuntosAnimacion();
        }

    }

    void CambiarTextoPorcentaje()
    {
        print("pasesoa ahflshdl fhapdas");

        imagenProgressBar.fillAmount = ((float)contPorcentaje) / 100;

        textoPorcentajeProgressBar.text = contPorcentaje + " %";
        StartCoroutine(AnimacionCargaPorcentaje());
        
    }

 


    //AAnimación de carga de la barra 
    IEnumerator AnimacionCargaPorcentaje()
    {
        yield return new WaitForSeconds(0.01f);
        if (contPorcentaje < porcentaje)
        {
            contPorcentaje++;
        }

        if (contPorcentaje == porcentaje )
        {
            animPor = false;
            StopAllCoroutines();
        }

        if (animPor)
        {
            StartCoroutine(AnimacionCargaPorcentaje());
        }
        imagenProgressBar.fillAmount = ((float)contPorcentaje) / 100;
        textoPorcentajeProgressBar.text = contPorcentaje + " %";
        print("Animación de carga de porcentaje en curso.");
    }


    public void EsperandoDatosArduinoTexto(bool estado)
    {
        textoPorcentajeProgressBar.text = "";
        estadoEfectoPuntos = estado;
        reinicioInterfazContador();
        
        //StartCoroutine(EfectoPuntos());
    }

    IEnumerator EfectoPuntos()
    {

        yield return new WaitForSeconds(0.5f);
        textoPorcentajeProgressBar.text = ".";
        yield return new WaitForSeconds(0.5f);
        textoPorcentajeProgressBar.text = "..";
        yield return new WaitForSeconds(0.5f);
        textoPorcentajeProgressBar.text = "...";
        
        if (estadoEfectoPuntos)
        {
            StartCoroutine(EfectoPuntos());
        }
    }

    public void AsignarPorcentaje(float porcentajeGas)
    {
        estadoEfectoPuntos = !estadoEfectoPuntos; //Quito la animación
        porcentaje = porcentajeGas;
        print(porcentaje);
        actualizarTextoPorcentaje = true;
        animPor = true;
    }

    public void AsignarPPM(string  valppm1, string valppm2)
    {
        texp1.SetActive(true);
        texp2.SetActive(true);
        textoPPM1.text = "<b>S1:</b> " +valppm1+" ppm";
        textoPPM2.text = "<b>S2:</b> " + valppm2+" ppm";
    }

    public void reinicioInterfazContador()
    {
        imagenProgressBar.fillAmount = 0;
        textoPorcentajeProgressBar.text = "...";
        contPorcentaje = 0;
    }


    void EsperaPuntosAnimacion()
    {
        
        if (estaActivoEspera)
        {
            texp1.SetActive(false);
            texp2.SetActive(false);
            contEspera +=Time.deltaTime;
        }
        if (contEspera>=0.8f)
        {
            textoPorcentajeProgressBar.text = ".";
            print("pase 1");
        }
        if (contEspera >= 1.6f)
        {
            textoPorcentajeProgressBar.text = "..";
            print("pase 2");
        }
        if (contEspera >= 2.4f)
        {
            textoPorcentajeProgressBar.text = "...";
            print("pase 3");
            contEspera = 0;
        }

    }
}
