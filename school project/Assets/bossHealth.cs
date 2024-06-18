using System.Collections;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public float enemyHealth ;
    public Animator dieAnimator;
    public AnimationClip dieAnimation;
    public ParticleSystem deathPar;
    public GameObject attack;
    public GameObject boss;
    public Transform player;
    public void TakeDamage(float damageAmount)
    {
        enemyHealth -= damageAmount;

        if (enemyHealth <= 0)
        {
            //ApplyDie();
            deathPar.Play();
            boss.SetActive(false);
            attack.SetActive(false);
        }
    }
    public void Update()
    {
        transform.LookAt(player);
    }
    //private void ApplyDie()
    //{
    //    StartCoroutine(Die());
    //}

    //private IEnumerator Die()
    //{
    //    dieAnimator.SetTrigger("Die");
    //    yield return new WaitForSeconds(dieAnimation.length + 0.2f);
    //    Destroy(gameObject);
    //}
}
