using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Romde_Little : MonoBehaviour
{
    public Transform Littler;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var aa = new Vector2(3, 1.1f);
            Instantiate<Transform>(Littler, aa, Quaternion.identity);
        }
    }
}
