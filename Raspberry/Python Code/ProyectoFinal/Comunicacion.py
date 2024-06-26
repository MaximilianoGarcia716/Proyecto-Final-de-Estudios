import socket
from threading import Thread
from threading import Lock
import serial
import serial.tools.list_ports
import time
import cv2

class Comunicacion:
    def __init__(self):
        self.DisponibleImagen=False
        self.ToogleTensorflow=False
        self.Rele=False
        #Se realiza la configuracion y conexion de los servidores
        for port in serial.tools.list_ports.comports():
            if str(port.manufacturer).startswith("Arduino"):
                ArduinoPort= "/dev/" + str(port.name)
                self.ArduinoSerial = serial.Serial(ArduinoPort , 115200)
        self.ip="10.42.0.1" #"10.42.0.1" para la red de raspberry, ifconfig para cualquier otra
        self.PortControl=40000
        self.PortImagen=40001
        self.PortNavegacion=40002
        self.ServerControl=socket.socket(socket.AF_INET,socket.SOCK_STREAM)
        self.ServerImagen=socket.socket(socket.AF_INET,socket.SOCK_STREAM)
        self.ServerNavegacion=socket.socket(socket.AF_INET,socket.SOCK_STREAM)
        self.ServerControl.bind((self.ip, self.PortControl))
        self.ServerImagen.bind((self.ip, self.PortImagen))
        self.ServerNavegacion.bind((self.ip, self.PortNavegacion))
        #Se preparan los locks y threads
        self.LockImagen=Lock()
        self.LockSerial=Lock()
        
        self.ThreadControl= Thread(target=self.ComunicacionControl)
        self.ThreadImagen= Thread(target=self.ComunicacionImagen)
        self.ThreadNavegacion= Thread(target=self.ComunicacionNavegacion)
        self.ServerControl.listen(1)
        self.ServerImagen.listen(1)
        self.ServerNavegacion.listen(1)
        self.ThreadControl.start()
        self.ThreadImagen.start()
        self.ThreadNavegacion.start()
        time.sleep(3)
        
    def ComunicacionControl(self):
        Buffer=str()
        while True:
            #Se aceptan conexiones
            ClienteControl, _= self.ServerControl.accept()
            print("Control")
            self.Connected=True
            try:
                while self.Connected:
                    Data=ClienteControl.recv(100)  #retorna 0 si se cierra el cliente, retorna un vector de bytes vacio si no hay nada         
                    if Data:
                        if not Data == "":
                            Buffer=Buffer + (Data.decode('UTF-8'))
                            if Buffer[len(Buffer)-1] == ';':
                                #Se ve que tipo de mensaje se ha recibido y se lo pasa al arduino o al objeto Tensorflow
                                if Buffer[0]=='R' or Buffer[0]=='W':
                                    self.LockSerial.acquire
                                    self.ArduinoSerial.write(Buffer.encode('utf-8'))
                                    if Buffer[0]=='R':
                                        self.Rele=not self.Rele
                                    self.LockSerial.release
                                elif Buffer[0]=='T':
                                    self.ToogleTensorflow=True
                                Buffer= ""
                    else:
                        break
            except:
                pass
            #Se realiza la desconexion y se le da la orden de apagar el rele al arduino
            self.Connected=False
            if self.Rele:
                Buffer="R;"
                self.Rele=False
            self.ArduinoSerial.write(Buffer.encode('utf-8'))
            Buffer= ""
            ClienteControl.close()
            print("Desconectado:Control")
        
    def ComunicacionImagen(self):
        while True:
            #Se aceptan conexiones
            ClienteImagen, _= self.ServerImagen.accept()
            print("Imagen")
            self.Connected=True
            try:
                while self.Connected:
                    if self.DisponibleImagen:
                        #Se envia la imagen y ';' para indicar el fin del mensaje
                        self.LockImagen.acquire()
                        _,ImagenPNG=cv2.imencode('.png',self.Imagen)
                        self.DisponibleImagen=False
                        self.LockImagen.release()
                        Send=ImagenPNG.tobytes()+";".encode('UTF-8')
                        ClienteImagen.sendall(Send)
            except:
                pass
            #Se realiza la desconexion
            self.Connected=False
            ClienteImagen.close()
            self.DisponibleImagen=False
            self.Imagen=""
            print("Desconectado:Imagen")
        
    def ComunicacionNavegacion(self):
        Buffer=str()
        while True:
            #Se aceptan conexiones
            ClienteNavegacion, _= self.ServerNavegacion.accept()
            print("Navegacion")
            self.Connected=True
            try:
                while self.Connected:
                    if (self.ArduinoSerial.in_waiting > 0):
                        #Se recibe el mensaje del arduino hasta detectar el ';'
                        self.LockSerial.acquire
                        Buffer=Buffer+self.ArduinoSerial.read(self.ArduinoSerial.in_waiting).decode('utf-8')
                        self.LockSerial.release
                        if Buffer[len(Buffer)-1] == ';':
                            #Se agrega el mensaje del Tensorflow de los FPS y se envia
                            Buffer=Buffer.rstrip(Buffer[len(Buffer)-1])
                            self.LockImagen.acquire()
                            Buffer=Buffer+self.FPS
                            self.LockImagen.release()
                            Send=Buffer.encode('UTF-8')
                            ClienteNavegacion.sendall(Send)
                            Buffer=""
            except:
                pass
            #Se realiza la desconexion
            self.Connected=False
            Buffer=""
            self.ArduinoSerial.read_all()
            ClienteNavegacion.close()
            print("Desconectado:Navegacion")
                
    
    def SetImagen(self,Imagen,fps):
        #Se carga la imagen y el dato de los FPS para enviar
        self.LockImagen.acquire()
        self.DisponibleImagen=True
        self.Imagen=Imagen
        self.FPS=fps
        self.LockImagen.release()
        
    def GetToogleTensorflow(self):
        #Se lee si ha habido orden de conmutar el Tensorlow
        if self.ToogleTensorflow:
            self.ToogleTensorflow=False
            return True
        else:
            return False
