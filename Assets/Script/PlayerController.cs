using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject gameController;
    public GameObject movePlate;

    //Vi tri
    private int xBoard = -1;
    private int yBoard = -1;

    // Check player
    private string player;

    //Sprite ref
    public Sprite white_king, white_queen, white_knight, white_bishop, white_rook, white_pawn;
    public Sprite black_king, black_queen, black_knight, black_bishop, black_rook, black_pawn;

    void Start()
    {
        
    }

    public void Activate(){
        gameController = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "White"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "White"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "White"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "White"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "White"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "White"; break;

            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "Black"; break;
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "Black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "Black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "Black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "Black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "Black"; break;
        }
    }

    public void SetCoords(){
        float x = xBoard;
        float y = yBoard;

        x *= 0.225f;
        y *= 0.225f;

        x+= -0.78f;
        y+= -0.78f;

        this.transform.position = new Vector3(x,y,-2f); 
    }

    public int getXBoard(){
        return xBoard;
    }

    public void setXBoard(int x){
        xBoard = x;
    }

    public int getYBoard(){
        return yBoard;
    }

    public void setYBoard(int y){
        yBoard = y;
    }

    private void OnMouseUp(){
        if(!gameController.GetComponent<GameController>().IsGameOver() && gameController.GetComponent<GameController>().GetCurrentPlayer() == player){
            PoolingMovePlate();

        InitiateMovePlate();
        gameController.GetComponent<GameController>().PlayAudio();
        }
    }

    public void PoolingMovePlate(){
        GameObject[] movePlate = GameObject.FindGameObjectsWithTag("MovePlate");
        for(int i = 0; i < movePlate.Length; i++){
            Destroy(movePlate[i]);
        } 
    }

    public void InitiateMovePlate(){
        switch(this.name){
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(1, -1);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                break;
            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1);
                break;
        }
    }

    public void LineMovePlate(int xIncrement,int yIncrement){
        GameController sc = gameController.GetComponent<GameController>();
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while(sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null){
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }
        if(sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<PlayerController>().player != player){
            MovePlateAttackSpawn(x, y);
        }
    }

    public void LMovePlate(){
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurroundMovePlate(){
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 1); 
        PointMovePlate(xBoard - 1, yBoard); 
        PointMovePlate(xBoard - 1, yBoard + 1); 
        PointMovePlate(xBoard + 1, yBoard - 1); 
        PointMovePlate(xBoard + 1, yBoard); 
        PointMovePlate(xBoard + 1, yBoard + 1); 
    }

    public void PointMovePlate(int x, int y){
        GameController sc = gameController.GetComponent<GameController>();
        if(sc.PositionOnBoard(x, y)){
            GameObject cp = sc.GetPosition(x, y);
            if(cp == null){
                MovePlateSpawn(x, y);
            }else if(cp.GetComponent<PlayerController>().player != player){
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void PawnMovePlate(int x, int y){
        GameController sc = gameController.GetComponent<GameController>();
        if(sc.PositionOnBoard(x, y)){
            if(sc.GetPosition(x, y) == null){
                MovePlateSpawn(x, y);
            }
            if(sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<PlayerController>().player != player){
                MovePlateAttackSpawn(x + 1, y);
            }
            if(sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<PlayerController>().player != player){
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY){
        float x = matrixX;
        float y = matrixY;

        x *= 0.225f;
        y *= 0.225f;

        x+= -0.78f;
        y+= -0.78f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY){
        float x = matrixX;
        float y = matrixY;

        x *= 0.225f;
        y *= 0.225f;

        x+= -0.78f;
        y+= -0.78f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
