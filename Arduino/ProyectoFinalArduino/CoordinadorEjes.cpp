#include "CoordinadorEjes.h"

CoordinadorEjes::CoordinadorEjes():
  //Al crearse el coordinador de ejes se crean los objetos Accelstepper con los puertos correspondientes
  Stepper1(DriverOption,Step1,Dir1),
  Stepper2(DriverOption,Step2,Dir2),
  Stepper3(DriverOption,Step3,Dir3)
{}

void CoordinadorEjes::Start(){
  //Se configuran los puertos del rele y se inician apagados, además del servo
  pinMode(Enable12VRail,OUTPUT);
  pinMode(Enable5VRail,OUTPUT);
  digitalWrite(Enable12VRail,HIGH);
  digitalWrite(Enable5VRail,HIGH);
  this->Camara_Posicion.attach(ServoPin);
  //Se configura posicion y velocidad inicial, así como la velocidad máxima
  this->Stepper1.setCurrentPosition(0);
  this->Stepper2.setCurrentPosition(0);
  this->Stepper3.setCurrentPosition(0);
  this->Stepper1.setMaxSpeed(500);
  this->Stepper2.setMaxSpeed(500);
  this->Stepper3.setMaxSpeed(500);
  this->Stepper1.setSpeed(0);
  this->Stepper2.setSpeed(0);
  this->Stepper3.setSpeed(0);
}

void CoordinadorEjes::ApagarRele(){
  //Se apaga el rele y se colocan en 0 todas las velocidades
  digitalWrite(Enable12VRail,HIGH);
  digitalWrite(Enable5VRail,HIGH);
  this->Stepper1.setSpeed(0);
  this->Stepper2.setSpeed(0);
  this->Stepper3.setSpeed(0);
  this->VelocidadObjetivo.DS=0;
  this->VelocidadObjetivo.W1=0;
  this->VelocidadObjetivo.W2=0;
  this->VelocidadObjetivo.W3=0;
  this->VelocidadActual.DS=0;
  this->VelocidadActual.W1=0;
  this->VelocidadActual.W2=0;
  this->VelocidadActual.W3=0;
  this->Estado_Rele=false;
}

void CoordinadorEjes::EncenderRele(){
  //Se enciende el rele y se da inicio a los timers
  digitalWrite(Enable12VRail,LOW);
  digitalWrite(Enable5VRail,LOW);
  this->Estado_Rele=true;
  this->ServoTimer=millis();
  this->Timer=micros();
}

void CoordinadorEjes::SetVelocidadObjetivo(Velocidad_Motor Velocidad){
  if(this->Estado_Rele){
    //Se asignan las velocidades objetivo, solo si el rele se encuentra encendido
    this->VelocidadObjetivo.W1=-Velocidad.W1;
    this->VelocidadObjetivo.W2=-Velocidad.W2;
    this->VelocidadObjetivo.W3=-Velocidad.W3;
    this->VelocidadObjetivo.DS=Velocidad.DS;
    //Se leen las velocidades del servo como grados por segundo y la direccion
    this->DirServo=1;
    if(this->VelocidadObjetivo.DS<0){
      this->VelocidadObjetivo.DS=-this->VelocidadObjetivo.DS;
      this->DirServo=-1;
    }
    if(this->VelocidadObjetivo.DS==0){
      //Se asigna un periodo muy largo si la velocidad es 0
      this->PeriodoServo=9223372000000;
    }
    else{
      //La velocidad actual se establece como un timer en milisegundos
      this->PeriodoServo=1000/this->VelocidadObjetivo.DS;   
    }
  }
}

Encoders CoordinadorEjes::GetDato(){
  //Retorna la posicion de los encoders en pasos
  this->DatoEncoder.Encoder1=-this->Stepper1.currentPosition();
  this->DatoEncoder.Encoder2=-this->Stepper2.currentPosition();
  this->DatoEncoder.Encoder3=-this->Stepper3.currentPosition();
  return this->DatoEncoder;
}

int CoordinadorEjes::GetServo(){
  //Retorna el angulo del servo en grados
  return this->AngServo;
}

void CoordinadorEjes::SetEstadoRele(bool Estado_Rele){
  //Se lee la consigna del rele
  this->Estado_Rele=Estado_Rele;
  if(this->Estado_Rele){
    EncenderRele();
  }
  else{
    ApagarRele();
  }
}

void CoordinadorEjes::Actualizar(){
  if(this->Estado_Rele){
    if(micros()-this->Timer>this->AccelTime){
      //Se mira el timer de aceleracion y se aumenta o disminuye la velocidad de todos los motores
      if(this->VelocidadActual.W1<this->VelocidadObjetivo.W1){
        this->VelocidadActual.W1=this->VelocidadActual.W1+1;
      }
      if(this->VelocidadActual.W1>this->VelocidadObjetivo.W1){
        this->VelocidadActual.W1=this->VelocidadActual.W1-1;
      }

      if(this->VelocidadActual.W2<this->VelocidadObjetivo.W2){
        this->VelocidadActual.W2=this->VelocidadActual.W2+1;
      }
      if(this->VelocidadActual.W2>this->VelocidadObjetivo.W2){
        this->VelocidadActual.W2=this->VelocidadActual.W2-1;
      }

      if(this->VelocidadActual.W3<this->VelocidadObjetivo.W3){
        this->VelocidadActual.W3=this->VelocidadActual.W3+1;
      }
      if(this->VelocidadActual.W3>this->VelocidadObjetivo.W3){
        this->VelocidadActual.W3=this->VelocidadActual.W3-1;
      }
      //Se asignan las nuevas velocidades
      this->Stepper1.setSpeed(this->VelocidadActual.W1);
      this->Stepper2.setSpeed(this->VelocidadActual.W2);
      this->Stepper3.setSpeed(this->VelocidadActual.W3);
      Timer=micros();
    }
    //Se mueven los steppers si el timer interno asi lo dice
    this->Stepper1.runSpeed();
    this->Stepper2.runSpeed();
    this->Stepper3.runSpeed();
    //Se mueve el servo un grado si asi lo dice su timer
    if(millis()-this->ServoTimer>this->PeriodoServo){
      if(this->DirServo==1){
        if(this->AngServo<180){
          this->AngServo=AngServo+1;
        }
      }
      if(this->DirServo==-1){
        if(this->AngServo>0){
          this->AngServo=AngServo-1;
        }
      }
      this->Camara_Posicion.write(this->AngServo);
      this->ServoTimer=millis();
    }
  }
}
