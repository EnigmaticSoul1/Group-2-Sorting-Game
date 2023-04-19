using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recipePiece : MonoBehaviour
{
    // Start is called before the first frame update
    private int x;
    private int y;

    public int X {
        get { return X; }
        set
        {
            if (isMovable())
            {
                x = value;
            }
        }
    }

    public int Y {
        get { return Y; }
        set
        {
            if (isMovable())
            {
                y = value;
            }
        }
    }

    private testGrid.PieceType type;
    public testGrid.PieceType Type
    {
        get { return type; }
    }

    private testGrid grid;
    public testGrid GridRef
    {
        get { return grid; }
    }

    private movablePiece movableComponent;

    public movablePiece MovableComponent
    {
        get { return movableComponent; }
    }

    private recipeData recipeComponent;

    public recipeData RecipeComponent {
        get { return recipeComponent; }
    }

    

    void Awake()
    {
        movableComponent = GetComponent<movablePiece>();
        recipeComponent = GetComponent<recipeData>();
    }

    

   
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(int _x, int _y, testGrid _grid, testGrid.PieceType _type)
    {
        x = _x;
        y = _y;
        grid = _grid;
        type = _type;
    }

    public bool isMovable()
    {
        return movableComponent != null;
    }

    public bool isRecipe(){
        return recipeComponent != null;
    }

    
}
