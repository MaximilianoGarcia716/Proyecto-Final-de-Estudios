using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;
using System.IO;

public class MyActivationTag : Randomizer
{
    private int Iter = 0;
    public Texture2D SavedTexture = null;

    // Se corre en cada iteracion
    protected override void OnIterationStart()
    {
        //Encuentra en el escenario de simulación el activador de objetos a detectar y lo desactiva cuando no se requieren mas objetos
        //if (Iter > 0)
        if (Iter > 950)
        {
            GameObject.Find("Simulation Scenario").transform.Find("Foreground Objects").gameObject.SetActive(false);
        }
        Iter = Iter + 1;
        int RandomActivation = UnityEngine.Random.Range(0, 2);
        //Crea un numero aleatorio entero entre 0 y 2, sin tener en cuenta el 2 y si es 1 activa la pared con una textura aleatoria
        if(RandomActivation==1)
        {
            if(!GameObject.Find("Fondo").GetComponent<Renderer>().isVisible)
            {
                GameObject.Find("Fondo").GetComponent<Renderer>().enabled = true;
            }
            int ImageNum= UnityEngine.Random.Range(0, 71);
            string ImageString = "C:/Users/MAXI/Dropbox/Proyecto final de estudios/Unity/Dataset Generator/Assets/My Asset/Fondos/" + ImageNum.ToString() + ".png";
            Texture2D Textura = new Texture2D(640,480);
            byte[] ImagenBytes = File.ReadAllBytes(ImageString);
            Textura.LoadImage(ImagenBytes);
            if (Textura.height > 8)          //La textura de error es de 8X8
            {
                Textura.Apply();
                SavedTexture = Textura;
            }
            GameObject.Find("Fondo").GetComponent <Renderer>().material.mainTexture=SavedTexture;
        }
        else
        {
            if (GameObject.Find("Fondo").GetComponent<Renderer>().isVisible)
            {
                GameObject.Find("Fondo").GetComponent<Renderer>().enabled = false;
            }
        }
    }
}
