/*
 * Para funciones del sensor de gases MQ-7 sensible al monoxido de carbono
 * 
 * Conexiónes:
 * Pin Análogo A0 ---> Pin lectura análoga MQ-7 s1
 * Pin Análogo A1 ---> Pin lectura análoga MQ-7 s2
 * 
 */

uint8_t pinSensorG1 = A0;
uint8_t pinSensorG2 = A1; 
int tiempoEsperaLecturaGas = 0;

//Varibale para dar inicio y fin de la lectura de gas.
bool inicioLecturaGasS1= false;
//bool finLecturaGasS1 = false;
bool inicioLecturaGasS2 = false;
//bool finLecturaGasS2 = false;

//Variables para controlar el tiempo de medición
unsigned long tiempoAnteriorLecturaGasS1 = 0;
//unsigned long tiempoAnteriorLecturaGasS2 = 0;
int periodoDeLecturaGas1 = 13000; 


//Variables de captura de informacion
int valSensorG1, valSensorG2;
float aireLimpio;
int valPorcentaje;

//Variables para le mapeo de datos de los sensores de gases 
float  Rs1, Rs2;
double ppm1,ppm2;
float volt1,volt2;
const float contGases = 0.0000000008192; 


 //Método para iniciar la captura de datos
 //Depende de que caso se realice, se inicia la captura para alguno de los 2 sensores.
 void deteccionGases(int opc){
  switch(opc){

    case 1: //Caso para inicio de captura del sensor 1
      inicioLecturaGasS1 = true;
      valSensorG1 = analogRead(pinSensorG1);
      mapeoDatos(1);
      Serial.print("Valor dato sensor mq7 1: ");
      Serial.println(valSensorG1);

    break;

    case 2: //Caso para inicio de captura del sensor 2
      inicioLecturaGasS2 = true;
      valSensorG2 = analogRead(pinSensorG2);
       mapeoDatos(2);
      Serial.print("Valor dato sensor mq7 2: ");
      Serial.println(valSensorG2);
      porcentajeAireLimpio();
      esperandoConfirmacion = true;
    break;
  }
   
   
  
 }


//Método para determinar el tiempo transcurrido apra capturar los datos entre el sensor G1 y el sensor G2 MQ.7
//Se aplica en la función Loop()
void tiempoCapturaDatosGas(){
  if(inicioLecturaGasS1){
    //Tomo el tiempo en donde se detectó una pisada e inició el extractor
    tiempoAnteriorLecturaGasS1 = millis();
    //Paso la variable a estado original
    inicioLecturaGasS1 = false; 
  }

  if(millis()- tiempoAnteriorLecturaGasS1 >= periodoDeLecturaGas1 && inicioLecturaGasS1){
    //Determino que es hora de iniciar la captura de los valores del sensor G2
    deteccionGases(2);    
  }
  
}

//Método para calcular el porcentaje de aire limpiado
void porcentajeAireLimpio(){
//Formula para determinar la limpieza de datos relativa
  //aireLimpio = valSensorG2 - valSensorG1;
  aireLimpio = abs(ppm2-ppm1);
  Serial.println(aireLimpio);  
//Determino el porcentaje (abstracto) de limpieza de aire en ese flujo de aire.
  //valPorcentaje = (100 * aireLimpio)/valSensorG1;
  valPorcentaje = (int)(100*aireLimpio/ppm1);
  Serial.print("Monoxido");
  Serial.print(",");
  Serial.print(valPorcentaje); 
  Serial.print(",");
  Serial.print(abs(ppm1));
  Serial.print(",");
  Serial.println(abs(ppm2));
}

//Método para tranformar el valor de los sensores en 
//valores de lectura de ppm (¡¡Datos con errores de precisión!!, no tomarlos como guía)
void mapeoDatos(int sensor){  
  switch(sensor){
      case 1:
           volt1 = valSensorG1 * (5.0 / 1023.0);
           Rs1 = 10000*((5-volt1)/volt1);
           ppm1 = 1000*Rs1+contGases; 
           Serial.println(ppm1);       
        break;
      case 2: 
          volt2 = valSensorG2 * (5.0 / 1023.0);
          Rs2 = 10000*((5-volt2)/volt2);
          ppm2 = 1000*Rs2+contGases;  
           Serial.println(ppm2); 
        break;
  }  
}

