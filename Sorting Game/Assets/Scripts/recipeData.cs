using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recipeData : MonoBehaviour
{
   
    public enum recipeType{
        A,
        B,
        C,
        D,
        E,
        F,
        //haven't included power ups type here
    }

    [System.Serializable]
    public struct recipeSprite{
        public recipeType recipe;
        public Sprite sprite;
    }

    public recipeSprite[] recipeSprites;

    private recipeType type;

    public recipeType Type{
        get {return type; }
        set { SetType (value); }
    }

    public int NumTypes{
        get { return recipeSprites.Length; }
    }

    private SpriteRenderer sprite;

    private Dictionary<recipeType, Sprite> recipeSpriteDict;

    void Awake(){
        sprite = transform.Find("piece").GetComponent<SpriteRenderer> ();

        recipeSpriteDict = new Dictionary<recipeType, Sprite> ();
        for (int i = 0; i < recipeSprites.Length; i++) {
            if (!recipeSpriteDict.ContainsKey (recipeSprites [i].recipe)) {
                recipeSpriteDict.Add (recipeSprites[i].recipe, recipeSprites[i].sprite);
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

    public void SetType(recipeType newType){
        type = newType;

        if (recipeSpriteDict.ContainsKey (newType)){
            sprite.sprite = recipeSpriteDict [newType];
        }
    }
}
