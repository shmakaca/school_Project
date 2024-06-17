using System.Collections;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public float enemyHealth = 50f;
    public Animator dieAnimator;
    public AnimationClip dieAnimation;

    public void TakeDamage(float damageAmount)
    {
        enemyHealth -= damageAmount;

        if (enemyHealth <= 0)
        {
            ApplyDie();
        }
    }

    private void ApplyDie()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        dieAnimator.SetTrigger("Die");
        yield return new WaitForSeconds(dieAnimation.length + 0.2f);
        Destroy(gameObject);
    }
}
