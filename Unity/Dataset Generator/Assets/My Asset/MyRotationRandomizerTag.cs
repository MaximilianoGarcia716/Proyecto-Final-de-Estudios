using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;

[RequireComponent(typeof(Transform))] //Solo pueden usarse en objetos que contengan el tag

public class MyRotationRandomizerTag : RandomizerTag
{
    public float minAngle;
    public float maxAngle;
    public float minScale;
    public float maxScale;

    public void SetRotation(float RotationX,float RotationY,float RotationZ, float Scale)
    {
        //Se cambia la rotación del objeto y su escala en base a lo que deseamos
        var tagRot = GetComponent<Transform>();
        float QuatX = RotationX * (maxAngle - minAngle) + minAngle;
        float QuatY = RotationY * (maxAngle - minAngle) + minAngle;
        float QuatZ = RotationZ * (maxAngle - minAngle) + minAngle;
        float newScale = Scale * (maxScale - minScale) + minScale;
        tagRot.eulerAngles = new Vector3(QuatX, QuatY, QuatZ);
        tagRot.transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}

[Serializable]
[AddRandomizerMenu("MyRotationRandomizer")]
public class MyRotationRandomizer : Randomizer
{
    // Se elige un valor entre 0 y 1
    public FloatParameter RotationX = new() { value = new UniformSampler(0, 1) };
    public FloatParameter RotationY = new() { value = new UniformSampler(0, 1) };
    public FloatParameter RotationZ = new() { value = new UniformSampler(0, 1) };
    public FloatParameter Scale = new() { value = new UniformSampler(0, 1) };


    // Se corre en cada iteracion
    protected override void OnIterationStart()
    {
        // Se buscan todos los objetos con el tag
        var tags = tagManager.Query<MyRotationRandomizerTag>();
        foreach (var tag in tags)
        {
            // Se toma un nimero aleatorio entre 0 y 1 para realizar la rotación
            tag.SetRotation(RotationX.Sample(), RotationY.Sample(), RotationZ.Sample(),Scale.Sample());
        }
    }
}