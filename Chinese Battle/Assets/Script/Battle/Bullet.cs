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
    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDamage(int index)
    {
        damage = index;
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
