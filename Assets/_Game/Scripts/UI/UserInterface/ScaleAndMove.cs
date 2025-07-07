using DG.Tweening;
using UnityEngine;

public class ScaleAndMove : MonoBehaviour
{
    [Header("Hedef Ölçek")]
    public Vector3 targetScale = new Vector3(0.5f, 0.5f, 0.5f);
    [Header("Hedef Transform")]
    public Transform targetTransform;
    [Header("Animasyon Süresi (saniye)")]
    public float duration = 0.5f;
    [Header("Döngü Sayısı (-1 = sonsuz)")]
    public int loops = -1;
    
    void Start()
    {
        // 1) Yeni bir sequence oluştur
        var seq = DOTween.Sequence();
        
        // 2) Ölçek animasyonunu ekle
        seq.Append(transform.DOScale(targetScale, duration));
        
        // 3) Aynı anda pozisyon animasyonunu da join et
        seq.Join(transform.DOMove(targetTransform.position, duration));
        
        // 4) İstersen Yoyo döngü ve easing ekle
        seq.SetLoops(loops, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}