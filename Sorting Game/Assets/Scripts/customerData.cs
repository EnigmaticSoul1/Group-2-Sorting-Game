using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customerData : MonoBehaviour
{
   
    public enum customerType{
        male1,
        male2,
        male3,
        female1,
        female2,
        star
    }

    [System.Serializable]
    public struct customerSprite{
        public customerType customer;
        public Sprite sprite;
    }

    public customerSprite[] customerSprites;

    private customerType type;

    public customerType Type{
        get {return type; }
        set { SetType (value); }
    }

    public int NumTypes{
        get { return customerSprites.Length; }
    }

    private SpriteRenderer sprite;

    private Dictionary<customerType, Sprite> customerSpriteDict;

    void Awake(){
        sprite = transform.Find("piece").GetComponent<SpriteRenderer> ();

        customerSpriteDict = new Dictionary<customerType, Sprite> ();
        for (int i = 0; i < customerSprites.Length; i++) {
            if (!customerSpriteDict.ContainsKey (customerSprites [i].customer)) {
                customerSpriteDict.Add (customerSprites[i].customer, customerSprites[i].sprite);
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetType(customerType newType){
        type = newType;

        if (customerSpriteDict.ContainsKey (newType)){
            sprite.sprite = customerSpriteDict [newType];
        }
    }
}
