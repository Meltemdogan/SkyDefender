using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Ölçeği")]
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f);
    [Header("Animasyon Süresi (s)")]
    public float duration = 0.2f;
    
    private Vector3 originalScale;
    private Tween hoverTween;
    
    void Awake()
    {
        // Orijinal ölçeği sakla
        originalScale = transform.localScale;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Mevcut tween varsa iptal et ve yeni tween başlat
        transform.DOKill();
        hoverTween = transform
            .DOScale(hoverScale, duration)
            .SetEase(Ease.OutBack)
            .SetAutoKill(true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        // Mevcut tween varsa iptal et ve orijinal ölçeğe dönen tween başlat
        transform.DOKill();
        hoverTween = transform
            .DOScale(originalScale, duration)
            .SetEase(Ease.OutBack)
            .SetAutoKill(true);
    }
    
    void OnDisable()
    {
        // Objeyi kapatırken tüm tweens'i iptal et
        transform.DOKill();
    }
    
    void OnDestroy()
    {
        // Objeyi yok ederken de güvenli olması için tekrar iptal et
        transform.DOKill();
    }
}