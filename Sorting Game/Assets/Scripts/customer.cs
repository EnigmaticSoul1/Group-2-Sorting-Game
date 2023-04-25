using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class customer : MonoBehaviour
{
    public GameObject[] customerType;

    
    void Start()
    {
        findRandom();
    }

    void findRandom()
    {
        int index = Random.Range(0, customerType.Length);
        GameObject customer = Instantiate(customerType[index], transform.position, Quaternion.identity);
        customerType[index].transform.parent = gameObject.transform;
        Debug.Log(index);
    }
}
