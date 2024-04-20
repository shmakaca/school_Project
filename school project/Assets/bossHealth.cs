using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bossHealth : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject boss;
    public int bossHP;
    public ParticleSystem deathPar;
   
    public void Damage(int damage)
    {
        bossHP = bossHP - damage;
    }

    private void Update()
    {
        if (bossHP <= 0 && boss != null && healthBar != null)
        {
            deathPar.Play();
            Destroy( boss , 5.4f);
            Destroy(healthBar);
        }
        healthBar.GetComponent<Slider>().value = bossHP ;
    }
}
