using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;

[RequireComponent(typeof(Light))] //Solo puede agregarse en objetos con una luz
public class MyLightRandomizerTag : RandomizerTag
{
    public float minIntensity;
    public float maxIntensity;

    public void SetIntensity(float rawIntensity)
    {
        //Se cambia la intensidad de la luz
        var tagLight = GetComponent<Light>();
        var scaledIntensity = rawIntensity * (maxIntensity - minIntensity) + minIntensity;
        tagLight.intensity = scaledIntensity;
    }
}

[Serializable]
[AddRandomizerMenu("MyLightRandomizer")]
public class MyLightRandomizer : Randomizer
{
    // Se eligen valores entre 0  y 1
    public FloatParameter lightIntensity = new() { value = new UniformSampler(0, 1) };
    public ColorRgbParameter color;


    // Se corre en cada iteracion
    protected override void OnIterationStart()
    {
        // Toma todos los objetos con el tag en la escena
        var tags = tagManager.Query<MyLightRandomizerTag>();
        foreach (var tag in tags)
        {
            //Toma la luz en el objeto
            var tagLight = tag.GetComponent<Light>();
            tagLight.color = color.Sample();
            //Cambia la intensidad
            tag.SetIntensity(lightIntensity.Sample());
        }
    }
}