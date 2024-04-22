using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaging : MonoBehaviour
{
    
    public int PlayerDamage;

    public bool canAttack = false;
    public float countDown = 0.7f;
    public bool isSowrd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isSowrd = FindAnyObjectByType<SwapGun>().Sowrding;//checks if the sowrd is in the hand


            if(countDown > 0)
            {
                countDown = countDown - Time.deltaTime;
            }
            else
            {
                canAttack = true;
            }
    }
    private void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.tag == "boss")
        {
            Debug.Log("ff");
            if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack && isSowrd)
            {
                
                FindAnyObjectByType<bossHealth>().Damage(PlayerDamage);
                countDown = 0.7f;
                canAttack = false;
            }
        }
    }
}
