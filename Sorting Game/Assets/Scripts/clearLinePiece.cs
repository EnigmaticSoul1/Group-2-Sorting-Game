using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearLinePiece : clearablePiece
{
    public bool isRow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Clear()
    {
        base.Clear();

        if (isRow){
            piece.GridRef.clearRow (piece.Y);
        }
        else {
            piece.GridRef.clearColumn (piece.X);
        }
    }
}
