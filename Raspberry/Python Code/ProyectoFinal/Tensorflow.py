import os
import cv2
import numpy as np
import sys
import time
from threading import Thread
import importlib.util
from tflite_runtime.interpreter import Interpreter
from threading import Thread
from threading import Lock

class Tensorflow:
    def __init__(self):
        self.min_conf_threshold = 0.20
        self.imW=640
        self.imH=480

        #Se carga el mapa de etiquetas
        with open("/home/maxiwarhammer/Documents/ProyectoFinal/labelmap.txt", 'r') as labelmap:
            self.labels = [line.strip() for line in labelmap.readlines()]

        #Se carga el modelo de Tensorflow
        self.interpreter = Interpreter(model_path="/home/maxiwarhammer/Documents/ProyectoFinal/detect.tflite")

        self.interpreter.allocate_tensors()

        #Se leen los detalles del modelo
        self.input_details = self.interpreter.get_input_details()
        self.output_details = self.interpreter.get_output_details()
        self.height = self.input_details[0]['shape'][1]
        self.width = self.input_details[0]['shape'][2]

        self.input_mean = 127.5
        self.input_std = 127.5

        #Se configura el modelo como TF2
        self.boxes_idx, self.classes_idx, self.scores_idx = 1, 3, 0
        
        #Se inicia el calculo de FrameRate (o FPS)
        self.TFframe_rate_calc = 1
        self.freq = cv2.getTickFrequency()
        self.Ready=True;
        self.TensorflowActive=True
        
        #Se activan los threads y locks
        self.LockOutput=Lock()
        self.LockImage=Lock()
        self.ThreadDetect= Thread(target=self.TFDetect)
        self.ThreadDetect.start()

        
    def TFDetect(self):
        while True:
            while not self.Ready and self.TensorflowActive:
                #Se toma el tiempo inicial del frame
                t1 = cv2.getTickCount()
                
                #Se obtiene la imagen
                self.LockImage.acquire()
                frame=self.Imagen
                self.LockImage.release()
                frame_resized = cv2.resize(frame, (self.width, self.height))
                input_data = np.expand_dims(frame_resized, axis=0)
    
                #Se normalizan los pixeles
                input_data = (np.float32(input_data) - self.input_mean) / self.input_std

                #Se realiza la deteccion
                self.interpreter.set_tensor(self.input_details[0]['index'],input_data)
                self.interpreter.invoke()
                #Se guardan los resultados
                self.LockOutput.acquire()
                self.boxes = self.interpreter.get_tensor(self.output_details[self.boxes_idx]['index'])[0] # Bounding box coordinates of detected objects
                self.classes = self.interpreter.get_tensor(self.output_details[self.classes_idx]['index'])[0] # Class index of detected objects
                self.scores = self.interpreter.get_tensor(self.output_details[self.scores_idx]['index'])[0] # Confidence of detected objects
                self.TFframe_rate_calc= self.freq/(cv2.getTickCount()-t1)# Calculate framerate
                self.LockOutput.release()
                self.Ready=True;

    def CV2ProcessImage(self,imagen):
        #Se leen los resultados
        self.LockOutput.acquire()
        try:
            Boxes=self.boxes
            Classes=self.classes
            Scores=self.scores
            frame_rate=self.TFframe_rate_calc
        except:
            Boxes={}
            Clases={}
            Scores={}
            frame_rate=0
        self.LockOutput.release()
        
        #Se revisan todas las detecciones y se dibuja el recuadro si cumple con el minimo de confianza
        for i in range(len(Scores)):
            if ((Scores[i] > self.min_conf_threshold) and (Scores[i] <= 1.0)):

                #Se toman las coordenadas y se dibuja la caja
                #Se corrige si las cajas tienen limites fuera de la imagen, suele ocurrir si una deteccion se realiza cerca del borde
                ymin = int(max(1,(Boxes[i][0] * self.imH)))
                xmin = int(max(1,(Boxes[i][1] * self.imW)))
                ymax = int(min(self.imH,(Boxes[i][2] * self.imH)))
                xmax = int(min(self.imW,(Boxes[i][3] * self.imW)))
            
                cv2.rectangle(imagen, (xmin,ymin), (xmax,ymax), (10, 255, 0), 2)

                #Se escriben las etiquetas
                #Se busca la etiqueta usando el indice de la etiqueta
                object_name = self.labels[int(Classes[i])] 
                #Se agrega el puntaje porcentual de deteccion
                label = '%s: %d%%' % (object_name, int(Scores[i]*100)) 
                #Se configura la fuente
                labelSize, baseLine = cv2.getTextSize(label, cv2.FONT_HERSHEY_SIMPLEX, 0.7, 2)
                #Se asegura que no se esten escribiendo las etiquetas muy cerca del borde
                label_ymin = max(ymin, labelSize[1] + 10) 
                #Se dibuja el recuadro blanco donde iran las detecciones
                cv2.rectangle(imagen, (xmin, label_ymin-labelSize[1]-10), (xmin+labelSize[0], label_ymin+baseLine-10), (255, 255, 255), cv2.FILLED) 
                #Se escribe el texto de la etiqueta
                cv2.putText(imagen, label, (xmin, label_ymin-7), cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 0, 0), 2) 
        #Se agrega el mensaje de los FPS y se retorna la imagen procesada
        if not self.TensorflowActive:
            fps="FN;"
        else:
            fps= "F" + '{:.2f}'.format(frame_rate) + ";"
        return fps,imagen
        
    def IsReady(self):
        #Retorna si se ha terminado la deteccion anterior
        return self.Ready;
    def SetImage(self,imagen):
        #Se carga la imagen y se prepara la siguiente deteccion
        self.LockImage.acquire()
        self.Imagen=imagen
        self.Ready=False
        self.LockImage.release()
    def ToogleTensorflow(self):
        #Se activa o desactivan las detecciones
        self.TensorflowActive=not self.TensorflowActive
