using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject gameController;

    GameObject reference = null;

    int matrixX;
    int matrixY;

    //fals: di chuyen, true: attack
    public bool attack = false;

    public void Start(){
        if(attack){
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0f, 0f, 1.0f);
        }
    }

    public void OnMouseUp(){
        gameController = GameObject.FindGameObjectWithTag("GameController");
        if(attack){
            GameObject cp = gameController.GetComponent<GameController>().GetPosition(matrixX, matrixY);

            if(cp.name == "white_king") gameController.GetComponent<GameController>().Winner("black");
            if(cp.name == "black_king") gameController.GetComponent<GameController>().Winner("white");
            Destroy(cp);
        }
        gameController.GetComponent<GameController>().SetPositionEmpty(reference.GetComponent<PlayerController>().getXBoard(), 
        reference.GetComponent<PlayerController>().getYBoard());

        reference.GetComponent<PlayerController>().setXBoard(matrixX);
        reference.GetComponent<PlayerController>().setYBoard(matrixY);
        reference.GetComponent<PlayerController>().SetCoords();

        gameController.GetComponent<GameController>().SetPosition(reference);

        gameController.GetComponent<GameController>().NextTurn();

        reference.GetComponent<PlayerController>().PoolingMovePlate();
    }

    public void SetCoords(int x, int y){
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj){
        reference = obj;
    }

    public GameObject GetReference(){
        return reference;
    }
}
