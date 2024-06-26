#ifndef Comunicacion_h
#define Comunicacion_h

#include "Estructuras.h"
#include <Arduino.h>

class Comunicacion{
public:
Comunicacion();
void Iniciar();
bool MensajeListo();
void EnviarNavegacion(Coordenadas Navegacion,int Servo);
void LlenarBuffer();
void LeerMensaje();
bool GetEstadoRele();
Velocidad_Motor GetVelocidades();
float Decode(int Posicion);
bool EnvioActivo();
bool TimerListo();

private:
  char Buffer[64];
  int Puntero=0;
  bool EstadoMensaje=false;
  Velocidad_Motor Velocidades;
  bool Estado_Rele=false;
  unsigned long TimerComunicacion=0;
  unsigned long IntervaloComunicacion=1000;
};

#endif