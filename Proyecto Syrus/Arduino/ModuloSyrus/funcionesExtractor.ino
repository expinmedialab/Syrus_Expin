/**
 * Para funciones del extrator de aire.
 * Esté módulo extractor está conectado al módulo Relé y se alimenta con 120V
 */

 
//Método que controla el encendido y apagado del extractor.
void controlExtractor(String opc){

  //Prendo el ventilador.
  if(opc=="prender"){
   prenderExtractor();
   }else if(opc=="apagar"){
   apagarExtractor();
   }else if(opc == "prenderEspecial"){
   // prendidoEspecial();
   } //fin if
   
     
}//fin método controlExtractor


//Método para prender el extractor
void prenderExtractor(){
  digitalWrite(pinReleEx,HIGH);
  Serial.println("Prendido el extractor");
  //Inicio de la variable que hace conteo para detener el extractor.
  tiempoExtractor = 0;
  
}

//Método para apagar el extractor
void apagarExtractor(){
    digitalWrite(pinReleEx,LOW);
    Serial.println("Apagador el extractor");
    
}

//Método para parar la detección forzada del extractor.
void detenerExtractor(){
    Serial.println("Detención del Extractor");
    digitalWrite(pinReleEx,LOW);
}

//Método para prender el extractor por 4 segundos aproximadamente. Aún esta en prueba. No se UTILIZA
//void prendidoEspecial(){
//  
//  prenderExtractor();
//  if(tiempoExtractor >= 2000){
//    apagarExtractor();
//  }
//    
//}

//Método para prender el extractor por 4 segundos aproximadamente. Aún esta en prueba.
void apagadoExtractorTiempo(){ 

        //Tal vez toca retirar este bloque y pasarlo a la función extractor
        if(var_tiempoApagadoExtractor && tiempoExtractor>=1250){
          //Serial.println(var_tiempoApagadoExtractor && tiempoExtractor>=10000);
          //detenerExtractor();
          //Serial.write("pase por aqui");
          apagarExtractor();          
          var_tiempoApagadoExtractor = false;
          tiempoExtractor = 0;
        }
}

