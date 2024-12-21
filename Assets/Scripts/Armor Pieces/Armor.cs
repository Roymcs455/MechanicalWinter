using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    public ArmorPiece[] armorPieces;
    private float chasisHealth;
    private float[] armorHealth;
    //Este arreglo guarda el id del ultimo objeto de daño a cada pieza.
    private int[] lastDamagingID;
    public ProxySet proxySet;

    private void Awake()
    {
        LoadProxySet();
    }
    private void Start()
    {
        chasisHealth = 100;
        armorHealth = new float[(int)ChasisPart.TOTAL];
        lastDamagingID = new int[(int)ChasisPart.TOTAL];
        foreach (ArmorPiece armorPiece in armorPieces)
        {
            armorPiece.OnArmorReceiveDamage += ArmorPiece_DamageEvent;
            ArmorPieceSO pieceSO = armorPiece.GetArmorPieceSO();
            if(pieceSO.pieceHealth != 0)
                armorHealth[(int)pieceSO.chasisPart] = pieceSO.pieceHealth;
        }
    }

    private void ArmorPiece_DamageEvent(object sender, ArmorPiece.ArmorPieceEventArgs e)
    {
        Debug.Log($"armorPiece damaging: {e.chasisPart} Applied Damage: {e.appliedDamage}");
        if (e.projectileID != lastDamagingID[(int)e.chasisPart])
        {
            lastDamagingID[(int)e.chasisPart] = e.projectileID;
            armorHealth[(int)e.chasisPart] -= e.appliedDamage;
            if (armorHealth[(int)e.chasisPart] <= 0.0f )
            {
                DisableVisual(e.chasisPart);
                Debug.Log($"armorPiece {e.chasisPart} has been destroyed");
            }
        }
        
    }

    private void DisableVisual(ChasisPart part)
    {
        foreach (ArmorPiece piece in armorPieces)
        {
            if (piece.GetArmorPieceSO().chasisPart == part)
            {
                piece.DisableVisual();
            }
        }
    }
    public void LoadProxySet()
    {
        foreach(ArmorPiece piece in armorPieces)
        {
            foreach(ArmorPieceSO apSO in proxySet.components)
            {
                if (piece.location == apSO.pieceLocation)
                {
                    piece.armorStats = apSO;
                }
            }
        }
    }
}
