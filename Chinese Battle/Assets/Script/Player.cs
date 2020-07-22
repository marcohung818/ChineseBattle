using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Player : MonoBehaviour
{
    int hp = 100;
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
        UnityEngine.Vector3 playerPos = this.transform.position;
        UnityEngine.Vector3 playerDirection = this.transform.forward;
        UnityEngine.Quaternion playerRotation = this.transform.rotation;
        UnityEngine.Vector3 spawnPos = playerPos + new UnityEngine.Vector3(1, 0, 0);
        GameObject bullet = Instantiate(ChassBoard.instance.bullet, spawnPos, playerRotation);
        bullet.GetComponent<Bullet>().UpdateDamage(10);
        bullet.GetComponent<Bullet>().Move(2);
    }
}
