using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interfaz : MonoBehaviour
{
    public Texture2D RaspiTexture = null;
    public Text StatusText,FPSText,PosicionText,ColocacionText;
    float FPSPrev = 0.0f;
    bool TFActive=false;
    string NavegacionText;
    public string UI;
    // Start is called before the first frame update
    void Start()
    {
        UI = "Base";
        StatusText = GameObject.Find("Status").GetComponent<Text>();
        FPSText = GameObject.Find("FPS").GetComponent<Text>();
        PosicionText= GameObject.Find("Posicion").GetComponent<Text>();
        ColocacionText = GameObject.Find("Colocacion").GetComponent<Text>();
        if (RaspiTexture == null)
        {
            RaspiTexture = new Texture2D(2, 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().MutexNavegacion.WaitOne();
        NavegacionText = GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().NavegacionDatos;
        GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().MutexNavegacion.ReleaseMutex();
        SelectUI();
        if (UI == "Base")
        {
            UpdateImage();
            UpdateStatusText();
            UpdateFPSText();
            UpdatePosicionText();
            UpdateColocacion();
        }
        if (UI == "Camara")
        {
            UpdateImage();
        }
    }

    void UpdateImage()
    {
        //Se leen los bytes de la imagen y se cargan en una textura
        if (GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().ImageIsReady())
        {
            Texture2D Texture = new Texture2D(2, 2);

            GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().MutexImagen.WaitOne();
            Texture.LoadImage(GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().BytesImagen);
            GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().MutexImagen.ReleaseMutex();
            if (Texture.height > 8)          //La textura de error es de 8X8
            {
                Texture.Apply();
                RaspiTexture = Texture;
            }
            if (UI == "Camara")
            {
                GameObject.Find("RaspiCamara2").GetComponent<RawImage>().texture = RaspiTexture;
            }
            if (UI == "Base")
            {
                GameObject.Find("RaspiCamara1").GetComponent<RawImage>().texture = RaspiTexture;
            }
        }
        else
            if (UI == "Camara")
        {
            GameObject.Find("RaspiCamara2").GetComponent<RawImage>().texture = RaspiTexture;
        }
        if (UI == "Base")
        {
            GameObject.Find("RaspiCamara1").GetComponent<RawImage>().texture = RaspiTexture;
        }
    }

    void UpdateStatusText()
    {
        bool Connected = GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().IsConnected;
        if (Connected)
        {
            StatusText.text = "Conexión: ON" + "\n";
            if (NavegacionText[0] == 'D' || NavegacionText[0] == 'E')
            {
                StatusText.text = StatusText.text + "Relé: OFF" + "\n";
            }
            else
            {
                StatusText.text = StatusText.text + "Relé: ON" + "\n";
            }
            if (!(NavegacionText[0] == 'E'))
            {
                if (NavegacionText.Substring(NavegacionText.IndexOf("F") + 1, 1) == "N")
                {
                    StatusText.text = StatusText.text + "Tensorflow: OFF";
                }
                else
                {
                    StatusText.text = StatusText.text + "Tensorflow: ON";
                }
            }
            else
            {
                StatusText.text = StatusText.text + "Tensorflow: OFF";
            }
        }
        else
        {
            StatusText.text= "Conexión: OFF" + "\n";
        }
    }

    void UpdateFPSText()
    {

        string FPS;
        if (!(NavegacionText[0] == 'E'))
        {
            if(!((NavegacionText.IndexOf(";") - NavegacionText.IndexOf("F") - 1)<0)){
                FPS = NavegacionText.Substring(NavegacionText.IndexOf("F") + 1, NavegacionText.IndexOf(";") - NavegacionText.IndexOf("F") - 1);
                if (FPS == "N")
                {
                    FPS = "_.__";
                }
            }
            else
            {
                FPS = "_.__";
            }
        }
        else
        {
            FPS = "_.__";
        }
        FPSText.text = "FPS: " + FPS + "\n";
        string Temp;
        if (NavegacionText[0] == 'D' || NavegacionText[0] == 'E')
        {
            Temp = "_.__";
        }
        else
        {
            Temp = NavegacionText.Substring(NavegacionText.IndexOf("C") + 1, NavegacionText.IndexOf("F") - NavegacionText.IndexOf("C")-1);
        }
        FPSText.text = FPSText.text + "Temp: " + Temp + "\n";
    }

    void UpdatePosicionText()
    {
        string X, Y, R, S;
        if (NavegacionText[0] == 'D' || NavegacionText[0] == 'E')
        {
            X = "_____.__";
            Y = "_____.__";
            R = "_____.__";
            S = "_____.__";
        }
        else
        {
            X = NavegacionText.Substring(NavegacionText.IndexOf("X") + 1, NavegacionText.IndexOf("Y") - NavegacionText.IndexOf("X") - 1);
            Y = NavegacionText.Substring(NavegacionText.IndexOf("Y") + 1, NavegacionText.IndexOf("P") - NavegacionText.IndexOf("Y") - 1);
            R = NavegacionText.Substring(NavegacionText.IndexOf("P") + 1, NavegacionText.IndexOf("S") - NavegacionText.IndexOf("P") - 1);
            S = NavegacionText.Substring(NavegacionText.IndexOf("S") + 1, NavegacionText.IndexOf("C") - NavegacionText.IndexOf("S") - 1);
        }
        PosicionText.text = "X: " + X + "\n";
        PosicionText.text = PosicionText.text + "Y: " + Y + "\n";
        PosicionText.text = PosicionText.text + "R: " + R + "\n";
        PosicionText.text = PosicionText.text + "S: " + S;
    }

    void SelectUI()
    {
        if (UI=="Base")
        {
            GameObject.Find("InterfazBase").GetComponent<Canvas>().enabled = true;
            GameObject.Find("InterfazCamara").GetComponent<Canvas>().enabled = false;
            GameObject.Find("InterfazMapa").GetComponent<Canvas>().enabled = false;
        }
        if (UI=="Camara")
        {
            GameObject.Find("InterfazBase").GetComponent<Canvas>().enabled = false;
            GameObject.Find("InterfazCamara").GetComponent<Canvas>().enabled = true;
            GameObject.Find("InterfazMapa").GetComponent<Canvas>().enabled = false;
        }
        if (UI=="Mapa")
        {
            GameObject.Find("InterfazBase").GetComponent<Canvas>().enabled = false;
            GameObject.Find("InterfazCamara").GetComponent<Canvas>().enabled = false;
            GameObject.Find("InterfazMapa").GetComponent<Canvas>().enabled = true;
        }
    }
    void UpdateColocacion()
    {
        ColocacionText.text = GameObject.Find("MapCamera").GetComponent<Minimap>().Texto;
    }
}
