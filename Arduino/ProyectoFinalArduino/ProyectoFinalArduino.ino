#include <Servo.h>
#include "Comunicacion.h"
#include "Navegacion.h"
#include "CoordinadorEjes.h"

Comunicacion ControlSerie;
Navegacion Kalman;
CoordinadorEjes Steppers;

void setup(void) {  
  //Se inicia cada uno de los objetos
  ControlSerie.Iniciar();
  Kalman.Iniciar();
  Steppers.Start();
}

void loop() {
  //Se revisa si se ha recibido un mensaje completo, se lee y se dan las instrucciones correspondientes al coordinador
  if(ControlSerie.MensajeListo()){  
    ControlSerie.LeerMensaje();
    Steppers.SetEstadoRele(ControlSerie.GetEstadoRele());
    Steppers.SetVelocidadObjetivo(ControlSerie.GetVelocidades());
  }
  Steppers.Actualizar();
  //Se actualiza la navegación si el timer eta listo
  if(Kalman.TimerListo()){
    Kalman.LeerSensores(); 
    Kalman.SetDatoEncoder(Steppers.GetDato());
    Kalman.CorreccionGravedad();
    Kalman.Kalman();
  }
  Steppers.Actualizar();
  //Se envían los datos de la navegación si el timer está listo
  if(ControlSerie.TimerListo()){
    ControlSerie.EnviarNavegacion(Kalman.GetPosicion(),Steppers.GetServo());
  }
  Steppers.Actualizar();
}

void serialEvent(){
  //Se cargan los caracteres recibidos en el buffer
  ControlSerie.LlenarBuffer();
}
