using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]int hp = 100;
    public Animator animator;
    //character skill

    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //attack have the paramter for the damage
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void GenBulletAfterAnimation()
    {
        UnityEngine.Vector3 spawnPos = this.transform.position + new UnityEngine.Vector3(1, 0, 0);
        GameObject bullet = Instantiate(ChassBoard.instance.bullet, spawnPos, this.transform.rotation);
        bullet.GetComponent<Bullet>().UpdateDamage(10);
        bullet.GetComponent<Bullet>().Move(2);
        bullet.GetComponent<Bullet>().UpdateTag("Player");
    }

    public void Injured(int damage)
    {
        hp -= damage;
        print(hp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != this.gameObject.tag && collision.GetComponent<Bullet>() != null)
        {
            Injured(collision.GetComponent<Bullet>().ReturnDamage());
        }
    }
}
