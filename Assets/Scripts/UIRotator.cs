using UnityEngine;

public class UIRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 12f;
    private RectTransform rotatingTransform;

    private void Awake()
    {
        rotatingTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rotatingTransform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }
}
