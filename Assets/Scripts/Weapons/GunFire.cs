using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunFire : MonoBehaviour
{
    public int gunDamage = 25;
    public float fireRate = 0.15f;
    public Transform gunEnd;
    public float weaponRange = 100f;
    public Camera fpsCam;
    public LayerMask shootableLayers;

    private Animator Anim_AssaultRifleFire;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.01f);
    private AudioSource gunFire;
    private LineRenderer laserLine;
    private float nextFire;

    InputAction attackAction;

    private void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");

        Anim_AssaultRifleFire = GetComponent<Animator>();
        laserLine = GetComponent<LineRenderer>();
        gunFire = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(attackAction.IsPressed() && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            PointsSystem.instance.RegisterShotFired();

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            laserLine.SetPosition(0, gunEnd.position);
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange, shootableLayers))
            {
                laserLine.SetPosition(1, hit.point);

                PointsSystem.instance.RegisterShotHit();

                EnemyHitDetectionHeadshot headshot = hit.collider.GetComponent<EnemyHitDetectionHeadshot>();
                if (headshot != null)
                {
                    headshot.Headshot();
                }
                else
                {
                    EnemyHitDetection health = hit.collider.GetComponentInParent<EnemyHitDetection>();
                    if (health != null)
                    {
                        health.Damage(gunDamage);
                    }
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
    }

    IEnumerator ShotEffect()
    {
        gunFire.Play();
        Anim_AssaultRifleFire.SetTrigger("Fire");
        laserLine.enabled = true;
        yield return shotDuration;

        laserLine.enabled = false;
    }
}
