/*
 * Proyecto Syrus, Módulo participativo para la Limpieza de Aire por medio de la interacción de las personas.
 * Componentes: Arduino UNO R3, Controlador 74HC595, Extractor de Aire,  Módulo Rele 4 Canales, Sensor de Gases CO MQ-7 y LEDs
 * 
 */


//Variables globales para el control del sistema

//Pin Rele extractor
int pinReleEx =8; //PIN digital  para controlar el rele que enciende o apaga el extractor. Recordar la conexión con la fuente de poder de 12 voltios a 1A (FUente de PC).

/******Pines Sw******/
/*int pinSw = 11; //Pin que recibe el cambio de voltaje cuando el tapete es pisado. Es tipo INPUT.
int pinOutputSw = 10;//Pin que sirve para SIMULAR EL interruptor. Tiene estado OUTPUT. Recordar que debe haber una resistencia mínima de 125 ohm para no producir cortos.
int estadoPinSw = 0; //Esta variable sirve como bandera para contar una pisada por separado.
int estadoAnteriorPinSw = 0; //Variable para guardar el estado anterior del pinSw
*/
/******       ******/

//Pines reles para Registradora, estos pines son para el control de 2 reles que permitirán cambiar la polaridad del motor de la registradora.
int pinRele1motor = 9; //Pin rele 1  (pin Rele in2) para manipular la elevación de la registradora.
int pinRele2motor = 8; //Pin rele 2  (pin rele in1) para manipular el descenso de la registradora.

//Pines para Switches finales de carrera que indican el máximo y el mínimo de desplazamiento de la registradora. Estan establecidos en el pin 2 y 3 porque
//hacen referencia al uso de interrupciones para esta operación.
//int pinSwSuperior =  3;
//int pinSwInferior = 2;


//Variable de tiempo de desplazamiento
 int tiempoMovimiento=2000;
 int tiempoReinicio;
 int tiempoPausa=500;

 
//Variables para detención forzada.
//bool var_Panico=false;

//Variable bandera para detectar los finales de carrera
bool var_sws = true;
bool var_swi = true;

bool var_swiInicio = false; //Variable que ayuda a detectar el reinicio del sistema
bool var_swsReinicio = false; //Variable que ayuda a detectar el final del desplazamiento para reiniciar el sistema.

//Variable para el bloqueo del sistema mientras se reinicia la posición inicial. Esto solo pasa cuando se toca el SW superior.
//bool var_bloque = false;

//Variables de conteo para personas que pisaron el tapete.
//int contador_tapete=0;

//Variable pin para detención FOrzada.
//int pinD = 3;

//Variables de control de tiempo del extractor.
long tiempoExtractor;
bool var_tiempoApagadoExtractor = false;

//Variables para la detección de las interrupciones, dependiendo en que switch fue producida, cada una es modificada a un valor
//verdadero.
//bool var_swFCInferior = false;
//bool var_swFCSuperior = false;

//Para verificar el sistema de luces, esto mantendrá el sistema de los 
bool verificacionLeds = false;

//Variable para dar inicio al sistema de la registradora.
bool var_inicio = true; //Modificar a false 

/***
Variables para control del Micro 74HC595 que hará el manejo de las Luces del sistema.
***/
//Pines básicos de conexión con el micro para el manjeo de señales
int dataPin = 6;  //Se conecta con el pin D5 del micro (pin14 del micro)
int latchPin = 5; // Se conecta con el pin ST_CP (pin12 del micro)
int clockPin = 4; //Se conecta con el pin SH_CP (pin11 del micro)
//Variable para animaciones de prendido y apagado por filas.
byte ledP[] = {0b00000000,0b00000001,0b00000011,0b00000111,0b00001111, 0b00011111, 0b00111111, 0b01111111, 0b11111111};
int var_contledP = 0;
/***
***/
int var_prueba = 1345;
bool esperandoConfirmacion = false;



void setup() {
  tiempoExtractor = 0; // Reinicio la variable tiempo extractor
  esperandoConfirmacion = false;


  //Llamo al metodo COnfiguración inicial para dar los parametros iniciales al arduino.
  configuracionInicial();
//  desplazarRegistradora("parar");
  //iniciarR();
  detenerExtractor();
  //Creación de interrupciones para la detección de los Switches finales de carrera superior e inferior.
  //Como se está usando un Arduino UNO, las interrupciones serán manejadas SOLO en los Pines 2 (para Sw inferior) y 3 (para Sw superior)

}

