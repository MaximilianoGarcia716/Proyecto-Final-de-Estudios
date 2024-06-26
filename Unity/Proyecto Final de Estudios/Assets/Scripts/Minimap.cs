using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera MinimapCam;
    private GameObject Robot;
    bool ColocarSenal = false;
    bool SenalElegida = false;
    int ObjetoSeleccionado = 0;
    public string Texto = "";
    Vector3 SenalPosicion, DefaultPosicion;
    public GameObject Biologico, Corrosivo, Inflamable, Radioactivo, Toxico;
    GameObject[] ObjetosLista;
    Quaternion Rotacion;
    void Start()
    {
        ObjetosLista = new GameObject[5];

        Rotacion =Quaternion.identity;
        SenalPosicion = new Vector3(0, 0, 0);
        DefaultPosicion= new Vector3(0, -9000, 0);
        MinimapCam = GameObject.Find("MapCamera").GetComponent<Camera>();
        Robot = GameObject.Find("Robot");
        IniciarLista();
        
    }

    // Update is called once per frame
    void Update()
    {
        MinimapCam.transform.position = new Vector3(Robot.transform.position.x, Robot.transform.position.y + 10f, Robot.transform.position.z);
        LeerBotones();
        if (ColocarSenal && !SenalElegida)
        {
            ElegirSenal();
        }
        if (ColocarSenal && SenalElegida)
        {
            MoverSenal();
        }
    }

    void IniciarLista()
    {
        //Se prepara la lista de objetos a clonar para ubicarlos
        ObjetosLista[0] = Instantiate(Biologico, DefaultPosicion, Rotacion);
        ObjetosLista[0].name = "Biologico";
        ObjetosLista[1] = Instantiate(Corrosivo, DefaultPosicion, Rotacion);
        ObjetosLista[1].name = "Corrosivo";
        ObjetosLista[2] = Instantiate(Inflamable, DefaultPosicion, Rotacion);
        ObjetosLista[2].name = "Inflamable";
        ObjetosLista[3] = Instantiate(Radioactivo, DefaultPosicion, Rotacion);
        ObjetosLista[3].name = "Radioactivo";
        ObjetosLista[4] = Instantiate(Toxico, DefaultPosicion, Rotacion);
        ObjetosLista[4].name = "Toxico";
    }
    void LeerBotones()
    {
        //Máquina de estados básica para definir que función se realiza
        float LT = GameObject.Find("Lector_USB").GetComponent<Lector_USB>().LT;
        float PrevLT = GameObject.Find("Lector_USB").GetComponent<Lector_USB>().PrevLT;
        float LB = GameObject.Find("Lector_USB").GetComponent<Lector_USB>().LB;
        float PrevLB = GameObject.Find("Lector_USB").GetComponent<Lector_USB>().PrevLB;
        if(LT >= 0.5 && PrevLT < 0.5)
        {
            
            if (ColocarSenal && !SenalElegida)
            {
                Texto = "Posicionar";
                SenalElegida = true;
            }
            else if (!ColocarSenal && !SenalElegida)
            {
                IniciarColocacion();
                Texto = "Seleccionar";
                ColocarSenal = true;
            }
            else
            {
                Texto = "";
                FijarSenal();
                ColocarSenal = false;
                SenalElegida = false;
            }
        }
        if(LB == 1 && PrevLB == 0)
        {
            Cancelar();
            Texto = "";
            ColocarSenal = false;
            SenalElegida = false;
        }
    }

    void IniciarColocacion()
    {
        //Inicia la colocación de una señal
        SenalPosicion = Robot.transform.position;
        SenalPosicion.y = SenalPosicion.y + 0.1f;
        ObjetosLista[ObjetoSeleccionado].GetComponent<Transform>().position = SenalPosicion;
    }

    void ElegirSenal()
    {
        //Se elige la señal, evitando órdenes contradictorias
        Vector2 Dpad = GameObject.Find("Lector_USB").GetComponent<Lector_USB>().Dpad;
        Vector2 PrevDpad = GameObject.Find("Lector_USB").GetComponent<Lector_USB>().PrevDpad;
        if ((Dpad.x*Dpad.y)>=0 && PrevDpad == Vector2.zero) 
        {
            if (Dpad.x > 0 || Dpad.y > 0)
            {
                ObjetosLista[ObjetoSeleccionado].GetComponent<Transform>().position = DefaultPosicion;
                ObjetoSeleccionado = ObjetoSeleccionado + 1;
                RevisarIndice();
                IniciarColocacion();
            }
            if (Dpad.x < 0 || Dpad.y < 0)
            {
                ObjetosLista[ObjetoSeleccionado].GetComponent<Transform>().position = DefaultPosicion;
                ObjetoSeleccionado = ObjetoSeleccionado - 1;
                RevisarIndice();
                IniciarColocacion();
            }
        }
    }

    void RevisarIndice()
    {
        //Si el índice se sale de la matriz se acomoda antes de accederse al array
        if (ObjetoSeleccionado == -1)
        {
            ObjetoSeleccionado = 4;
        }
        else if (ObjetoSeleccionado == 5)
        {
            ObjetoSeleccionado = 0;
        }
    }

    void MoverSenal()
    {
        Vector2 Dpad = GameObject.Find("Lector_USB").GetComponent<Lector_USB>().Dpad;
        if (Dpad.x > 0)
        {
            SenalPosicion.x = SenalPosicion.x + 0.1f;
            ObjetosLista[ObjetoSeleccionado].GetComponent<Transform>().position = SenalPosicion; 
        }
        if (Dpad.x < 0)
        {
            SenalPosicion.x = SenalPosicion.x - 0.1f;
            ObjetosLista[ObjetoSeleccionado].GetComponent<Transform>().position = SenalPosicion;
        }
        if (Dpad.y > 0)
        {
            SenalPosicion.z = SenalPosicion.z + 0.1f;
            ObjetosLista[ObjetoSeleccionado].GetComponent<Transform>().position = SenalPosicion;
        }
        if (Dpad.y < 0)
        {
            SenalPosicion.z = SenalPosicion.z - 0.1f;
            ObjetosLista[ObjetoSeleccionado].GetComponent<Transform>().position = SenalPosicion;
        }

    }

    void FijarSenal()
    {
        //Se fija la señal clonada debajo del plano del robot y se esconde la señal original
        SenalPosicion.y = SenalPosicion.y - 0.2f;
        Instantiate(ObjetosLista[ObjetoSeleccionado], SenalPosicion, Rotacion);
        ObjetosLista[ObjetoSeleccionado].GetComponent<Transform>().position = DefaultPosicion;
    }

    void Cancelar()
    {
        //Devuelve las señales a su posición oculta
        ObjetosLista[ObjetoSeleccionado].GetComponent<Transform>().position = DefaultPosicion;
    }

}
