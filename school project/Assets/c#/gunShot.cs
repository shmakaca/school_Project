using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class gunShot : MonoBehaviour
{
    public Animator animator;
    
    public TextMeshProUGUI bullText;
    public Transform bulletHall;
    public GameObject Bullet;

    public int bullDamage;

    private bool isGun;
    private bool isReloading;
    private int shotsNum;
    private int mag = 2;
    // Start is called before the first frame update
    void Start()
    {
        isReloading = false;
        shotsNum = mag;
    }

    // Update is called once per frame
    void Update()
    {
        isGun = FindAnyObjectByType<SwapGun>().Guning;
        if(isGun && Input.GetKeyDown(KeyCode.Mouse0) && !isReloading && shotsNum > 0)
        {
            shot();
            shotsNum--;
        }


        if (isGun && Input.GetKeyDown(KeyCode.R))
        {
            isReloading = true;
            animator.SetTrigger("trReload");
            Invoke("Reloading", 2f);
            isReloading =false;
        }
        
    }
    private void shot()
    {
        bool koko = true;

        if(koko)
        {
            Instantiate(Bullet, bulletHall);
            koko = false;
        }

    }
    private void Reloading()
    {
        
        shotsNum = mag;
    }
}
