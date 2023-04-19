using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGrid : MonoBehaviour
{
    // Start is called before the first frame update
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

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;
    private Dictionary<PieceType, GameObject> piecePrefabDict;

    private recipePiece[,] pieces;

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
        Fill();
    }

    // Update is called once per frame
    void Update()
    {
       
            
    }

    //move the piece one space
    public void Fill()
    {
        while (fillStep())
        {

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
                        piece.MovableComponent.Move(x, y + 1);
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
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[x, 0] = newPiece.GetComponent<recipePiece>();
                pieces[x, 0].init(x, -1, this, PieceType.NORMAL);
                pieces[x, 0].MovableComponent.Move(x, 0);
                pieces[x, 0].RecipeComponent.SetType((recipeData.recipeType)Random.Range(0, pieces[x, 0].RecipeComponent.NumTypes));
                movedPiece = true;
            }
        }
        return movedPiece;
    }

    public Vector2 GetWorldPosition(int x, int y){
        return new Vector2 (transform.position.x - xDim / 2.0f + x, transform.position.y + yDim / 2.0f -y);
    }

    public recipePiece spawnNewPiece(int x, int y, PieceType type)
    {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;

        pieces[x, y] = newPiece.GetComponent<recipePiece>();
        pieces[x, y].init (x, y, this, type);

        return pieces[x, y];
    }
}
