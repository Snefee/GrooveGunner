using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunFire : MonoBehaviour
{
    InputAction attackAction;
    [SerializeField] AudioSource gunFire;
    [SerializeField] GameObject assaultRifle;
    [SerializeField] bool canFire = true;

    private void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        if(attackAction.IsPressed())
        {
            if (canFire == true)
            {
                canFire = false;
                StartCoroutine(firingGun());
            }
        }
    }

    IEnumerator firingGun()
    {
        gunFire.Play();
        assaultRifle.GetComponent<Animator>().Play("AssaultRifleFire");
        yield return new WaitForSeconds(0.15f);
        assaultRifle.GetComponent<Animator>().Play("New State");
        canFire = true;
    }
}
