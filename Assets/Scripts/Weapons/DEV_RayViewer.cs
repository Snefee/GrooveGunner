using UnityEngine;

public class DEV_RayViewer : MonoBehaviour
{
    public float rayLength = 100f;

    private Camera fpsCam;

    void Start()
    {
        fpsCam = GetComponentInParent<Camera>();
    }

    void Update()
    {
        Vector3 lineOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(lineOrigin, fpsCam.transform.forward * rayLength, Color.green);
    }
}
