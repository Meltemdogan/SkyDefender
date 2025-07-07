using UnityEngine;
using System.Collections;
using ToolBox.Pools;
public class ReturnPoolAfterAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(WaitAndReturnPool());
    }
    
    private IEnumerator WaitAndReturnPool()
    {
        Animator animator = GetComponent<Animator>();
        yield return null;
        float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationTime);
        gameObject.Release();
    }
}