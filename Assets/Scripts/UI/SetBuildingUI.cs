using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetBuildingUI : MonoBehaviour
{

    [SerializeField]
    public Transform listedArmorPieces;
    [SerializeField]
    public GameObject prefabListItem;
    [SerializeField]
    public List<GameObject> instancesUIList;
    [SerializeField]
    private Button
        headButton,
        torsoButton,
        rightArmButton,
        leftArmButton,
        rightLegButton,
        leftLegButton,
        sensonButton,
        weaponButton
        ;
    [Header("Dropdown List")]
    [SerializeField]
    private TMP_Dropdown dropdown;
    [SerializeField]
    private List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

    
    private SetBuilder logic;
    // Start is called before the first frame update
    void Start()
    {
        logic = GetComponent<SetBuilder>();
        SetupPieceSelectionButton(headButton, ChasisPart.HEAD);
        SetupPieceSelectionButton(torsoButton, ChasisPart.TORSO);
        SetupPieceSelectionButton(rightArmButton, ChasisPart.ARM_R);
        SetupPieceSelectionButton(leftArmButton, ChasisPart.ARM_L);
        SetupPieceSelectionButton(rightLegButton, ChasisPart.LEG_R);
        SetupPieceSelectionButton(leftLegButton, ChasisPart.LEG_L);

    }
    private void SetupPieceSelectionButton(Button button,ChasisPart chasisPart)
    {
        button.onClick.AddListener(()=>logic.ListBy(chasisPart));
    }
    private void InstantiateButton(ArmorPieceSO apSO)
    {
        //Debug.Log($"apSO: {apSO.pieceName} {apSO.pieceLocation} {apSO.chasisPart}");
        GameObject newItem = Instantiate(prefabListItem, listedArmorPieces);
        ArmorPieceListingUI newItemUI = newItem.GetComponent<ArmorPieceListingUI>();
        if (newItemUI != null )
        {
            newItemUI.SetArmorPieceSO(apSO);
            instancesUIList.Add(newItem);
            if (newItemUI.button != null)
                newItemUI.button.onClick.AddListener(() => SetPiece(apSO));
            else
                Debug.Log("button null");
            Debug.Log("exiting");

        }

    }
    public void ShowButtons(ArmorPieceSO[] apSOArray)
    {
        ClearButtons();
        foreach (ArmorPieceSO item in apSOArray)
        {
            InstantiateButton(item);
        }
    }

    public void ClearButtons()
    {
        foreach (Transform item in listedArmorPieces)
        {
            item.gameObject.GetComponent<ArmorPieceListingUI>().button.onClick.RemoveAllListeners();
            Destroy(item.gameObject);
        }
        instancesUIList.Clear();
    }

    public void SetPiece(ArmorPieceSO apSO)
    {
        logic.SetPiece(apSO);
    }

    public void ListSets(ProxySet[] setList)
    {
        foreach(ProxySet item in setList)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(item.setName));
        }
        dropdown.AddOptions(options);
        dropdown.RefreshShownValue();
    }
    public void AddOptionDropdown(ProxySet proxySet)
    {
        dropdown.options.Add(new TMP_Dropdown.OptionData(proxySet.setName));
        dropdown.AddOptions(options);
        dropdown.RefreshShownValue();
    }
    public void DropdownValueChanged()
    {
        logic.ChangeActiveSet(dropdown.value);
    }
}
