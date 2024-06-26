using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Lector_USB : MonoBehaviour
{
    Controles Control;

    private string Motores;
    public string ComunicaciónMensaje;
    int Stepper1;
    int Stepper2;
    int Stepper3;
    int Servo;
    bool ReleLeido;
    bool TensorflowLeido;

    public Vector2 IStick, DStick;
    public float A;
    public float Y;
    public float LT;
    public float LB;
    public float RT;
    public float buttonStart;
    public float Select;
    public Vector2 Dpad;
    public float PrevA = 0;
    public float PrevY=0;
    public float PrevLT = 0;
    public float PrevLB = 0;
    public float PrevRT = 0;
    public float PrevbuttonStart = 0;
    public Vector2 PrevDpad;
    public int Interfaz;

    // Start is called before the first frame update
    void Start()
    {
        ReleLeido = true;
        TensorflowLeido = true;
        Control = new Controles();
        //Se prepara la lectura del control y sus variables
        Control.Deteccion.Enable();
        Control.Deteccion.LeftAnalogStick.performed += Contexto => IStick = Contexto.ReadValue<Vector2>();
        Control.Deteccion.LeftAnalogStick.canceled += Contexto => IStick = Vector2.zero;
        Control.Deteccion.RightAnalogStick.performed += Contexto => DStick = Contexto.ReadValue<Vector2>();
        Control.Deteccion.RightAnalogStick.canceled += Contexto => DStick = Vector2.zero;
        Control.Deteccion.A.performed += Contexto => A = Contexto.ReadValue<float>();
        Control.Deteccion.A.canceled += Contexto => A = 0f;
        Control.Deteccion.Y.performed += Contexto => Y = Contexto.ReadValue<float>();
        Control.Deteccion.Y.canceled += Contexto => Y = 0f;
        Control.Deteccion.LT.performed += Contexto => LT = Contexto.ReadValue<float>();
        Control.Deteccion.LT.canceled += Contexto => LT = 0f;
        Control.Deteccion.LB.performed += Contexto => LB = Contexto.ReadValue<float>();
        Control.Deteccion.LB.canceled += Contexto => LB = 0f;
        Control.Deteccion.RT.performed += Contexto => RT = Contexto.ReadValue<float>();
        Control.Deteccion.RT.canceled += Contexto => RT = 0f;
        Control.Deteccion.Select.performed += Contexto => Select = Contexto.ReadValue<float>();
        Control.Deteccion.Select.canceled += Contexto => Select = 0f;
        Control.Deteccion.Start.performed += Contexto => buttonStart = Contexto.ReadValue<float>();
        Control.Deteccion.Start.canceled += Contexto => buttonStart = 0f;
        Control.Deteccion.Dpad.performed += Contexto => Dpad = Contexto.ReadValue<Vector2>();
        Control.Deteccion.Dpad.canceled += Contexto => Dpad = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        CrearMensaje();
        if (A == 1 && PrevA==0 || RT >= 0.5 && PrevRT < 0.5)
        {
            CambiarInterfaz();
        }
        if (Select == 1)
        {
            Quit();
        }
        UpdateButtons();
    }

    void StepperCalculate()
    {
        float Motor1 = IStick.x - DStick.x;
        float Motor2 = -IStick.y - IStick.x/2 - DStick.x;
        float Motor3 = IStick.y  - IStick.x/2 - DStick.x;
        Servo = (int)(-90*DStick.y);        // el - es porque el servo está instalado al revez de lo que se quiere
        if(Motor1 * Motor1 > 1 || Motor2 * Motor2 > 1 || Motor3 * Motor3 > 1)
        {
            if(Motor1*Motor1>Motor2*Motor2 && Motor1*Motor1 > Motor3*Motor3)
            {
                Motor1 = Motor1 / Mathf.Abs(Motor1);
                Motor2 = Motor2 / Mathf.Abs(Motor1);
                Motor3 = Motor3 / Mathf.Abs(Motor1);
            }
            if(Motor2 * Motor2 > Motor1 * Motor1 && Motor2 * Motor2 > Motor3 * Motor3)
            {
                Motor1 = Motor1 / Mathf.Abs(Motor2);
                Motor2 = Motor2 / Mathf.Abs(Motor2);
                Motor3 = Motor3 / Mathf.Abs(Motor2);
            }
            if(Motor3 * Motor3 > Motor1 * Motor1 && Motor3 * Motor3 > Motor2 * Motor2)
            {
                Motor1 = Motor1 / Mathf.Abs(Motor3);
                Motor2 = Motor2 / Mathf.Abs(Motor3);
                Motor3 = Motor3 / Mathf.Abs(Motor3);
            }
        }
        Stepper1 = (int)(500 * Motor1);
        Stepper2 = (int)(500 * Motor2);
        Stepper3 = (int)(500 * Motor3);
    }

    public string LeerComando()
    {
        if (!ReleLeido)
        {
            ReleLeido = true;
        }
        else
        {
            if (!TensorflowLeido)
            {
                TensorflowLeido = true;
            }
        }
        return ComunicaciónMensaje;
    }

    void CrearMensaje()
    {
        //Se prepara el mensaje dando prioridad al relé y luego a desactivar el tensorflow
        if (buttonStart == 1 && PrevbuttonStart == 0)
        {
            GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().MutexControl.WaitOne();
            ComunicaciónMensaje = "R;";
            ReleLeido = false;
            GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().MutexControl.ReleaseMutex();
        }
        else if (ReleLeido && Y == 1 && PrevY == 0)
        {
            GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().MutexControl.WaitOne();
            ComunicaciónMensaje = "T;";
            TensorflowLeido = false;
            GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().MutexControl.ReleaseMutex();
        }
        else if (ReleLeido && TensorflowLeido)
        {
            StepperCalculate();
            GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().MutexControl.WaitOne();
            ComunicaciónMensaje = "W" + Stepper1.ToString() + "," + Stepper2.ToString() + "," + Stepper3.ToString() + "," + Servo.ToString() + ";";
            GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().MutexControl.ReleaseMutex();
        }
    }

    void CambiarInterfaz()
    {
        //Se cambia entre las interfaces posibles
        string Interfaz=GameObject.Find("Interfaces").GetComponent<Interfaz>().UI;
        if (A == 1)
        {
            if (Interfaz == "Base")
            {
                GameObject.Find("Interfaces").GetComponent<Interfaz>().UI = "Camara";
            }
            else
            {
                GameObject.Find("Interfaces").GetComponent<Interfaz>().UI = "Base";
            }
        }
        else
        {
            if (Interfaz == "Base")
            {
                GameObject.Find("Interfaces").GetComponent<Interfaz>().UI = "Mapa";
            }
            else
            {
                GameObject.Find("Interfaces").GetComponent<Interfaz>().UI = "Base";
            }
        }
    }

    void UpdateButtons()
    {
        PrevY = Y;
        PrevA = A;
        PrevLB = LB;
        PrevLT = LT;
        PrevRT = RT;
        PrevbuttonStart = buttonStart;
        PrevDpad = Dpad;
    }

    void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
