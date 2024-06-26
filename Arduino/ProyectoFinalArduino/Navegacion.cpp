#include "Navegacion.h"

Navegacion::Navegacion(){
}

void Navegacion::Iniciar(){
  //Se inicia el modulo MPU6050
  mpu.begin();
  mpu.setAccelerometerRange(MPU6050_RANGE_8_G);
  mpu.setGyroRange(MPU6050_RANGE_250_DEG);
  mpu.setFilterBandwidth(MPU6050_BAND_21_HZ);
  //Se calcula la deriva del modulo para eliminarla
  int NumValores=20;
  for(int i=0;i<NumValores;i++){
    CalcularDrift(NumValores);
  }
  //Se calcula el angulo para eliminar la gravedad del movimiento
  for(int i=0;i<NumValores;i++){
    LeerSensores();
    GravedadInicial(NumValores,i);
  }
  //Se inicia el timer
  TimerDatos=millis();
  IntervaloLectura=50;
}

void Navegacion::CalcularDrift(int NumValores){
  //Se calcula el drift para cada medicion y se divide por la cantidad de mediciones
  sensors_event_t a, g, temp;
  mpu.getEvent(&a, &g, &temp);
  DriftX=DriftX+g.gyro.x/NumValores;
  DriftY=DriftY+g.gyro.y/NumValores;
  DriftZ=DriftZ+g.gyro.z/NumValores;
}

Coordenadas Navegacion::GetPosicion(){
  //Regresa la posicion en coordenadas globales
  return this->PosicionAbsoluta;
}

bool Navegacion::TimerListo(){
  //Retorna el estado del timer
  bool Timer = false;
  if(millis() - this->TimerDatos >= this->IntervaloLectura){
    Timer = true;
  }
  return Timer;
}

void Navegacion::LeerSensores(){
  //Se leen los sensores, incluyendo el intervalo de tiempo
  sensors_event_t a, g, temp;
  mpu.getEvent(&a, &g, &temp);
  this->LecturaSensores.ax=a.acceleration.x;
  this->LecturaSensores.ay=a.acceleration.y;
  this->LecturaSensores.az=a.acceleration.z;
  this->LecturaSensores.wx=g.gyro.x-DriftX;
  this->LecturaSensores.wy=g.gyro.y-DriftY;
  this->LecturaSensores.wz=g.gyro.z-DriftZ;
  this->LecturaSensores.C=temp.temperature;
  dt=(millis() - this->TimerDatos)/1000.0;
  TimerDatos=millis();
}

void Navegacion::SetDatoEncoder(Encoders Datos){
  //Se cargan los datos del encoder
  this->DatoEncoder=Datos;
}

void Navegacion::GravedadInicial(int NumValores, int i){
  //Se leen los angulos iniciales a partir de la gravedad
  i=i+1;
  float g=sqrt(LecturaSensores.ax*LecturaSensores.ax+LecturaSensores.ay*LecturaSensores.ay+LecturaSensores.az*LecturaSensores.az);
  this->Tita0=Tita0+asin(this->LecturaSensores.ay/g)/NumValores;
  this->Phi0=Phi0+asin(-this->LecturaSensores.ax/(g*cos(Tita0*NumValores/i)))/NumValores;
  this->g=this->g+g/NumValores;
  if(i==NumValores){
    Tita=Tita0;
    Phi=Phi0;
    Psi=0;
  }
}

void Navegacion::CorreccionGravedad(){
  //Se mide y elimina la fuerza de la gravedad de la medicion de los sensores
  float grados=PI/180.0;
  Tita=Tita+(dt)*LecturaSensores.wx;
  Phi=Phi+(dt)*LecturaSensores.wy;
  Psi=Psi+(dt)*LecturaSensores.wz;
  float snPsi=sin(Psi);
  float csPsi=cos(Psi);
  float snPhi=sin(Phi);
  float csPhi=cos(Phi);
  float snTita=sin(Tita);
  float csTita=cos(Tita);
  LecturaSensores.ax=LecturaSensores.ax+g*(snPsi*snTita+csPsi*csTita*snPhi);
  LecturaSensores.ay=LecturaSensores.ay+g*(csTita*snPhi*snPsi-csPsi*snTita);
  LecturaSensores.az=LecturaSensores.az-g*(csPhi*csTita);
}

void Navegacion::Kalman(){

  PosicionAnterior=Posicion;
  //Se realiza la prediccion en base a los encoders y en base a las velocidades
  float Y1=PI*(sqrt(3.0)/3.0*(-DatoEncoder.Encoder2+DatoEncoder.Encoder3))*0.058/PPR;
  float Y2=PI*(1.0/3.0*(-2.0*DatoEncoder.Encoder1+DatoEncoder.Encoder2+DatoEncoder.Encoder3))*0.058/PPR;
  float Y3=PI*((DatoEncoder.Encoder1+DatoEncoder.Encoder2+DatoEncoder.Encoder3)*0.058/(3*0.112))/PPR;

  Velocidad.VelocidadX=Velocidad.VelocidadX+dt*LecturaSensores.ax;
  Velocidad.VelocidadY=Velocidad.VelocidadY+dt*LecturaSensores.ay;
  Velocidad.VelocidadWZ=LecturaSensores.wx;

  float xp1=PosicionAnterior.X+dt*Velocidad.VelocidadX;
  float xp2=PosicionAnterior.Y+dt*Velocidad.VelocidadY;
  float xp3=PosicionAnterior.Psi+dt*Velocidad.VelocidadWZ;

  //Se carga la matriz de covarianza inicial para el punto actual
  P[0]=P[0]+Q[0];
  P[1]=P[1]+Q[1];
  P[2]=P[2]+Q[2];
  
  //Se estima la matriz de Kalman
  float K[3];
  K[0]=P[0]/(P[0]+R[0]);
  K[1]=P[1]/(P[1]+R[1]);
  K[2]=P[2]/(P[2]+R[2]);

  //Se realiza la correccion
  Posicion.X=xp1+K[0]*(Y1-xp1);
  Posicion.Y=xp2+K[1]*(Y2-xp2);
  Posicion.Psi=xp3+K[2]*(Y3-xp3);

  this->Psi=Posicion.Psi;
  
  //Se convierte de coordenadas locales a absolutas
  float DeltaX=Posicion.X-PosicionAnterior.X;
  float DeltaY=Posicion.Y-PosicionAnterior.Y;

  PosicionAbsoluta.X=PosicionAbsoluta.X+DeltaX*cos(Psi)-DeltaY*sin(Psi);
  PosicionAbsoluta.Y=PosicionAbsoluta.Y+DeltaX*sin(Psi)+DeltaY*cos(Psi);
  PosicionAbsoluta.Psi=Posicion.Psi*180.0/PI;
  PosicionAbsoluta.Temperatura=LecturaSensores.C;

  //Se carga la matriz de covarianza corregida
  P[0]=(1-K[0])*P[0];
  P[1]=(1-K[1])*P[1];
  P[2]=(1-K[2])*P[2];
}
