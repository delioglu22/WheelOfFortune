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
    [SerializeField] private float windUpAngle = 18f;
    [SerializeField] private float windUpDuration = 0.25f;
    [SerializeField] private float settleAngle = 5f;
    [SerializeField] private float settleDuration = 0.35f;
    [Header("Indicator Tick")]
    [SerializeField] private float indicatorKickAngle = 16f;
    [SerializeField] private float indicatorReturnDuration = 0.12f;
    private float lastTickAngle;
    private bool isSpinning = false;
    [Header("Data")]
    public WheelData activeWheelData;
    public GameObject slicePrefab;
    public event System.Action<SliceData> OnSpinCompleted;
    [Header("UI Visuals")]
    [SerializeField] private Image indicatorImage;

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
        float spinAngle = 360f / activeWheelData.slices.Count;

        float targetAngle = spinAngle * randomSliceIndex;
        float currentAngle = wheelBase.localEulerAngles.z;
        float totalRotation = -((spinLoops * 360f) + Mathf.Repeat(currentAngle - targetAngle, 360f));

        lastTickAngle = currentAngle;

        Sequence spinSequence = DOTween.Sequence();
        spinSequence.Append(wheelBase.DOLocalRotate(new Vector3(0, 0, windUpAngle), windUpDuration, RotateMode.LocalAxisAdd)
        .SetEase(Ease.OutQuad));
        spinSequence.Append(wheelBase.DOLocalRotate(new Vector3(0, 0, totalRotation - windUpAngle - settleAngle), spinDuration, RotateMode.LocalAxisAdd)
        .SetEase(Ease.InOutQuad).OnUpdate(TickIndicator));
        spinSequence.Append(wheelBase.DOLocalRotate(new Vector3(0, 0, settleAngle), settleDuration, RotateMode.LocalAxisAdd)
        .SetEase(Ease.OutQuad));
        spinSequence.OnComplete(() =>
        {
                isSpinning = false;
                spinButton.interactable = true;

                SliceData wonSlice = activeWheelData.slices[randomSliceIndex];
                OnSpinCompleted?.Invoke(wonSlice);
        });
    }

    private void TickIndicator()
    {
        if (indicatorImage == null) return;

        float sliceAngle = 360f / activeWheelData.slices.Count;
        float currentAngle = wheelBase.localEulerAngles.z;
        float travelled = Mathf.Repeat(lastTickAngle - currentAngle, 360f);

        if (travelled < sliceAngle) return;

        lastTickAngle = Mathf.Repeat(lastTickAngle - sliceAngle * Mathf.Floor(travelled / sliceAngle), 360f);

        RectTransform indicatorTransform = indicatorImage.rectTransform;
        indicatorTransform.DOKill();
        indicatorTransform.localRotation = Quaternion.Euler(0f, 0f, indicatorKickAngle);
        indicatorTransform.DOLocalRotate(Vector3.zero, indicatorReturnDuration)
        .SetEase(Ease.OutBack);
    }

     

}
