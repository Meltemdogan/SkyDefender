using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Minimap : MonoBehaviour
{
    public static Minimap Instance { get; private set; }
    
    [Header("UI Settings")]
    [SerializeField]
    private RectTransform mapRect; // Minimap panel
    [SerializeField] private RectTransform elementParents;
    [SerializeField] private Image enemyIndicatorPf;
    [SerializeField] private Image playerIndicator;
    [SerializeField] private float realWorldScale = 0.1f;
    
    [Header("World References")]
    [SerializeField]
    private Transform playerTransform;
    
    private readonly List<MinimapElement> enemyIndicators = new List<MinimapElement>();
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        if (playerIndicator != null)
            playerIndicator.rectTransform.localScale = Vector3.one;
    }
    
    public void RegisterEnemy(Transform enemyTransform)
    {
        if (enemyTransform == null) return;
        if (enemyIndicators.Exists(e => e.Transform == enemyTransform)) return;
        
        var inst = Instantiate(enemyIndicatorPf, elementParents);
        inst.rectTransform.localScale = Vector3.one; // Use default scale
        enemyIndicators.Add(new MinimapElement(enemyTransform, inst));
    }
    
    public void UnregisterEnemy(Transform enemyTransform)
    {
        var elem = enemyIndicators.Find(e => e.Transform == enemyTransform);
        if (elem.Indicator != null) Destroy(elem.Indicator.gameObject);
        enemyIndicators.RemoveAll(e => e.Transform == enemyTransform);
    }
    
    private void LateUpdate()
    {
        if (playerTransform == null) return;
        
        playerIndicator.rectTransform.anchoredPosition = Vector2.zero;
        var playerAngle = playerTransform.eulerAngles.z;
        playerIndicator.transform.rotation = Quaternion.Euler(0, 0, playerAngle);
        
        float halfW = mapRect.rect.width * 0.5f;
        float halfH = mapRect.rect.height * 0.5f;
        
        foreach (var elem in enemyIndicators)
        {
            if (elem.Transform == null) continue;
            
            Vector2 delta = new Vector2(
                elem.Transform.position.x - playerTransform.position.x,
                elem.Transform.position.y - playerTransform.position.y
            );
            
            Vector2 uiPos = delta * realWorldScale;
            uiPos.x = Mathf.Clamp(uiPos.x, -halfW, halfW);
            uiPos.y = Mathf.Clamp(uiPos.y, -halfH, halfH);
            
            elem.Indicator.rectTransform.anchoredPosition = uiPos;
            elem.Indicator.gameObject.SetActive(uiPos.magnitude <= halfW && uiPos.magnitude <= halfH);
        }
    }
    
    private struct MinimapElement
    {
        public Transform Transform;
        public Image Indicator;
        
        public MinimapElement(Transform t, Image i)
        {
            Transform = t;
            Indicator = i;
        }
    }
}