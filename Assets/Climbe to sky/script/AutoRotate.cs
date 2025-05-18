using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [Header("Rotation Speed (degrees per second)")]
    public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}