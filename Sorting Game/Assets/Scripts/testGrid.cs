using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGrid : MonoBehaviour
{
    // Start is called before the first frame update
    public enum PieceType{
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
                GameObject background = (GameObject)Instantiate (backgroundPrefab, GetWorldPostion(x, y), Quaternion.identity);
                background.transform.parent = transform;
            }
        }

        pieces = new recipePiece[xDim, yDim];
        for (int x = 0; x < xDim; x++){
            for (int y = 0; y < yDim; y++){
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPostion(x, y), Quaternion.identity);
                newPiece.name = "Piece(" + x + "," + y + ")";
                newPiece.transform.parent = transform;

                pieces[x, y] = newPiece.GetComponent<recipePiece>();
                pieces[x, y].init(x, y, this, PieceType.NORMAL);

                if (pieces [x, y].isRecipe()){
                    pieces [x, y].RecipeComponent.SetType((recipeData.recipeType)Random.Range(0, pieces [x, y].RecipeComponent.NumTypes));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector2 GetWorldPostion(int x, int y){
        return new Vector2 (transform.position.x - xDim / 2.0f + x, transform.position.y + yDim / 2.0f -y);
    }
}
