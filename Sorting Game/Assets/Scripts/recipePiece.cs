using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recipePiece : MonoBehaviour
{
    
    private int x;
    private int y;

    public int X {
        get { return x; }
        set
        {
            if (isMovable())
            {
                x = value;
            }
        }
    }

    public int Y {
        get { return y; }
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

    //gives the movable piece component
    private movablePiece movableComponent;

    public movablePiece MovableComponent
    {
        get { return movableComponent; }
    }

    private recipeData recipeComponent;

    public recipeData RecipeComponent {
        get { return recipeComponent; }
    }

    
    //References movablePiece and recipeData script
    void Awake()
    {
        movableComponent = GetComponent<movablePiece>();
        recipeComponent = GetComponent<recipeData>();
    }




    // Start is called before the first frame update
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


    //when mouse enters the element
    void OnMouseEnter()
    {
        grid.enterPiece(this);
        
    }

    //when mouse presses down the recipe
    void OnMouseDown()
    {
        grid.pressPiece(this);
    }

    //When mouse releases the press of the recipe
    void OnMouseUp()
    {
        grid.releasePiece();
    }

    //For pieces who have movable component
    public bool isMovable()
    {
        return movableComponent != null;
    }

    public bool isRecipe(){
        return recipeComponent != null;
    }

    
}