void loop() {

  if(Serial.available()){
    
  
  char opc = Serial.read();
       //Ejemplo prueba para mandar datos codificados 
 
    /*
    Para manejar los LEDS con los datos del puerto serial del arduino
    */
    if(opc == 'c'){
      //funciones para prender el extractor
      
      //Funciones para inicio de detección de gas de monóxido de carbono
      deteccionGases(1);


    //Funciones de conteo para determinar el comportamiento de los LEDS
    //Caso 1: llegó al máximo  
     if(var_contledP ==8){
        Serial.println("Llegaste al maximo");
      }else{
         var_contledP ++;
         Serial.println(var_contledP);
         prenderLedsCont();
         controlExtractor("prender");
         var_tiempoApagadoExtractor = true; //Variable para activar la condición de apagado en 2 segunda 
         //prenderLeds();
      }
    }
    //Caso 2: llegó al mínimo  
    if (opc == 'v'){
          Serial.println("v");
          if(var_contledP ==0){
              Serial.println("Llegaste al minimo");
              //verificacionLeds = !verificacionLeds;
            }else{
                var_contledP --;
                apagarLedsSecuencia();
            }  
    }    
    if (opc == 'k'){ // Para apagar todos los Leds, este método sirve para llamarlo desde Unity3D
      Serial.println("Apagando los Leds");
       apagadoTotalLeds();
    }

  //Control manual del Extractor
  if(opc == 'z' || opc == 'h'){
      Serial.println("Prendo el Extractor");
      controlExtractor("prender");
      var_tiempoApagadoExtractor = true; //Variable para activar la condición de apagado en 2 segunda 
  }

  if(opc == 'x'){
    Serial.println("Deteccion, Apago el Extractor");
    controlExtractor("apagar");
  }
     
   

         
    //delay(800);
    
    //Para la lectura  manual de gases por medio del MQ-7  
    if(opc == 'g'){
      deteccionGases(2);
    }
    if(opc == 'u'){
      deteccionGases(1);
    }

    if(esperandoConfirmacion){
      porcentajeAireLimpio();
    }
    if(opc == 'm'){
      esperandoConfirmacion = !esperandoConfirmacion;
      Serial.println("Si funciono!");
    }
    
//   if(verificacionLeds){
//      apagadoTotalLeds();
//      Serial.println("Apagué todos los Leds");
//      verificacionLeds = !verificacionLeds;
//    }
    opc = "";

  }
   //La variable tiempoExtractor ira creciendo cada ciclo de
    //ejecución, pero se hace una condición para darle un límite y 
    //se reinicie a su valor inicial de la variable tiempoExtractor..
    tiempoExtractor++;
//    if(tiempoExtractor >= 1000000){
//      tiempoExtractor = 0;
//    }
    //Serial.println(tiempoExtractor);
    //Serial.println(var_tiempoApagadoExtractor);
    delay(10);
     apagadoExtractorTiempo();
     
    Serial.flush(); //Limpio el Buffer por cada ciclo
  
  
 
} //Fin Loop


//Método de configuración a la placa.
void configuracionInicial(){

  //Inicio la comunicación serial a 9600.
  Serial.begin(115200);  
  Serial.setTimeout(50);
  //Inicializo los modos de cada PIN
  //Para pinRele extractor
  pinMode(pinReleEx,OUTPUT);
  digitalWrite(pinReleEx,LOW);  
  //Configuracion de los leds para comunicacion con el micro integrado 74hc595
  pinMode(dataPin,OUTPUT);
  pinMode(latchPin,OUTPUT);
  pinMode(clockPin,OUTPUT);
  digitalWrite(dataPin, LOW);
  digitalWrite(latchPin, HIGH);
  
  //Apago los LEDS para el inico del sistema
  apagadoTotalLeds();

  delay(100);
  Serial.write("Comunicación iniciada a 9600 Baudios. Bienvenido...");
  Serial.println("Rele asignado en pin "+ String(pinReleEx)+ "estado HIGH");
  Serial.println("Finales de carrera pin fin de carrera superior asignado en pin "+ String(pinReleEx)+ "en modo Input");
  Serial.println("Configuración del microprocesador 74HC595: DataPin: "+ String(dataPin)+ "en modo OUTPUT. y en LOW");
  Serial.println("Configuración del microprocesador 74HC595: DataPin: "+ String(latchPin)+ "en modo OUTPUT.y en HIGH");
  Serial.println("Configuración del microprocesador 74HC595: DataPin: "+ String(clockPin)+ "en modo OUTPUT.");
  delay(100);
}


//Metodo de inicio para pruebas
void configuracionInicialPruebas(){
  Serial.begin(9600);
  pinMode(7,OUTPUT); //Pin de correinte extra simulada.
  //Para pines Rele  motor registradora.
  pinMode(pinRele1motor,OUTPUT);
  pinMode(pinRele2motor,OUTPUT);
  
}




