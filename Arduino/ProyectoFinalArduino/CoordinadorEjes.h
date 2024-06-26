#ifndef CoordinadorEjes_h
#define CoordinadorEjes_h

#include "Estructuras.h"
#include <Arduino.h>
#include <Servo.h>
#include <AccelStepper.h>

namespace NamespaceCoordinadorEjes{
  #define DriverOption 1
  #define ServoPin 2
  #define Enable12VRail 3
  #define Enable5VRail 4
  #define Step1 5
  #define Dir1 6
  #define Step2 7
  #define Dir2 8
  #define Step3 9
  #define Dir3 10
 
}
using namespace NamespaceCoordinadorEjes;

class CoordinadorEjes{
public:
CoordinadorEjes();
void Start();
void ApagarRele();
void EncenderRele();
void SetVelocidadObjetivo(Velocidad_Motor Velocidad);
Encoders GetDato();
int GetServo();
void SetEstadoRele(bool Estado);
void Actualizar();

private:
  AccelStepper Stepper1;
  AccelStepper Stepper2;
  AccelStepper Stepper3;
  bool Estado_Rele=false;
  Encoders DatoEncoder;
  Velocidad_Motor VelocidadObjetivo;
  Velocidad_Motor VelocidadActual;
  Servo Camara_Posicion;
  int AngServo=90;
  int DirServo=1;
  long int AccelTime=1000;
  long int Timer=0;
  long int ServoTimer=0;
  long int PeriodoServo=9223372000000;
};

#endif
