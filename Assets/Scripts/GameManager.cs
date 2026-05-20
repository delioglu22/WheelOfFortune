using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("UI and References")]
    [SerializeField] private TextMeshProUGUI zoneText;
    [SerializeField] private TextMeshProUGUI safeZoneText;
    [SerializeField] private TextMeshProUGUI superZoneText;
    [SerializeField] private Button leaveButton;
    [SerializeField] private WheelController wheelController;
    private int totalReward = 0;

    [Header("Wheel Data")]
    [SerializeField] private WheelData normalWheel;
    [SerializeField] private WheelData safeWheel;
    [SerializeField] private WheelData superWheel;

    [Header("Bomb Panel")]
    [SerializeField] private GameObject uiPanelBomb;
    [SerializeField] private Button uiButtonGiveUp;
    [SerializeField] private Button uiButtonReviveGold;
    [SerializeField] private Button uiButtonReviveAd;

    [Header("Inventory System")]
    [SerializeField] private Transform inventoryContent; 
    [SerializeField] private GameObject inventoryItemPrefab; 
    private Dictionary<Sprite, int> collectedItems = new Dictionary<Sprite, int>();



    private int currentZone = 1;

    private void OnValidate()
    {
        if (leaveButton == null)
            leaveButton = transform.Find(".../.../ui_button_leave")
                                ?.GetComponent<Button>();
        if (uiButtonGiveUp == null)
            uiButtonGiveUp = transform.Find(".../.../ui_button_give_up")
                                    ?.GetComponent<Button>();
        if (uiButtonReviveGold == null)
            uiButtonReviveGold = transform.Find(".../.../ui_button_revive_gold")
                                    ?.GetComponent<Button>();
        if (uiButtonReviveAd == null)
            uiButtonReviveAd = transform.Find(".../.../ui_button_revive_ad")
                                    ?.GetComponent<Button>();
        if (uiPanelBomb == null)
            uiPanelBomb = transform.root.Find("Canvas/ui_panel_bomb")
                                ?.gameObject;
        
        if (safeZoneText == null)
            safeZoneText = transform.root.Find("Canvas/ui_container_wheel/RightSection Panel/ui_panel_milestones/ui_safezone/Text (TMP)")
                                ?.GetComponent<TextMeshProUGUI>();
                                
        if (superZoneText == null)
            superZoneText = transform.root.Find("Canvas/ui_container_wheel/RightSection Panel/ui_panel_milestones/ui_superzone/Text (TMP)")
                                ?.GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        if (leaveButton != null)
            leaveButton.onClick.AddListener(OnLeaveClicked);

        if (uiButtonGiveUp != null)
            uiButtonGiveUp.onClick.AddListener(OnGiveUpClicked);

        if (uiButtonReviveGold != null)
            uiButtonReviveGold.onClick.AddListener(OnReviveClicked);

        if (uiButtonReviveAd != null)
            uiButtonReviveAd.onClick.AddListener(OnReviveClicked);

        if (uiPanelBomb != null)
        {
            uiPanelBomb.transform.SetAsLastSibling();
            uiPanelBomb.SetActive(false);
        }

        wheelController.OnSpinCompleted += HandleSpinResult;

        UpdateZone();
    }

    private void UpdateZone()
    {
        zoneText.text = "ZONE " + currentZone;

        if (safeZoneText != null)
        {
            int nextSafeZone = (((currentZone - 1) / 5) + 1) * 5;
            safeZoneText.text = "SAFE ZONE " + nextSafeZone;
        }

        if (superZoneText != null)
        {
            int nextSuperZone = (((currentZone - 1) / 30) + 1) * 30;
            superZoneText.text = "SUPER ZONE " + nextSuperZone;
        }

        if (currentZone % 30 == 0)
        {
            wheelController.activeWheelData = superWheel;
            leaveButton.interactable = true;
        }
        else if (currentZone % 5 == 0)
        {
            wheelController.activeWheelData = safeWheel;
            leaveButton.interactable = true;
        }
        else
        {
            wheelController.activeWheelData = normalWheel;
            leaveButton.interactable = false;
        }

        wheelController.GenerateWheel();
    }

    private void HandleSpinResult(SliceData landedSlice)
    {
        if (landedSlice.sliceType == SliceType.Bomb)
        {
            if (uiPanelBomb != null)
                uiPanelBomb.SetActive(true);
            else
                Debug.LogError("Bomba paneli (uiPanelBomb) GameManager'da atanmamış!");
        }
        else
        {
            if (collectedItems.ContainsKey(landedSlice.icon))
                collectedItems[landedSlice.icon] += landedSlice.amount;
            else
                collectedItems.Add(landedSlice.icon, landedSlice.amount);
            
            UpdateInventoryUI(); 
            currentZone++;
            UpdateZone();
        }
    }

    private void UpdateInventoryUI()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in collectedItems)
        {
            GameObject newItem = Instantiate(inventoryItemPrefab, inventoryContent);
            
            // Resmi (Key) ata
            Image iconImage = newItem.GetComponentInChildren<Image>();
            if (iconImage != null) iconImage.sprite = item.Key;
            
            // Miktarı (Value) ata
            TextMeshProUGUI amountText = newItem.GetComponentInChildren<TextMeshProUGUI>();
            if (amountText != null) amountText.text = "x" + item.Value.ToString();
        }
    }
    
    private void ClearInventory()
    {
        collectedItems.Clear(); 
        UpdateInventoryUI(); 
    }

    private void OnGiveUpClicked()
    {
        uiPanelBomb.SetActive(false);
        ClearInventory();
        currentZone = 1;
        UpdateZone();
    }

    private void OnReviveClicked()
    {
        uiPanelBomb.SetActive(false);
        currentZone++;
        UpdateZone();
    }

    private void OnLeaveClicked()
    {
        ClearInventory();
        currentZone = 1;
        UpdateZone();
    }
}