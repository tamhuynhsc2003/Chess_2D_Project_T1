using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject chesspiece_prefab;
    private GameObject [,] position = new GameObject[8, 8];
    private GameObject [] playerBlack = new GameObject[16];
    private GameObject [] playerWhite = new GameObject[16];

    private string currentPlayer ="White";
    private bool gameOver = false;

    public AudioSource audioSource; // Nơi phát âm thanh
    public AudioClip audioClip;

    void Start()
    {
        playerWhite = new GameObject[]{
            Create("white_rook",0 ,0), Create("white_knight",1 ,0), Create("white_bishop",2 ,0), 
            Create("white_queen",3 ,0), Create("white_king",4 ,0), Create("white_bishop",5 ,0),
            Create("white_knight",6 ,0), Create("white_rook",7 ,0), 
            Create("white_pawn",0 ,1), Create("white_pawn",1 ,1), Create("white_pawn",2 ,1), Create("white_pawn",3 ,1),
            Create("white_pawn",4 ,1), Create("white_pawn",5 ,1), Create("white_pawn",6 ,1), Create("white_pawn",7 ,1)
        };

        playerBlack = new GameObject[]{
            Create("black_rook",0 ,7), Create("black_knight",1 ,7), Create("black_bishop",2 ,7), 
            Create("black_queen",3 ,7), Create("black_king",4 ,7), Create("black_bishop",5 ,7),
            Create("black_knight",6 ,7), Create("black_rook",7 ,7), 
            Create("black_pawn",0 ,6), Create("black_pawn",1 ,6), Create("black_pawn",2 ,6), Create("black_pawn",3 ,6),
            Create("black_pawn",4 ,6), Create("black_pawn",5 ,6), Create("black_pawn",6 ,6), Create("black_pawn",7 ,6)
        };

        for(int i = 0; i < playerWhite.Length; i++){
            SetPosition(playerWhite[i]);
            SetPosition(playerBlack[i]);
        }
    }

    public GameObject Create(string name, int x, int y){
        GameObject obj = Instantiate(chesspiece_prefab, new Vector3(x, y,-2), Quaternion.identity);
        PlayerController pc = obj.GetComponent<PlayerController>();
        pc.name = name;
        pc.setXBoard(x);
        pc.setYBoard(y);
        pc.Activate(); 
        return obj;
    }

    public void SetPosition(GameObject obj){
        PlayerController pc = obj.GetComponent<PlayerController>();

        position[pc.getXBoard(), pc.getYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y){
        position[x, y] = null;
    }

    public GameObject GetPosition(int x, int y){
        return position[x, y];
    }

    public bool PositionOnBoard(int x, int y){
        if(x < 0 || y < 0 || x >= position.GetLength(0) || y >= position.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer(){
        return currentPlayer;
    }

    public bool IsGameOver(){
        return gameOver;
    }

    public void NextTurn(){
        if(currentPlayer == "White"){
            currentPlayer = "Black";
        }else{
            currentPlayer = "White";
        }
    }

    public void Update(){
        if(gameOver == true && Input.GetMouseButton(0)){
            gameOver = false;

            SceneManager.LoadScene("Main_Scene");
        }
    }

    public void Winner(string playerWinner){
        gameOver = true;

        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the Winner";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("GameOverPanel").GetComponent<Image>().enabled = true;
    }

    public void PlayAudio()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource hoặc AudioClip chưa được gán!");
        }
    }
}
