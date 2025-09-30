using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunFire : MonoBehaviour
{
    public int gunDamage = 25;
    public float fireRate = 0.15f;
    public Transform gunEnd;


    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private AudioSource gunFire;
    private LineRenderer laserLine;
    private float nextFire;

    InputAction attackAction;

    private void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");

        laserLine = GetComponent<LineRenderer>();
        gunFire = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
    }

    void Update()
    {
        if(attackAction.IsPressed() && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            laserLine.SetPosition(0, gunEnd.position);
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit))
            {
                laserLine.SetPosition(1, hit.point);
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * 100));
            }
        }
    }

    IEnumerator ShotEffect()
    {
        gunFire.Play();
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
}
