using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour
{
    
    LineRenderer Linea;
    string Navegacion;
    Vector3 Posicion;
    Vector3 PosicionPrev;
    float Rotacion=0f;
    float Escala = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Linea = GameObject.Find("Trayectoria").GetComponent<LineRenderer>();
        Linea.startWidth = 0.1f;
        Linea.endWidth = 0.1f;
        Linea.startColor = new Color(255, 0, 0);
        Linea.endColor = new Color(255, 0, 0);
        Posicion.x = 0;
        Posicion.y = 0;
        Posicion.z = 0;
        PosicionPrev.x = 0;
        PosicionPrev.y = 0;
        PosicionPrev.z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Navegacion = GameObject.Find("Raspberry_Comunicacion").GetComponent<Raspberry_Comunicacion>().NavegacionDatos;
        LeerNavegacion();
        float Modulo = (Posicion - PosicionPrev).magnitude;
        if (Modulo >= 0.01)
        {
            Linea.positionCount = Linea.positionCount + 1;
            Linea.SetPosition(Linea.positionCount - 1, Posicion);
            PosicionPrev = Posicion;
        }
        else
        {
            Posicion = PosicionPrev;
        }
        GameObject.Find("Robot").GetComponent<Transform>().position = Posicion;
        GameObject.Find("Robot").GetComponent<Transform>().eulerAngles = new Vector3(90.0f, Rotacion, 0.0f);
    }

    void LeerNavegacion()
    {
        //Se leen los datos de navegación para rotar el objeto robot y dibujar la línea
        string X, Z, R;
        if (!(Navegacion[0] == 'D' || Navegacion[0] == 'E'))
        {
            X = Navegacion.Substring(Navegacion.IndexOf("X") + 1, Navegacion.IndexOf("Y") - Navegacion.IndexOf("X") - 1);
            Z = Navegacion.Substring(Navegacion.IndexOf("Y") + 1, Navegacion.IndexOf("P") - Navegacion.IndexOf("Y") - 1);
            R = Navegacion.Substring(Navegacion.IndexOf("P") + 1, Navegacion.IndexOf("S") - Navegacion.IndexOf("P") - 1);
            Posicion.x = Escala*float.Parse(X,System.Globalization.CultureInfo.InvariantCulture);
            Posicion.z = Escala*float.Parse(Z, System.Globalization.CultureInfo.InvariantCulture);
            Rotacion = -float.Parse(R, System.Globalization.CultureInfo.InvariantCulture)+90.0f;
        }
    }
}
