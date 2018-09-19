using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.IO;


public class controlArduino : MonoBehaviour {
	
	[SerializeField]
	SerialPort arduino;

	[SerializeField]
	string puerto = "COM4";
	[SerializeField]
	int baud = 9600;
	[SerializeField]
	bool ledsPrendidos = false;
	public string mensaje = "";
	[SerializeField]
	float tiempo = 8f;
	[SerializeField]
	float tiempoApagando = 6f;
	public GameObject cambioImagen;
	CambiarImagen cambioI;
    [SerializeField]
   // int indexImagen = 0;
    int contCambiarImagen = 0;
	[SerializeField]
	GameObject controlParticulas;
	ControladorParticulas particulas;
	int contParaParticulas = 0;
    float tiempoParticulasEmision=20f;
    bool hayhumo = false;
    [SerializeField] int numDetec;
    [SerializeField]
    GameObject controlInterfaz;
    private controladorInterfaz controlUi;

    [SerializeField]
    int contadorPisadaEquivalente = 0;

    public GameObject pisar;
    //private deteccionPisada pisada;
    
    //Variable para almacenar el promedio de los valores del arduino
    private string[] porcentaje;

    //Variable para medir tiempo de lectura de gases
    [SerializeField]
    //float tiempoLecturaGas, tiempoLecturaGasInicial= 60f;
    // bool activarLecturaGas = false;

    //Variable para los gases 
    float valGas = 0;
    string valSG1;
    string valSG2 ;
    float contLecturaGas = 0;
    float conEsperaLectura = 0;
    bool estaActivaGas = false;
    bool inicioEsperaLecturaGas = false;
    bool lecturaGasFinalizada = false, mandardatos, listos;

    private ControlArduinoRegistradora registradora;
    public GameObject alertaConexion;

  

    // Use this for initialization

    void Awake () {
		arduino = new SerialPort(puerto, baud);
		arduino.ReadTimeout = 50;
        try
        {
            arduino.Open();
            mandardatos = true;
        }
        catch (System.IO.IOException e)
        {
            alertaConexion.SetActive(true);
            mandardatos = false;
            print("No esta conectado el arduino principal: " + e.Message);
        }

    }

	void Start(){
        //Inicio de la segunda comunicación
        registradora = gameObject.GetComponent<ControlArduinoRegistradora>();
        registradora.IniciarComunicacionRegistradora();
        //Demás declaraciones
        //pisada = pisar.GetComponent<deteccionPisada>();
		particulas = controlParticulas.GetComponent<ControladorParticulas> ();
        controlUi = controlInterfaz.GetComponent<controladorInterfaz>();

        //Corutina para la detección de arduino de manera paralela al hilo principal
        if (mandardatos)
        {
          

            StartCoroutine
            (
                AsynchronousReadFromArduino
                ((string s) => leerSerialDeArduino(s),     // Callback
                    () => Debug.LogError("Error!"), // Error callback
                    10000f                          // Timeout (milliseconds)
                )
            );

            cambioI = cambioImagen.GetComponent<CambiarImagen>();
            //Para apagar los Leds al iniciar el sistema
            apagarLedsManual(2);
        }

	}
	/// <summary>
    /// Método público para reintentar la conexión con arduino.
    /// </summary>
    public void ReintentarConexion()
    {
        try
        {
            arduino.Open();
            mandardatos = true;
            StartCoroutine
            (
                AsynchronousReadFromArduino
                ((string s) => leerSerialDeArduino(s),     // Callback
                    () => Debug.LogError("Error!"), // Error callback
                    10000f                          // Timeout (milliseconds)
                )
            );

            cambioI = cambioImagen.GetComponent<CambiarImagen>();
            //Para apagar los Leds al iniciar el sistema
            apagarLedsManual(2);
        }
        catch (System.IO.IOException e)
        {
            alertaConexion.SetActive(true);
            mandardatos = false;
            print("Aún No esta conectado el arduino principal: " + e.Message);
        }
    }


