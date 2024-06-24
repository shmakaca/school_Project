using System.Collections;
using UnityEngine;

public class Pistol : Weapon
{
    public Camera PlayerCamera;

    private new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeybindManager.GetKeyCode("Shoot")))
        {
            HandleShooting();
        }
    }

    protected override void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, Range))
        {
            Debug.Log("Hit: " + hit.transform.name);
            EnemyClass Enemy = hit.transform.GetComponent<EnemyClass>();
            if (Enemy != null)
            {
                Enemy.TakeDamage(Damage);
            }
            GameObject impact = Instantiate(ImpactHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1.5f);
        }
    }
}
