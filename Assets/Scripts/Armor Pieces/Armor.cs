using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Armor : MonoBehaviour, IAgent
{
    public ArmorPiece[] armorPieces;
    
    private float[] armorHealth;
    //Este arreglo guarda el id del ultimo objeto de daño a cada pieza.
    private bool[] pieceDamagedThisFrame;
    public ProxySet proxySet;

    private void Awake()
    {
        LoadProxySet();
    }
    private void Start()
    {
        armorHealth = new float[(int)ChasisPart.TOTAL];
        pieceDamagedThisFrame = new bool[(int)ChasisPart.TOTAL];
        foreach (ArmorPiece armorPiece in armorPieces)
        {
            armorPiece.OnArmorReceiveDamage += ArmorPiece_DamageEvent;
            ArmorPieceSO pieceSO = armorPiece.GetArmorPieceSO();
            if(pieceSO.pieceHealth != 0)
                armorHealth[(int)pieceSO.chasisPart] = pieceSO.pieceHealth;
        }
    }
    private void FixedUpdate()
    {
        //Reiniciando el arreglo para las piezas, sirve para no hacer doble daño en hitbox compuestos.
        System.Array.Clear(pieceDamagedThisFrame, 0, pieceDamagedThisFrame.Length);
    }
    private void ArmorPiece_DamageEvent(object sender, ArmorPiece.ArmorPieceEventArgs e)
    {
        Debug.Log($"armorPiece damaging: {e.chasisPart} Applied Damage: {e.appliedDamage}");
        if (!pieceDamagedThisFrame[(int)e.chasisPart])
        {
            
            pieceDamagedThisFrame[(int)e.chasisPart] = true;
            armorHealth[(int)e.chasisPart] -= e.appliedDamage;
            if (armorHealth[(int)e.chasisPart] <= 0.0f )
            {
                DisableVisual(e.chasisPart);
                Debug.Log($"armorPiece {e.chasisPart} has been destroyed");
            }
        }
        if (armorHealth.All(health => health <= 0.0f))
        {
            Debug.Log("All pieces broken");
            gameObject.SendMessage("Die");
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

    public void Die()
    {
        gameObject.SendMessage("AgentIsDead");
    }
}
