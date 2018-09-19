/*
 * Métodos para el control del sistema de Leds que se usan en la estructura.
 * Los Leds están conectados a una plaqueta que tiene por dentro el controlador 74HC595
 * que nos permite manejar el comportamiento de los LEDs de una manera más Rápida y sencilla.  
 */

/*
Método para apagar los Leds, los cuales 
se apagarán de manera descendente dependiendo a lo
que manden en la variable cont que significa los
rangos mínimos y máximos que podra tener (de 0 a 8)
para el integrado.
*/

void apagarLedsSecuencia(){

  
      digitalWrite(latchPin,LOW);
      shiftOut(dataPin,clockPin,MSBFIRST,ledP[var_contledP]);
      //delay(600);
      digitalWrite(latchPin,HIGH);
    
}


/*
    Método para encender los Leds, los cuales encienden 
    cada fila dependiendo del valor que le manden en cont.

*/
void prenderLedsCont(){
        
      digitalWrite(latchPin,LOW);
      shiftOut(dataPin,clockPin,MSBFIRST,ledP[var_contledP]);
      delay(100);
      digitalWrite(latchPin,HIGH);   
  
}

/*
Método que ayuda a apagar los leds con 
respecto a un tiempo de inactividad en el sistema.
Descenderá desde el último valor de cont
hasta 0.
Falta determinar....
*/
void apagadoTotalLeds(){

      digitalWrite(latchPin,LOW);
      shiftOut(dataPin,clockPin,MSBFIRST,ledP[0]);
      delay(100);
      digitalWrite(latchPin,HIGH);   
  
   /* long tiempoLeds = millis();
    if(millis() - tiempoLeds < tiempoApagado){
      
    }
*/

    var_contledP = 0;
}

//No funciona ni se está usando
void prenderLeds(){
    
    for(int i=0; i<=8;i++){
      digitalWrite(latchPin,LOW);
      shiftOut(dataPin,clockPin,MSBFIRST,ledP[1]);
      delay(600);
      digitalWrite(latchPin,HIGH);
      Serial.println(ledP[1]);
    }
    
}

void apagarLeds(){
    digitalWrite(latchPin,LOW);
    shiftOut(dataPin,clockPin,LSBFIRST,0);
    digitalWrite(latchPin,HIGH);
}


