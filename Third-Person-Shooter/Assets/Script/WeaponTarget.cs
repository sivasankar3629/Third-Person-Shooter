using UnityEngine;

public class WeaponTarget : MonoBehaviour
{
    [SerializeField] Camera cam;
    Ray ray;
    RaycastHit hitInfo;

    private void Update()
    {
        ray.origin = cam.transform.position;
        ray.direction = cam.transform.forward;
        Physics.Raycast(ray, out hitInfo);
        transform.position = hitInfo.point;
    }
}
