using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet : MonoBehaviour
{
    private int damage;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void UpdateDamage(int damageValue)
    {
        damage = damageValue;
    }

    public void Move(int speed)
    {
        rb.velocity = gameObject.GetComponentInParent<Transform>().right * speed;
    }

    public void UpdateTag(string tag)
    {
        this.gameObject.tag = tag;
    }

    public int ReturnDamage()
    {
        return damage;
    }
}
