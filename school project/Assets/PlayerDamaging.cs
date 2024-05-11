using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaging : MonoBehaviour
{

    public int PlayerDamage;

    public bool canAttack = false;
    public float countDown = 0.7f;
    public bool isSowrd;
    public  bool Swinging;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isSowrd = FindAnyObjectByType<SwapGun>().InSowrdSlot;//checks if the sowrd is in the hand


        if (countDown > 0)
        {
            countDown = countDown - Time.deltaTime;
        }
        else
        {
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack && isSowrd)
        {
            Swinging = true;
            animator.SetTrigger("attacking");
        }
        else
        {
            Swinging = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "boss")
        {

            if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack && isSowrd)
            {

                Invoke("damage", 0.7f);
                animator.SetTrigger("attacking");
                countDown = 0.7f;
                canAttack = false;
            }
        }
    }
    private void damage()
    {
        FindAnyObjectByType<bossHealth>().Damage(PlayerDamage);
    }
}
