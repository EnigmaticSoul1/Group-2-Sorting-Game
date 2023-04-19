using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recipePiece : MonoBehaviour
{
    // Start is called before the first frame update
    private int x;
    private int y;

    private int X {
        get { return X; }
    }

    private int Y{
        get { return Y; }
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

    private recipeData recipeComponent;

    public recipeData RecipeComponent{
        get { return recipeComponent; }
    }

    void Awake(){
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

    public bool isRecipe(){
        return recipeComponent != null;
    }
}
