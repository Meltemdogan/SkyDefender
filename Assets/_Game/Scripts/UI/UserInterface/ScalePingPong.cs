using DG.Tweening;
using UnityEngine;

public class ScalePingPong : MonoBehaviour
{
    [Header("Hedef Ölçek")]
    public Vector3 targetScale = new Vector3(0.5f, 0.5f, 0.5f);
    [Header("Tek Yön Animasyon Süresi (saniye)")]
    public float duration = 0.5f;
    [Header("Animasyon Döngü Sayısı (-1 = sonsuz)")]
    public int loops = -1;
    
    void Start()
    {
        // Orijinal ölçeğe dönüp tekrar hedefe geçsin, ping-pong döngüsüyle
        transform.DOScale(targetScale, duration)
            .SetLoops(loops, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}