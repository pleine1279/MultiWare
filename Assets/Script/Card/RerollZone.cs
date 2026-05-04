using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RerollZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static RerollZone Instance;

    private Image zoneImage;
    private Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.7f);
    private Color highlightColor = new Color(0.8f, 0.8f, 0f, 0.9f); // 노란색 강조

    private void Awake()
    {
        Instance = this;
        zoneImage = GetComponent<Image>();
        if (zoneImage != null)
            zoneImage.color = normalColor;
    }

    // 마우스 올라왔을 때 강조
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (zoneImage != null)
            zoneImage.color = highlightColor;
    }

    // 마우스 나갔을 때 원래 색
    public void OnPointerExit(PointerEventData eventData)
    {
        if (zoneImage != null)
            zoneImage.color = normalColor;
    }
}