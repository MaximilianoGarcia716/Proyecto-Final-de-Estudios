#ifndef Navegacion_h
#define Navegacion_h

#include "Estructuras.h"
#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <Wire.h>

class Navegacion{
public:
Navegacion();
void Iniciar();
void CalcularDrift(int NumValores);
Coordenadas GetPosicion();
bool TimerListo();
void LeerSensores();
void SetDatoEncoder(Encoders);
void GravedadInicial(int NumValores,int i);
void CorreccionGravedad();
void Kalman();

private:
  unsigned long TimerDatos;
  unsigned long IntervaloLectura;
  float dt;
  MPU6050_Data LecturaSensores;
  Coordenadas Posicion;
  Coordenadas PosicionAnterior;
  Coordenadas PosicionAbsoluta;
  Adafruit_MPU6050 mpu;
  Encoders DatoEncoder;
  Velocidad_Robot Velocidad;
  float PPR=400;
  float Phi0=0;
  float Tita0=0;
  float Tita;             //Asociado a wy
  float Phi;              //Asociado a wx
  float Psi;             //Asociado a wz
  float g=0.0;
  float DriftX=0.0;
  float DriftY=0.0;
  float DriftZ=0.0;
  float P[3]={1.0,1.0,1.0};
  float Q[3]={0.00000324,0.00000324,0.00032527};
  float R[3]={0.0,0.0,0.00001598};
};

#endif
