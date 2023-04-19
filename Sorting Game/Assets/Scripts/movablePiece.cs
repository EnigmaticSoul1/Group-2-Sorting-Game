using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movablePiece : MonoBehaviour 
{
 
    private recipePiece piece;

    //Awake references the recipe piece
    void Awake()
    {
        piece = GetComponent<recipePiece>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //moves the piece
    public void Move(int newX, int newY)
    {
        piece.X = newX;
        piece.Y = newY;

        piece.transform.localPosition = piece.GridRef.GetWorldPosition(newX, newY);
    }
}
