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

    private GameObject[,] pieces;

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
                GameObject background = (GameObject)Instantiate (backgroundPrefab, new Vector3(x, y, 0), Quaternion.identity);
                background.transform.parent = transform;
            }
        }

        pieces = new GameObject[xDim, yDim];
        for (int x = 0; x < xDim; x++){
            for (int y = 0; y < yDim; y++){
                pieces [x, y] = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], new Vector3(x, y, 0), Quaternion.identity);
                pieces [x, y].name = "Piece(" + x + "," + y + ")";
                pieces [x, y].transform.parent = transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
