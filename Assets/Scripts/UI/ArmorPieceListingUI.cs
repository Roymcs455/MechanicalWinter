using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArmorPieceListingUI : MonoBehaviour
{
    public ArmorPieceSO armorPieceSO;
    public TMP_Text partName, energyRes, explosiveRes, antimatRes, weight;
    public Button button;

    public void SetArmorPieceSO (ArmorPieceSO apSO)
    {
        armorPieceSO = apSO;
        partName.text = armorPieceSO.pieceName;
        energyRes.text = armorPieceSO.energyResistance.ToString("0.00");
        explosiveRes.text = armorPieceSO.explosiveResistance.ToString("0.00");
        antimatRes.text = armorPieceSO.antiMaterialResistance.ToString("0.00");
        weight.text = armorPieceSO.weight.ToString("0.00");
    }
    public void SetArmorPieceSO()
    {
        partName.text = armorPieceSO.pieceName;
        energyRes.text = armorPieceSO.energyResistance.ToString("0.00");
        explosiveRes.text = armorPieceSO.explosiveResistance.ToString("0.00");
        antimatRes.text = armorPieceSO.antiMaterialResistance.ToString("0.00");
        weight.text = armorPieceSO.weight.ToString("0.00");
    }
    public void Awake()
    {
        button = GetComponent<Button>();
    }
    public void Start()
    {
        if (button == null)
        {
            Debug.Log("Boton null en start");
        }
        //button.onClick.AddListener(Func);
        SetArmorPieceSO();
    }
    public void Func()
    {
        Debug.Log("Blabla");
    }
}
