using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail Configuration", menuName = "Guns/Gun Trail Configuration", order = 4)]
public class TrailConfigurationSO : ScriptableObject
{
    public Material material;
    public AnimationCurve widthCurve;
    public float duration = 0.5f;
    public float minVertexDistance = 0.1f;
    public Gradient Color;

    public float missDistance = 100f;
    public float simulationSpeed = 100f;
}
