using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("refrences")]
    public GameObject Player;
    

    [Header("ShotsKind")]
    public GameObject FireBall, ElectroBall, SnowBall;
    [Header ("ShotsDetails")]
    
    public float cooldown;

    public bool IsCoolDownDone = false;
   public bool IsInRange = false;

    [Header("animation")]
    public Animator Animator;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        transform.LookAt(Player.transform.position);

        //timer
        if (this.cooldown > 0)
        {
            this.cooldown = this.cooldown - Time.deltaTime;
        }
        else
        {
            this.IsCoolDownDone = true;
        }

        if (this.IsInRange && this.IsCoolDownDone)
        {


            SummonBall();
            Invoke("anim", 2f);
            
        }

    }
    private void anim()
    {
        Animator.SetTrigger("isShooting");
    }
    public void SummonBall()
    {
        bool temp = true;
        if(temp)
        {
            int x;
          x = Random.Range(1, 4);

          if (x == 1)
          {
            Instantiate(FireBall, this.transform);
          }
          else if (x == 2)
          {
            Instantiate(ElectroBall, this.transform);
          }
         else
         {
            Instantiate(SnowBall, this.transform);
         }
          this.cooldown = Random.Range(5, 7);
          IsCoolDownDone = false;

            temp = false;
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.IsInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.IsInRange = false;
        }
    }
}
