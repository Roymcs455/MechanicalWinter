using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "Damage Config", menuName = "Guns/Damage Configuration", order = 1)]
public class DamageConfigurationSO : ScriptableObject
{
    public MinMaxCurve damageCurve;
    [SerializeField]
    public DamageTypes type;
    [SerializeField]
    public float knockbackForce= 5.0f;

    public bool isExplosive = true;
    public float explosionRadius = 1.0f;
    public LayerMask explosionLayerMask;

    public void Reset()
    {
        damageCurve.mode = ParticleSystemCurveMode.Curve;
    }

    public Damage GetDamage(float Distance = 0 )
    {
        
        Damage damage = new Damage(type, damageCurve.Evaluate(Distance, Random.value));

        return damage;
    }

}
