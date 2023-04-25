using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class testGrid : MonoBehaviour
{
   
    public enum PieceType{
        EMPTY,
        NORMAL,
        ROW_CLEAR,
        COLUMN_CLEAR,
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
        bool needsRefill = true;

        while (needsRefill) {
            yield return new WaitForSeconds(fillTime);

            while (fillStep())
            {
                yield return new WaitForSeconds(fillTime);
            }

            needsRefill = ClearAllValidMatches();
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
        || (piece1.Y == piece2.Y && (int)Mathf.Abs(piece1.X - piece2.X) == 1);
    }

    //Swaps the pieces
    public void swapPieces(recipePiece piece1, recipePiece piece2)
    {
        if (piece1.isMovable() && piece2.isMovable())
        {
            pieces [piece1.X, piece1.Y] = piece2;
            pieces [piece2.X, piece2.Y] = piece1;

            if (GetMatch (piece1, piece2.X, piece2.Y) != null || GetMatch (piece2, piece1.X, piece1.Y) != null) {
                int piece1X = piece1.X;
                int piece1Y = piece1.Y;

                piece1.MovableComponent.Move(piece2.X, piece2.Y, fillTime);
                piece2.MovableComponent.Move(piece1X, piece1Y, fillTime);
                
                ClearAllValidMatches();

                if (piece1.Type == PieceType.ROW_CLEAR || piece1.Type == PieceType.COLUMN_CLEAR){
                    ClearPiece (piece1.X, piece1.Y);
                }

                if (piece2.Type == PieceType.ROW_CLEAR || piece2.Type == PieceType.COLUMN_CLEAR){
                    ClearPiece (piece2.X, piece2.Y);
                }

                pressedPiece = null;
                enteredPiece = null;

                StartCoroutine(Fill());
            } else {
                pieces [piece1.X, piece1.Y] = piece1;
                pieces [piece2.X, piece2.Y] = piece1;
            }
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

    //Checking if there are matching pieces
    public List<recipePiece> GetMatch(recipePiece piece, int newX, int newY)
    {
        if (piece.isRecipe()) {
            recipeData.recipeType recipe = piece.RecipeComponent.Type;
            List<recipePiece> horizontalPieces = new List<recipePiece> ();
            List<recipePiece> verticalPieces = new List<recipePiece> ();
            List<recipePiece> matchingPieces = new List<recipePiece> ();

            //Check horizontally for matching pieces
            horizontalPieces.Add(piece);
            for (int dir = 0; dir <= 1; dir++) {
                for (int xOffset = 1; xOffset < xDim; xOffset++) {
                    int x;

                    if (dir == 0) {
                        x = newX - xOffset;
                    } else {
                        x = newX + xOffset;
                    }

                    if (x < 0 || x >= xDim) {
                        break;
                    }

                    if (pieces[x, newY].isRecipe() && pieces[x, newY].RecipeComponent.Type == recipe) {
                        horizontalPieces.Add(pieces[x, newY]);
                    } else {
                        break;
                    }
                }
            }

            if (horizontalPieces.Count >= 3) {
                for (int i = 0; i < horizontalPieces.Count; i++) {
                    matchingPieces.Add(horizontalPieces[i]);
                } 
            }

            //Traverse vertically if we found a match (for L and T shape)
            if (horizontalPieces.Count >= 3) {
                for (int i = 0; i < horizontalPieces.Count; i++) {
                    for (int dir = 0; dir <= 1; dir++) {
                        for (int yOffset = 1; yOffset < yDim; yOffset++) {
                            int y;

                            if (dir == 0) { //Up
                                y = newY - yOffset;
                            } else { //Down
                                y = newY + yOffset;
                            }

                            if (y < 0 || y >= yDim) {
                                break;
                            }

                            if (pieces[horizontalPieces[i].X, y].isRecipe() && pieces[horizontalPieces[i].X, y].RecipeComponent.Type == recipe) {
                                verticalPieces.Add(pieces[horizontalPieces[i].X, y]);
                            } else {
                                break;
                            }
                        }
                    }

                    if (verticalPieces.Count < 2) {
                        verticalPieces.Clear();
                    } else {
                        for (int j = 0; j < verticalPieces.Count; j++) {
                            matchingPieces.Add(verticalPieces[j]);
                        }

                        break;
                    }
                }
            }

            if (matchingPieces.Count >= 3) {
                return matchingPieces;
            }

            //Check vertically for matching pieces
            horizontalPieces.Clear();
            verticalPieces.Clear();
            verticalPieces.Add(piece);

            for (int dir = 0; dir <= 1; dir++) {
                for (int yOffset = 1; yOffset < yDim; yOffset++) {
                    int y;

                    if (dir == 0) {
                        y = newY - yOffset;
                    } else {
                        y = newY + yOffset;
                    }

                    if (y < 0 || y >= yDim) {
                        break;
                    }

                    if (pieces[newX, y].isRecipe() && pieces[newX, y].RecipeComponent.Type == recipe) {
                        verticalPieces.Add(pieces[newX, y]);
                    } else {
                        break;
                    }
                }
            }

            if (verticalPieces.Count >=  3) {
                for (int i = 0; i < verticalPieces.Count; i++) {
                    matchingPieces.Add(verticalPieces[i]);
                }
            }

            //Traverse horizontally if we found a match (for L and T shape)
            if (verticalPieces.Count >= 3) {
                for (int i = 0; i < verticalPieces.Count; i++) {
                    for (int dir = 0; dir <= 1; dir++) {
                        for (int xOffset = 1; xOffset < xDim; xOffset++) {
                            int x;

                            if (dir == 0) { //Left
                                x = newX - xOffset;
                            } else { //Right
                                x = newX + xOffset;
                            }

                            if (x < 0 || x >= xDim) {
                                break;
                            }

                            if (pieces[x, verticalPieces[i].Y].isRecipe() && pieces[x, verticalPieces[i].Y].RecipeComponent.Type == recipe) {
                                horizontalPieces.Add(pieces[x, verticalPieces[i].Y]);
                            } else {
                                break;
                            }
                        }
                    }

                    if (horizontalPieces.Count < 2) {
                        horizontalPieces.Clear();
                    } else {
                        for (int j = 0; j < horizontalPieces.Count; j++) {
                            matchingPieces.Add(horizontalPieces[j]);
                        }

                        break;
                    }
                }
            }

            if (matchingPieces.Count >= 3) {
                return matchingPieces;
            }
        }

        return null;
    }

    public bool ClearAllValidMatches() {
        bool needsRefill = false;

        for (int y = 0; y < yDim; y++) {
            for (int x = 0; x < xDim; x++) {
                if (pieces [x, y].isClearable()) {
                    List<recipePiece> match = GetMatch(pieces[x, y], x, y);

                    if (match != null) {
                        PieceType specialPieceType = PieceType.COUNT;
                        recipePiece randomPiece = match [Random.Range (0, match.Count)];
                        int specialPieceX = randomPiece.X;
                        int specialPieceY = randomPiece.Y;

                        if (match.Count == 4) {
                            if (pressedPiece == null || enteredPiece == null) {
                                specialPieceType = (PieceType)Random.Range ((int)PieceType.ROW_CLEAR, (int)PieceType.COLUMN_CLEAR);
                            }
                            else if (pressedPiece.Y == enteredPiece.Y){
                                specialPieceType = PieceType.ROW_CLEAR;
                            }
                            else {
                                specialPieceType = PieceType.COLUMN_CLEAR;
                            }
                        }

                        for (int i = 0; i < match.Count; i++) {
                            if (ClearPiece(match[i].X, match[i].Y)) {
                                needsRefill = true;

                                if (match [i] == pressedPiece || match [i] == enteredPiece){
                                    specialPieceX = match [i].X;
                                    specialPieceY = match [i].Y;
                                }
                            }
                        }

                        if (specialPieceType != PieceType.COUNT){
                            Destroy (pieces [specialPieceX, specialPieceY]);
                            recipePiece newPiece = spawnNewPiece (specialPieceX, specialPieceY, specialPieceType);

                            if ((specialPieceType == PieceType.ROW_CLEAR || specialPieceType == PieceType.COLUMN_CLEAR) && newPiece.isRecipe() && match[0].isRecipe ()){
                                newPiece.RecipeComponent.SetType(match[0].RecipeComponent.Type);
                            }
                        }
                    }
                }
            }
        }

        return needsRefill;
    }

    public bool ClearPiece(int x, int y) {
        if (pieces[x, y].isClearable() && !pieces[x, y].ClearableComponent.IsBeingCleared) {
            pieces [x, y].ClearableComponent.Clear();
            spawnNewPiece(x, y, PieceType.EMPTY);

            return true;
        }

        return false;
    }

    //to be used in the clearline file; implements the effects of row clearing powerups
    public void clearRow(int row){
        for (int x = 0; x < xDim; x++){
            ClearPiece (x, row);
        }
    }

    //to be used in the clearline file; implements the effects of column clearing powerups
    public void clearColumn(int column){
        for (int y = 0; y < yDim; y++){
            ClearPiece (y, column);
        }
    }
}
