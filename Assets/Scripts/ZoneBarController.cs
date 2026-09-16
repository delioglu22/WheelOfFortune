using UnityEngine;
using TMPro;
using DG.Tweening;

public class ZoneBarController : MonoBehaviour
{
    [SerializeField] private RectTransform zoneContent;
    [SerializeField] private GameObject zoneItemPrefab;

    [Header("Layout")]
    [SerializeField] private float itemSpacing = 90f;
    [SerializeField] private int passedCount = 4;
    [SerializeField] private int comingCount = 7;

    [Header("Slide")]
    [SerializeField] private float slideDuration = 0.4f;

    [Header("Colors")]
    [SerializeField] private Color passedColor = new Color(1f, 1f, 1f, 0.35f);
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color safeColor = new Color(0.45f, 0.9f, 0.2f);
    [SerializeField] private Color superColor = new Color(1f, 0.72f, 0.2f);

    private void OnValidate()
    {
        if (zoneContent == null)
        {
            Transform contentTransform = transform.Find("ui_mask_zone/ui_content_zone");
            if (contentTransform != null)
            zoneContent = contentTransform.GetComponent<RectTransform>();
        }
    }

    public void GenerateZoneBar(int currentZone)
    {
        foreach (Transform child in zoneContent)
        {
            Destroy(child.gameObject);
        }

        int firstZone = Mathf.Max(1, currentZone - passedCount);
        int lastZone = currentZone + comingCount;

        for (int zone = firstZone; zone <= lastZone; zone++)
        {
            GameObject newItem = Instantiate(zoneItemPrefab, zoneContent);
            RectTransform itemTransform = newItem.GetComponent<RectTransform>();
            itemTransform.anchoredPosition = new Vector2((zone - currentZone) * itemSpacing, 0f);

            Transform currentFrame = newItem.transform.Find("ui_panel_zone_item_current");
            if (currentFrame != null)
                currentFrame.gameObject.SetActive(zone == currentZone);

            TextMeshProUGUI itemText = newItem.GetComponentInChildren<TextMeshProUGUI>();
            if (itemText != null)
            {
                itemText.text = zone.ToString();
                itemText.color = GetZoneColor(zone, currentZone);
            }
        }

        zoneContent.DOKill();
        zoneContent.anchoredPosition = new Vector2(itemSpacing, 0f);
        zoneContent.DOAnchorPosX(0f, slideDuration).SetEase(Ease.OutCubic);
    }

    private Color GetZoneColor(int zone, int currentZone)
    {
        if (zone < currentZone)
            return passedColor;
        if (zone == currentZone)
            return normalColor;
        if (zone % 30 == 0)
            return superColor;
        if (zone % 5 == 0)
            return safeColor;

        return normalColor;
    }
}