    // Update is called once per fra
    void Update () {        

        if (mandardatos== false)
        {         
            print("No hay conexión con el arduino Principal");
            alertaConexion.SetActive(true);
            print(mandardatos);
            StopAllCoroutines();
        }else if (mandardatos)
        {
            alertaConexion.SetActive(false);  
		    if(arduino.IsOpen)
            {
		    //	print("Lo que dice el puerto: "+leerSerialDeArduino());
			    if(Input.GetKeyUp(KeyCode.Z)){
				    prenderExtractor ();
			    }

                if (Input.GetKeyUp(KeyCode.C) ||  Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.G))
                {//Input.GetKeyUp(KeyCode.W) ||
                    print("me han pisado");
                    
                    DeteccionPisadaInicio();


                }
                CondicionalDeHumno();
               
                CondicionLeds();
            }
            else
            {
                mandardatos = false;
            }

            TiempoLecturaGas();
           
            if (lecturaGasFinalizada)
            {
                print("Pase por aqui");
                DeteccionGasValores();
            
            }
        }
    }

    //Funciones de prueba recopilatoria 
    void DeteccionPisadaInicio()
    {
        //Detengo la corutina existente
        //StopCoroutine(ControlTiempoGas());                
        //Inicio la corutina para la lectura del gas con el sensor 2
        //StartCoroutine(ControlTiempoGas());
        //Inicio contador para partículas y para el cambio de imágenes
        contadorPisadaEquivalente++;

        if (contadorPisadaEquivalente == numDetec)
        {
            tiempo = 8f; //Siempre que se pise se reinicia el contador
            hayhumo = true;
            tiempoParticulasEmision = 20f;
            contadorPisadaEquivalente = 0;
            //Inicio el encendido de Leds desde el nivel básico
            prenderLeds();
            // prenderExtractor();
            ledsPrendidos = true; //Activo la cuenta regresiva para poder apagar los leds en secuencia.
                                  //Leo el gas del sensor 1, ubicado en la parte inferior de la estructura
            leerGas(1);
            //Cambio estado de variable para la lectura de gas
            estaActivaGas = true;
            contLecturaGas = 0;
            conEsperaLectura = 0;
            contParaParticulas++;
            contCambiarImagen++;
            if(registradora.iniciada)
            registradora.ControlRegistradora(5);
            //cambioI.CambiarIconoP();
        }
        

      
        if (contParaParticulas >= 8)
            contParaParticulas = 8;
        //print (contParaParticulas+" ---");
        //Función para controlar el Humo cuando se pise la plataforma.
        humoControl();
    }

    //Funciones de priueba recopilatoria
    void CondicionalDeHumno()
    {
        if (hayhumo)
        {
            tiempoParticulasEmision -= Time.deltaTime;
           // print(tiempoParticulasEmision);
            if (tiempoParticulasEmision <= 0.5f)
            {
                contParaParticulas--; // Para cambiar la cantidad de partículas emitidas, en este caso de menos a mas.
                tiempoParticulasEmision = 20f;
                //Función para controlar el Humo cuando comience el contador de apagado de Leds
                humoControl();
                print("Print particulas: "+contParaParticulas);
            }

            if (contParaParticulas == 0)
            {
                hayhumo = !hayhumo;
            }
        }
    }

    void CondicionLeds()
    {
        if (ledsPrendidos)
        { //Condición para la cuenta regresiva de apagado.				
          //print ("leds prendido :" +ledsPrendidos);
            tiempo -= Time.deltaTime;
            //print (tiempo);
            if (tiempo <= 0.5f)
            { //Condición para detectar que ya no se ha detectado pisadas y comineza el tiempo de apagado.

                tiempoApagando -= Time.deltaTime;

                //print ("Valor tiempo apagando: "+tiempoApagando);
                if (tiempoApagando <= 0.5f)
                {//Condición para darle un delay de 0.5 seg al momento de apagar una linea de LEDs, esto se hace por medio de la variable tiempoApagado
                 //print("Valor original: "+contCambiarImagen);
                    contCambiarImagen--; //Para cambiar las imagenes de manera inversa a través de este valor



                    //print ("Valor en disminución contCambiarImagen: "+contCambiarImagen);
                    //print ("Valor de partículas: "+contParaParticulas);
                    tiempoApagando = 8f;


                   

                    //Función de apagado de Leds
                    apagarLeds();


                }

                if (mensaje == "Llegaste al minimo" )
                { //Condición que determina cuando se apagaron los últimos Leds.
                    ledsPrendidos = !ledsPrendidos; //Lo vuelvo negativo.
                    tiempo = 8f;
                    tiempoApagando = 8f;
                    contCambiarImagen = 0;
                    //contParaParticulas = 0;
                    print("Hola llegue al límite");
                }
            }
        }
    }

    void DeteccionGasValores()
    {
        porcentaje = mensaje.Split(',');
        print("entre al método");
        for (int j = 0; j <= porcentaje.Length; j++)
        {
            print("Ciclo");
            if (porcentaje[0].Equals("Monoxido"))
            {
                
                print(porcentaje);
                valGas = float.Parse(porcentaje[1].ToString());
                print("Valor: " + valGas);
                controlUi.AsignarPorcentaje(valGas);
                lecturaGasFinalizada = !lecturaGasFinalizada;
                confirmaciónLectura("m");
                valSG1 =porcentaje[2].ToString();
                valSG2 = porcentaje[3].ToString();
                controlUi.AsignarPPM(valSG1,valSG2);
            }           
        }
       

    }

	//Método de lectura de puerto serial para leer datos de arduino. Se ejecuta como corutina asincrona para no 
	//dañar el hilo principal de ejecución del programa.
	public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity) {
		DateTime initialTime = DateTime.Now;    
		DateTime nowTime;
		TimeSpan diff = default(TimeSpan);

		string dataString = null;

        if (mandardatos)
        {
                   
		    do {
			    try {
				    dataString = arduino.ReadLine();
			    }
			    catch (TimeoutException) {
				    dataString = null;
                }
                catch (System.IO.IOException e)
                {
                    mandardatos = false;
                    print("Desconectado el arduino");
                    arduino.Close();
                }

			    if (dataString != null)
			    {
				    callback(dataString);
				    yield return null;
			    } else
				    yield return new WaitForSeconds(0.05f);

			    nowTime = DateTime.Now;
			    diff = nowTime - initialTime;

		    } while (diff.Milliseconds < timeout);

		    if (fail != null)
			    fail();
        }
        yield return null;
	}
    /// <summary>
    ///Método  de lectura de arduino para mostrar el sistema. 
    /// </summary>
    /// <param name="mes"></param>
    
    public void leerSerialDeArduino(string mes){
		Debug.Log (mes);
		mensaje = mes;

	}

    /// <summary>
    /// Función para Prender Leds de manera 
    /// manual. 
    /// </summary>

    public void prenderLeds(){
		print ("Prendiendo Leds");
		arduino.Write ("c");
        cambioI.CambiarIconoP();
    }

	/// <summary>
	/// Método para apagar los LEDS, se utiliza en la rutina de apagado autámtico
	/// </summary>
	/// 
	public void apagarLeds(){
		print ("comenzado secuencia de apagado");
		arduino.Write ("v");
        cambioI.CambiarIconoInac();
    }

	/// <summary>
	/// Función para apagar el los LEDs manualmente de todo el  sistema.
	/// </summary>
	/// <param name=null>No recibe parametros.</param>
	/// <returns>No retorna nada.</returns>
	/// 
	public void apagarLedsManual(int opc){
		if (opc == 1) {
			print ("comenzado secuencia de apagado");
			arduino.Write ("Apagando manualmente los Leds en Secuencia");
		}
		if (opc == 2) {
			print ("Apagando los Leds A iniciar");
			arduino.Write ("k");
		}

	}

	/// <summary>
	/// Función para prender el extractor del sistema.
	/// </summary>
	/// <param name="null">no retorna nada.</param>
	/// <returns>No retorna nada.</returns>
	/// 
	public void prenderExtractor(){
		print ("prender Registradora");
		arduino.Write ("h");
	}

	/// <summary>
	/// Función para apagar el extractor del sistema.
	/// </summary>
	/// <param name=null>Vacio</param>
	/// <returns>No retorna nada.</returns>
	/// 
	public void apagarExtractor(){
		print ("apagar");
        arduino.Write("x");
	}

 
 
  

    //Aun no se utiliza
	public void controlFunciones(string opc){
		if(opc == "pisada"){
			//subirRegistradoraT ();
			prenderExtractor ();
			prenderLeds ();
		}
	}


    /// <summary>
    /// Función para controlar el sistema de particulas (humo) 
    /// en la interfaz.
    /// </summary>
    public void humoControl(){

		if (contParaParticulas < 1) {
			particulas.emisionParticulas (0);
		} else if (contParaParticulas >= 8) {
			particulas.emisionParticulas (8);
		} else {
			particulas.emisionParticulas (contParaParticulas);
		}
	}

    /// <summary>
    /// Corutina para determinar el tiempo que queda para iniciar la segunda lectura del sensor de gas.
    /// No se esta utilizando
    /// </summary>
    IEnumerator ControlTiempoGas()
    {
        yield return new WaitForSeconds(4f);
        controlUi.EsperandoDatosArduinoTexto(true);
        yield return new WaitForSeconds(20f);
        leerGas(2);
    }

    void TiempoLecturaGas()
    {
        //Para la primera pausa
        if (estaActivaGas)
        {
            contLecturaGas += Time.deltaTime;
           // print("Valor de contLecturaGas: "+contLecturaGas);
        }
        else
        {
            contLecturaGas = 0;
        }

        if (contLecturaGas>= 4f)
        {
            print("4 seg han pasado");
            inicioEsperaLecturaGas = true;
            contLecturaGas = 0;
            estaActivaGas = !estaActivaGas;
            controlUi.EsperandoDatosArduinoTexto(true);
        }
        //Para el segundo conteo, de 20 seg para tomar la lectura de gas
        if (inicioEsperaLecturaGas)
        {
            conEsperaLectura += Time.deltaTime;
            //print("Valor de conEsperaLectura: " + conEsperaLectura);
        }
        else
        {
            conEsperaLectura = 0;
        }
     
        if (conEsperaLectura>=20f)
        {
            print("Pasaron 20 seg, lectura de variables");
            leerGas(2);
            conEsperaLectura = 0f;
            inicioEsperaLecturaGas = !inicioEsperaLecturaGas;
            lecturaGasFinalizada = !lecturaGasFinalizada;
        }        
    }


    /// <summary>
    /// Este método sirve para -.-..
    /// </summary>
    /// <param name="opc">Este parámetro sirve para-....</param>
    public void leerGas(int opc)
    {
        switch (opc) {
            case 1:
                arduino.Write("u");                
                break;
            case 2:
                arduino.Write("g");
               
                break;
        }        
    }

    public void confirmaciónLectura(string confirmacion)
    {
        arduino.Write(confirmacion);
        
    }

	//Para cerrar conexión con arduino, solo sirve en build
	void OnApplicationQuit(){
		arduino.Close ();
		Debug.Log ("Cerrando conexión arduino");
	}



}
