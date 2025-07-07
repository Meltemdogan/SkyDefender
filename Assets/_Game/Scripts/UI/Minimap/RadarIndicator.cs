using UnityEngine;

public class RadarIndicator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f; // Degrees per second
    void Update()
    {
        transform.eulerAngles += new Vector3(0, 0, rotationSpeed * Time.deltaTime);
    }
}
