﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.name == "Player") {
            GetComponent<DeathManager>().StartDeathCoroutine();
        }
    }
}
