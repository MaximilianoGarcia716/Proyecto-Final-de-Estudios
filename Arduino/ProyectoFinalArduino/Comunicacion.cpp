#include "Comunicacion.h"

Comunicacion::Comunicacion(){
}

void Comunicacion::Iniciar(){
  //Se inicia el puerto serie
  Serial.begin(115200);
}

bool Comunicacion::MensajeListo(){
  //Se devuelve el flag que indica que el mensaje esta listo para leer
  return this->EstadoMensaje;
}

void Comunicacion::EnviarNavegacion(Coordenadas Navegacion, int Servo){
  if(this->Estado_Rele==false){
    //Se arma el mensaje para enviar si esta encendido el rele
    Serial.print("D;");
  }
  else{
    //Se arma el mensaje para enviar si esta encendido el rele
    Serial.print("X");
    Serial.print(Navegacion.X);
    Serial.print("Y");
    Serial.print(Navegacion.Y);
    Serial.print("P");
    Serial.print(Navegacion.Psi);
    Serial.print("S");
    Serial.print(Servo);
    Serial.print("C");
    Serial.print(Navegacion.Temperatura);
    Serial.print(";");
  }
}

void Comunicacion::LlenarBuffer(){
  //Se cargan los caracteres en el buffer hasta que encuentra el ';' y marca como listo el mensaje
  while(Serial.available()>0){
    this->Buffer[this->Puntero]=Serial.read();
    this->Puntero=this->Puntero+1;
  }
  if(this->Buffer[this->Puntero-1]==';'){
    this->EstadoMensaje=true;
  }
}

void Comunicacion::LeerMensaje(){
  this->EstadoMensaje=false;
  this->Puntero=0;
  if(this->Buffer[Puntero]=='R' && this->Estado_Rele==true){
    //Se apaga el rele e ignorar el resto del mensaje
    this->Estado_Rele=false;   
  }
  else if(this->Buffer[Puntero]=='R'){       
    //Encender Rele        
    this->Estado_Rele=true;
  }
  else if(this->Buffer[Puntero]=='W'){               
    //Cambiar la velocidad de los steppers, expresada en ancho de pulso
    this->Puntero=Puntero+1;
    this->Velocidades.W1=Decode(Puntero);
    this->Puntero=Puntero+1;
    this->Velocidades.W2=Decode(Puntero);
    this->Puntero=Puntero+1;
    this->Velocidades.W3=Decode(Puntero);
    this->Puntero=Puntero+1;
    this->Velocidades.DS=Decode(Puntero);
  }
  else if(this->Buffer[Puntero]=='D'){
    //Cambiar la velocidad del servo, expresada en grados por segundo
    this->Estado_Rele=false;
  }
  this->Puntero=0;
}

bool Comunicacion::GetEstadoRele(){
  //retorna el estado deseado del rele
  return this->Estado_Rele;
}

Velocidad_Motor Comunicacion::GetVelocidades(){
  //retorna las consignas de velocidad
  return this->Velocidades;
}

float Comunicacion::Decode(int Posicion){
  //se decodifica el mensaje como entero con signo
  int Numero=0;
  int Signo=1;
  if(this->Buffer[Posicion]=='-'){
    Signo=-1;
    Posicion=Posicion+1;
  }
  while(Buffer[Posicion]!=',' && Buffer[Posicion]!=';'){
    Numero=Numero*10;
    Numero=Numero+(Buffer[Posicion]-'0');
    Posicion=Posicion+1;
  }
  this->Puntero=Posicion;
  return Numero*Signo;
}

bool Comunicacion::TimerListo(){
  //Se retorna el estado del temporizador
  bool Timer = false;
  if(millis() - this->TimerComunicacion >= this->IntervaloComunicacion){
    Timer = true;
    this->TimerComunicacion=millis();
  }
  return Timer;
}
