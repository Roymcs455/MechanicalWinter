using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArmorPiece : MonoBehaviour
{
    private ChasisPart chasisPart;
    public PieceLocation location;

    [SerializeField]
    public ArmorPieceSO armorStats;
    public event EventHandler<ArmorPieceEventArgs> OnArmorReceiveDamage;

    private bool isPartAlive;
    [SerializeField]
    private bool renderArmorPiece = true;

    private GameObject armorVisual;
    
    public ArmorPieceSO GetArmorPieceSO (){ return armorStats; }

    private void Start()
    {
        isPartAlive = true;
        if(armorStats.prefab != null)
        {
            armorVisual = Instantiate(armorStats.prefab, transform);
            if (!renderArmorPiece)
                armorVisual.SetActive(false);
        }
        chasisPart = armorStats.chasisPart;
        if (armorStats == null)
        {
            Debug.LogError("Armor piece has no ArmorPieceSO!");
        }
    }
    public void DisableVisual()
    {
        if(renderArmorPiece)
            armorVisual.SetActive(false);
        isPartAlive=false;
    }
    public void EnableVisual() 
    { 
        armorVisual.SetActive(true); 
        isPartAlive=true; 
    }
    public void ReceiveDamage(Damage appliedDamage,int projectileID)
    {
        if (isPartAlive)
        {
            float multiplier;
            switch (appliedDamage.damageType)
            {
                case DamageTypes.EXPLOSIVE:
                    multiplier = armorStats.explosiveResistance;
                    break;
                case DamageTypes.ANTIMATERIAL:
                    multiplier = armorStats.antiMaterialResistance;
                    break;
                case DamageTypes.ENERGY:
                    multiplier = armorStats.energyResistance;
                    break;
                default:
                    multiplier = 0.0f;
                    break;

            }
            OnArmorReceiveDamage?.Invoke(this, new ArmorPieceEventArgs(chasisPart, appliedDamage.damage * multiplier, projectileID));
        }
        else
        {
            Debug.Log("Pieza Rota");
        }
    }
    public class ArmorPieceEventArgs : EventArgs
    {
        public ChasisPart chasisPart;
        public float appliedDamage;
        public int projectileID;
        public ArmorPieceEventArgs(ChasisPart chasisPart, float appliedDamage, int projectileID)
        {
            this.chasisPart = chasisPart;
            this.appliedDamage = appliedDamage;
            this.projectileID = projectileID;
        }
    }
}
