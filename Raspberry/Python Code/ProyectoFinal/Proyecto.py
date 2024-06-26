from Comunicacion import Comunicacion
from Tensorflow import Tensorflow

#from Tensorflow import Tensorflow

import time

import cv2
from picamera2 import Picamera2
#Se inicializan todos los objetos
net = Comunicacion()
picam2 = Picamera2()
picam2.configure(picam2.create_preview_configuration(main={"format": 'RGB888', "size": (640, 480)}))
picam2.start()
TF=Tensorflow()

while True:
    #Se captura la imagen
    Imagen = cv2.rotate(picam2.capture_array(),cv2.ROTATE_180)
    #Si se recibio la orden se cambia el estado del tensorflow
    if net.GetToogleTensorflow():
        TF.ToogleTensorflow()
    #Si la medicion esta lista se carga la imagen para realizar la nueva medicion
    if TF.IsReady():
        TF.SetImage(Imagen)
    #Se procesa la imagen capturada
    fps,imagen=TF.CV2ProcessImage(Imagen)
    #Se carga la imagen para el envio
    net.SetImagen(imagen,fps);
