using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.IO;

public class ControlArduinoRegistradora : MonoBehaviour {

    [SerializeField]
    SerialPort arduinoRegistradora;

    [SerializeField]
    string puerto = "COM8";
    [SerializeField]
    int baud = 9600;

    string mensajeRegistradora="";
    public bool iniciada = false;
    // Use this for initialization
    void Start()
    {

    }
    
    public void IniciarComunicacionRegistradora()
    {

        arduinoRegistradora = new SerialPort("\\\\.\\" + puerto, baud);
        arduinoRegistradora.ReadTimeout = 50;
        try
        {
            arduinoRegistradora.Open();
            iniciada = true;
            StartCoroutine
        (
            AsynchronousReadFromArduinoRegistradora
            ((string s) => leerSerialDeArduino(s),     // Callback
                () => Debug.LogError("Error!"), // Error callback
                10000f                          // Timeout (milliseconds)
            )
        );
        }
        catch (System.IO.IOException e)
        {
            print("No esta conectado el segundo arduino: "+e.Message);
            iniciada = false;
        }
        
    }
	// Update is called once per frame
	void Update () {
        if (iniciada)
        {

            //Condiciones para el ejemplo
            if (Input.GetKeyUp(KeyCode.H))
            {
                ControlRegistradora(1);

            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                ControlRegistradora(2);

            }
            else if (Input.GetKeyUp(KeyCode.K))
            {
                ControlRegistradora(3);
            }

        }
    }

    
    /// <summary>
    /// Método de lectura de puerto serial para leer datos de arduino. Se ejecuta como corutina asincrona para no 
    ///dañar el hilo principal de ejecución del programa.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="fail"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public IEnumerator AsynchronousReadFromArduinoRegistradora(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        yield return new WaitForSeconds(.1f);
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            try
            {
                dataString = arduinoRegistradora.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                yield return null;
            }
            else
                yield return new WaitForSeconds(0.05f);

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);

        if (fail != null)
            fail();
        yield return null;
    }

    //Método  de lectura de arduino para mostrar el sistema.
    public void leerSerialDeArduino(string mes)
    {
        Debug.Log(mes);
        mensajeRegistradora = mes;

    }

    
    /// <summary>
    /// Función para indicar el comportamiento de la registradora.
    /// </summary>
    /// <param name=opc>Indica el tipo de comportamiento del motor de la registradora, 1= subir, 2= bajara, 3= detener</param>
    /// <returns>No retorna nada.</returns>
    /// 
    public void ControlRegistradora(int opc)
    {
        if (iniciada)
        {        
            switch (opc)
            {
                case 1:
                    SubirRegistradora();
              
                    break;
                case 2:
                    BajarRegistradora();
                    break;
                case 3:
                    DetenerRegistradora();
                    break;
                case 4:
                    StartCoroutine(SolicitudMovimiento());
                    break;
                case 5:
                    TiempoSubirRegistradora();
                    break;

            }
        }
    }

    /// <summary>
    /// Función para subir la registradora, mandando el caracter "s".
    /// </summary>

    private void SubirRegistradora()
    {   
        arduinoRegistradora.Write("s");
    }
    /// <summary>
    /// Función para bajar la registradora, mandando el caracter "b".
    /// </summary>
    private void BajarRegistradora()
    {
        arduinoRegistradora.Write("b");
    }

    /// <summary>
    /// Función para detener la registradora, mandando el caracter "d".
    /// </summary>
    private void DetenerRegistradora()
    {
        arduinoRegistradora.Write("d");
    }

    /// <summary>
    /// Función para subir por un tiempo la registradora, mandando el caracter "d".
    /// </summary>
    private void TiempoSubirRegistradora()
    {
        arduinoRegistradora.Write("t");
    }

    /// <summary>
    /// Corutina para subir la registradora durante 2 segundos y luego detenerla..
    /// </summary>
    IEnumerator SolicitudMovimiento()
    {
        SubirRegistradora();
        yield return new WaitForSeconds(2f);
        DetenerRegistradora();
    }

   
    void OnApplicationQuit()
    {
        arduinoRegistradora.Close();
        Debug.Log("Cerrando conexión arduino Registradora");
    }
}
