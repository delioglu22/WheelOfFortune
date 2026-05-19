using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI and References")]
    [SerializeField] private TextMeshProUGUI zoneText;
    [SerializeField] private TextMeshProUGUI totalRewardText;
    [SerializeField] private Button leaveButton;
    [SerializeField] private WheelController wheelController;
    private int totalReward = 0;

    [Header("Wheel Data")]
    [SerializeField] private WheelData normalWheel;
    [SerializeField] private WheelData safeWheel;
    [SerializeField] private WheelData superWheel;

    private int currentZone = 1;

    private void Start()
    {
        if (leaveButton != null)
            leaveButton.onClick.AddListener(OnLeaveClicked);

        wheelController.OnSpinCompleted += HandleSpinResult;

        UpdateZone();
    }

    private void UpdateZone()
    {
        zoneText.text = "ZONE " + currentZone;

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
            totalReward = 0; 
            totalRewardText.text = "Total Reward: 0";
            currentZone = 1;
            UpdateZone();
        }
        else
        {
            totalReward += landedSlice.amount; 
            totalRewardText.text = "Total Reward: " + totalReward; 
            currentZone++;
            UpdateZone();
        }
    }

    private void OnLeaveClicked()
    {
        totalReward = 0;
        totalRewardText.text = "Total Reward: 0";
        currentZone = 1;
        UpdateZone();
    }
}
