using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("UI and References")]
    [SerializeField] private TextMeshProUGUI safeZoneText;
    [SerializeField] private TextMeshProUGUI superZoneText;
    [SerializeField] private Button leaveButton;
    [SerializeField] private WheelController wheelController;
    [SerializeField] private ZoneBarController zoneBarController;

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

    [Header("Reward Animation")]
    [SerializeField] private RectTransform rewardFlyStart;
    [SerializeField] private RectTransform rewardPopTarget;
    [SerializeField] private GameObject rewardFlyPrefab;
    [SerializeField] private float rewardPopScale = 4f;
    [SerializeField] private float rewardPopDuration = 0.35f;
    [SerializeField] private float rewardPopHold = 0.4f;
    [SerializeField] private int rewardFlyCount = 5;
    [SerializeField] private float rewardFlySpread = 60f;
    [SerializeField] private float rewardFlyStep = 0.1f;
    [SerializeField] private float rewardFlyDuration = 0.6f;
    [SerializeField] private float rewardBurstScale = 1.6f;
    [SerializeField] private float rewardFlyEndScale = 0.8f;
    [SerializeField] private float rewardCountDuration = 0.4f;



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

        if (zoneBarController == null)
            zoneBarController = FindObjectOfType<ZoneBarController>();

        if (rewardFlyStart == null)
            rewardFlyStart = GameObject.Find("ui_image_indicator")
                                ?.GetComponent<RectTransform>();

        if (rewardPopTarget == null)
            rewardPopTarget = GameObject.Find("ui_image_wheel_base")
                                ?.GetComponent<RectTransform>();
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
        if (zoneBarController != null)
            zoneBarController.GenerateZoneBar(currentZone);

        if (safeZoneText != null)
        {
            int nextSafeZone = (((currentZone - 1) / 5) + 1) * 5;
            safeZoneText.text = nextSafeZone.ToString();
        }

        if (superZoneText != null)
        {
            int nextSuperZone = (((currentZone - 1) / 30) + 1) * 30;
            superZoneText.text = nextSuperZone.ToString();
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
            PlayRewardFly(landedSlice);
            currentZone++;
            UpdateZone();
        }
    }

    private void PlayRewardFly(SliceData landedSlice)
    {
        Sprite collectedIcon = landedSlice.CollectedIcon;

        if (rewardFlyStart == null || rewardFlyPrefab == null)
        {
            AddCollectedAmount(collectedIcon, landedSlice.amount);
            return;
        }

        if (!collectedItems.ContainsKey(collectedIcon))
            collectedItems.Add(collectedIcon, 0);

        UpdateInventoryUI();

        Vector3 popPosition = rewardPopTarget != null ? rewardPopTarget.position : rewardFlyStart.position;

        GameObject rewardCard = CreateRewardIcon(landedSlice.icon, rewardFlyStart.position, 1f, landedSlice.GetAmountLabel());
        RectTransform cardTransform = rewardCard.GetComponent<RectTransform>();

        Sequence popSequence = DOTween.Sequence();
        popSequence.Append(cardTransform.DOMove(popPosition, rewardPopDuration).SetEase(Ease.OutQuad));
        popSequence.Join(cardTransform.DOScale(rewardPopScale, rewardPopDuration).SetEase(Ease.OutBack));
        popSequence.AppendInterval(rewardPopHold);
        int flyCount = Mathf.Clamp(landedSlice.amount, 1, rewardFlyCount);

        popSequence.OnComplete(() =>
        {
                Canvas.ForceUpdateCanvases();
                SpawnRewardBurst(landedSlice, flyCount, popPosition, GetInventorySlotPosition(collectedIcon));

                cardTransform.DOScale(0f, rewardPopDuration)
                .SetDelay(rewardFlyStep * flyCount)
                .SetEase(Ease.InBack)
                .OnComplete(() => Destroy(rewardCard));
        });
    }

    private void SpawnRewardBurst(SliceData landedSlice, int flyCount, Vector3 burstPosition, Vector3 slotPosition)
    {
        for (int i = 0; i < flyCount; i++)
        {
            bool isLastIcon = i == flyCount - 1;

            Vector3 spreadPosition = burstPosition + (Vector3)(Random.insideUnitCircle * rewardFlySpread);
            GameObject flyingIcon = CreateRewardIcon(landedSlice.CollectedIcon, burstPosition, rewardBurstScale, "");
            RectTransform iconTransform = flyingIcon.GetComponent<RectTransform>();

            Sequence flySequence = DOTween.Sequence();
            flySequence.AppendInterval(i * rewardFlyStep);
            flySequence.Append(iconTransform.DOMove(spreadPosition, rewardFlyStep * 2f).SetEase(Ease.OutQuad));
            flySequence.Append(iconTransform.DOMove(slotPosition, rewardFlyDuration).SetEase(Ease.InBack));
            flySequence.Join(iconTransform.DOScale(rewardFlyEndScale, rewardFlyDuration).SetEase(Ease.InQuad));
            flySequence.OnComplete(() =>
            {
                    Destroy(flyingIcon);

                    if (isLastIcon)
                        AddCollectedAmount(landedSlice.CollectedIcon, landedSlice.amount);
            });
        }
    }

    private GameObject CreateRewardIcon(Sprite icon, Vector3 position, float scale, string amountLabel)
    {
        GameObject rewardIcon = Instantiate(rewardFlyPrefab, inventoryContent.root);
        RectTransform iconTransform = rewardIcon.GetComponent<RectTransform>();
        iconTransform.position = position;
        iconTransform.localScale = Vector3.one * scale;
        iconTransform.SetAsLastSibling();

        Image iconImage = rewardIcon.GetComponent<Image>();
        if (iconImage != null) iconImage.sprite = icon;

        TextMeshProUGUI amountText = rewardIcon.GetComponentInChildren<TextMeshProUGUI>();
        if (amountText != null) amountText.text = amountLabel;

        return rewardIcon;
    }

    private void AddCollectedAmount(Sprite icon, int amount)
    {
        if (!collectedItems.ContainsKey(icon))
        {
            collectedItems.Add(icon, amount);
            UpdateInventoryUI();
            return;
        }

        int startAmount = collectedItems[icon];
        collectedItems[icon] += amount;

        int slotIndex = GetInventorySlotIndex(icon);
        if (slotIndex >= inventoryContent.childCount)
            return;

        TextMeshProUGUI amountText = inventoryContent.GetChild(slotIndex).GetComponentInChildren<TextMeshProUGUI>();
        if (amountText == null)
            return;

        int targetAmount = collectedItems[icon];
        DOTween.To(() => startAmount, countedAmount => amountText.text = "x" + countedAmount.ToString(), targetAmount, rewardCountDuration)
        .SetEase(Ease.OutQuad)
        .SetLink(amountText.gameObject);
    }

    private int GetInventorySlotIndex(Sprite icon)
    {
        int slotIndex = 0;
        foreach (var item in collectedItems)
        {
            if (item.Key == icon)
                return slotIndex;

            slotIndex++;
        }

        return slotIndex;
    }

    private Vector3 GetInventorySlotPosition(Sprite icon)
    {
        int slotIndex = GetInventorySlotIndex(icon);
        if (slotIndex < inventoryContent.childCount)
            return inventoryContent.GetChild(slotIndex).position;

        return inventoryContent.position;
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