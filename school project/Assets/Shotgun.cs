using System.Collections;
using UnityEngine;

public class Shotgun : Weapon
{
    private KeybindManager KeybindManager;
    public GameObject Keybindmanager;
    public GameObject Player;
    public Camera PlayerCamera;
    private Rigidbody Rigidbody;
    public float knockbackforce = 10f;
    public float Impactforce = 10f;


    private new void Start()
    {
        base.Start();
        KeybindManager = Keybindmanager.GetComponent<KeybindManager>();
        Rigidbody = Player.GetComponent<Rigidbody>();

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
                Debug.Log("Enemy hit, applying damage.");
                Enemy.TakeDamage(Damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * Impactforce);
            }

            GameObject impact = Instantiate(ImpactHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1.5f);
        }

        knockBack();
    }

    private void knockBack()
    {
        if (Rigidbody != null && PlayerCamera != null)
        {
            Vector3 forceDirection = -PlayerCamera.transform.forward; 
            forceDirection.Normalize(); 
            Rigidbody.AddForce(forceDirection * knockbackforce, ForceMode.Impulse);
        }
    }
}
