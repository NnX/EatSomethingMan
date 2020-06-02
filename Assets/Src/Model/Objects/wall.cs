using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    public GameObject pacman;
    void OnTriggerEnter2D(Collider2D other)
    {

        //debug.log("wall trigger tag = " + other.tag + ", name = " + other.name);
    }
}
