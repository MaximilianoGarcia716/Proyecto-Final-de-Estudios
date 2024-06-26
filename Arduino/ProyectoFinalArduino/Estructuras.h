#ifndef Estructuras_h
#define Estructuras_h

struct Coordenadas {
  float X=0;
  float Y=0;
  float Psi=0;
  float Temperatura=0;
};

struct Velocidad_Robot{
  int VelocidadX=0;
  int VelocidadY=0;
  int VelocidadWZ=0;
  int VelocidadServo=0;
};

struct MPU6050_Data{
  float ax=0;
  float ay=0;
  float az=0;
  float wx=0;
  float wy=0;
  float wz=0;
  float C=0;
};

struct Encoders{
  long Encoder1=0;
  long Encoder2=0;
  long Encoder3=0;
};

struct Velocidad_Motor{
  int W1=0;
  int W2=0;
  int W3=0;
  int DS=0;
};

#endif
