using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI; 

public class WheelController : MonoBehaviour
{
    [SerializeField] private Button spinButton;
    [SerializeField] private RectTransform wheelBase;

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

    private void OnSpinButtonClicked()
    {
        Debug.Log("Spinnnnninngggg!!!");
    }

}
