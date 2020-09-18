using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassValueTesting : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
    }
}
