using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orderData : MonoBehaviour
{
    public enum orderType{
        black_cof,
        latte,
        cappuccino,
        milky_way,
        mocha,
        frappe,
        rush_hour,
        cascara,
        milk_tea,
    }

    [System.Serializable]
    public struct orderSprite{
        public orderType order;
        public Sprite sprite;
    }

    public orderSprite[] orderSprites;

    private orderType type;

    public orderType Type{
        get {return type; }
        set { SetType (value); }
    }

    public int NumTypes{
        get { return orderSprites.Length; }
    }

    private SpriteRenderer sprite;

    private Dictionary<orderType, Sprite> orderSpriteDict;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetType(orderType newType){
        type = newType;

        if (orderSpriteDict.ContainsKey (newType)){
            sprite.sprite = orderSpriteDict [newType];
        }
    }
}
