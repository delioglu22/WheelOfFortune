using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI; 
using DG.Tweening;
using TMPro;

public class WheelController : MonoBehaviour
{
    [SerializeField] private Button spinButton;
    [SerializeField] private RectTransform wheelBase;
    [Header("Spin Settings")]
    [SerializeField] private float spinDuration = 3f; 
    [SerializeField] private int spinLoops = 5;
    private bool isSpinning = false;
    [Header("Data")]
    public WheelData activeWheelData;
    public GameObject slicePrefab;
    public event System.Action<SliceData> OnSpinCompleted;
    [Header("UI Visuals")]
    [SerializeField] private Image wheelBaseImage;
    [SerializeField] private Image indicatorImage;
    [SerializeField] private Image panelZoneImage;
    


    private void OnValidate()
    {
        if(spinButton == null)
        spinButton = GetComponentInChildren<Button>();

        if(wheelBase == null)
        {
            Transform baseTransform = transform.Find("ui_image_wheel_base");
            if(baseTransform != null)
            wheelBase = baseTransform.GetComponent<RectTransform>();
        }
    }
    
    private void Awake() 
    {
        if (spinButton != null)
        spinButton.onClick.AddListener(OnSpinButtonClicked);
    }
    private void Start()
    {
    }

    public void GenerateWheel()
    {
        if (activeWheelData.wheelBaseSprite != null)
        {
            wheelBase.GetComponent<Image>().sprite = activeWheelData.wheelBaseSprite;
        }
        if (indicatorImage != null && activeWheelData.indicatorSprite != null)
        {
            indicatorImage.sprite = activeWheelData.indicatorSprite;
        }
        if (panelZoneImage != null && activeWheelData.panelZoneSprite != null)
        {
            panelZoneImage.sprite = activeWheelData.panelZoneSprite;
        }
        
        foreach (Transform child in wheelBase)
        {
            Destroy(child.gameObject);
        }
        int totalSlices = activeWheelData.slices.Count;
        float anglePerSlice = 360f / totalSlices;

        for(int i = 0; i < totalSlices; i++)
        {
            GameObject newSlice = Instantiate(slicePrefab, wheelBase);
            float angle = -anglePerSlice * i;
            newSlice.transform.localRotation = Quaternion.Euler(0, 0, angle);
            float radius = 145f;    

            float posAngle = anglePerSlice * i;
            float x = Mathf.Sin(posAngle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Cos(posAngle * Mathf.Deg2Rad) * radius;
            newSlice.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            SliceData data = activeWheelData.slices[i];
            Image iconImage = newSlice.GetComponent<Image>();
            if (iconImage != null && data.icon != null)
                iconImage.sprite = data.icon;
            TextMeshProUGUI amountText = newSlice.GetComponentInChildren<TextMeshProUGUI>();
            if (amountText != null)
            {
                if (data.amount < 2)
                    amountText.text = "";
                else
                
                    amountText.text = "x" + data.amount.ToString();
            }
        }
    }

    private void OnSpinButtonClicked()
    {
        if(isSpinning) return;

        isSpinning = true;
        spinButton.interactable = false;

        int randomSliceIndex = Random.Range(0, activeWheelData.slices.Count);
        float spinAngle = 360f / 8f;

        float targetAngle = -spinAngle * randomSliceIndex;
        float totalRotation = targetAngle - (spinLoops * 360f);
        
        wheelBase.DORotate(new Vector3(0, 0, totalRotation), spinDuration, RotateMode.FastBeyond360)
        .SetEase(Ease.OutCirc).OnComplete(() =>
        {
                isSpinning = false;
                spinButton.interactable = true;

                SliceData wonSlice = activeWheelData.slices[randomSliceIndex];
                OnSpinCompleted?.Invoke(wonSlice);
        });
    }

     

}
