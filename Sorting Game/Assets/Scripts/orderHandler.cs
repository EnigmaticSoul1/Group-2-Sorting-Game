using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class orderHandler : MonoBehaviour
{
    public GameObject[] orderType;

    
    void Start()
    {
        findRandom();
    }

    void findRandom()
    {
        int index = Random.Range(0, orderType.Length);
        GameObject order = Instantiate(orderType[index], transform.position, Quaternion.identity);
        orderType[index].transform.parent = gameObject.transform;
        Debug.Log(index);

        
    }
}
