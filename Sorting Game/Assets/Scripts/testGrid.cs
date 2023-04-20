using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class testGrid : MonoBehaviour
{
   
    public enum PieceType{
        EMPTY,
        NORMAL,
        COUNT,
    };

    [System.Serializable]
    public struct PiecePrefab{
        public PieceType type;
        public GameObject prefab;

    }

    public int xDim;
    public int yDim;
    public float fillTime;

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;
    private Dictionary<PieceType, GameObject> piecePrefabDict;

    private recipePiece[,] pieces;

    private recipePiece pressedPiece;
    private recipePiece enteredPiece;

    // Start is called before the first frame update
    void Start()
    {
        piecePrefabDict = new Dictionary<PieceType, GameObject> ();
        
        for (int i = 0; i < piecePrefabs.Length; i++){
            if (!piecePrefabDict.ContainsKey (piecePrefabs [i].type)) {
                piecePrefabDict.Add (piecePrefabs [i].type, piecePrefabs [i].prefab);
            }
        }

        for (int x = 0; x < xDim; x++){
            for (int y = 0; y < yDim; y++){
                GameObject background = (GameObject)Instantiate (backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
                background.transform.parent = transform;
            }
        }

        pieces = new recipePiece[xDim, yDim];
        for (int x = 0; x < xDim; x++){
            for (int y = 0; y < yDim; y++){
                spawnNewPiece(x, y, PieceType.EMPTY);
            }
        }
        StartCoroutine(Fill());
    }

    // Update is called once per frame
    void Update()
    {
       
            
    }

    //move the piece one space
    public IEnumerator Fill()
    {
        while (fillStep())
        {
            yield return new WaitForSeconds(fillTime);
        }
    }

    //fills the board
    public bool fillStep()
    {
        bool movedPiece = false;

        for (int y = yDim - 2; y >= 0; y--)
        {
            for (int x = 0; x < xDim; x++)
            {
                recipePiece piece = pieces[x, y];

                if (piece.isMovable())
                {
                    recipePiece pieceBelow = pieces[x, y +1];

                    if (pieceBelow.Type == PieceType.EMPTY)
                    {
                        Destroy(pieceBelow.gameObject);
                        piece.MovableComponent.Move(x, y + 1, fillTime);
                        pieces[x, y + 1] = piece;
                        spawnNewPiece(x, y, PieceType.EMPTY);
                        movedPiece = true;
                    }
                }
            }
        }

        for (int x = 0; x < xDim; x++)
        {
            recipePiece pieceBelow = pieces[x, 0];

            if (pieceBelow.Type == PieceType.EMPTY)
            {
                Destroy(pieceBelow.gameObject);
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[x, 0] = newPiece.GetComponent<recipePiece>();
                pieces[x, 0].init(x, -1, this, PieceType.NORMAL);
                pieces[x, 0].MovableComponent.Move(x, 0, fillTime);
                pieces[x, 0].RecipeComponent.SetType ((recipeData.recipeType)Random.Range(0, pieces[x, 0].RecipeComponent.NumTypes));
                movedPiece = true;
            }
        }
        return movedPiece;
    }

    //gets the position in the scene
    public Vector2 GetWorldPosition(int x, int y){
        return new Vector2 (transform.position.x - xDim / 2.0f + x, transform.position.y + yDim / 2.0f -y);
    }

    //Spawns pieces
    public recipePiece spawnNewPiece(int x, int y, PieceType type)
    {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;

        pieces[x, y] = newPiece.GetComponent<recipePiece>();
        pieces[x, y].init (x, y, this, type);

        return pieces[x, y];
    }

    //Checks the adjacent tiles of the recipe
    public bool isAdjacent(recipePiece piece1, recipePiece piece2)
    {
        return (piece1.X == piece2.X && (int)Mathf.Abs(piece1.Y - piece2.Y) == 1)
        || (piece1.Y == piece2.Y && (int)Mathf.Abs(piece1.X - piece2.Y) == 1);
    }

    //Swaps the pieces
    public void swapPieces(recipePiece piece1, recipePiece piece2)
    {
        if (piece1.isMovable() && piece2.isMovable())
        {
            pieces [piece1.X, piece1.Y] = piece2;
            pieces [piece2.X, piece2.Y] = piece1;

            int piece1X = piece1.X;
            int piece1Y = piece1.Y;

            piece1.MovableComponent.Move(piece2.X, piece2.Y, fillTime);
            piece2.MovableComponent.Move(piece1X, piece1Y, fillTime);
        }
    }

    //when a piece is pressed
    public void pressPiece(recipePiece piece)
    {
        pressedPiece = piece;
    }

    //when a piece is entered
    public void enterPiece (recipePiece piece)
    {
        enteredPiece = piece;
    }

    //when the mouse releases the piece
    public void releasePiece()
    {
        if (isAdjacent(pressedPiece, enteredPiece))
        {
            swapPieces(pressedPiece, enteredPiece);
        }
    }
}
