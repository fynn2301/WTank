using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour {
    public static GameScene instance;
    // Objects
    public Vector2 directtionThisPlayer;
    public Vector2 directionOtherPlayer = Vector2.zero;
    // Text fields

    // Fields for interpolating the movement of an enemy
    Queue<Vector2> DirectionEnemyBuffer = new Queue<Vector2>();

    void Start() {
        Application.targetFrameRate = 30;
        if (instance == null) instance = this; // creating singleton
        //UpdateVievHealth();
        //UpdateScore();
        float timeRepeat = 1f / 30f;
        InvokeRepeating("MoveEnemy", timeRepeat, timeRepeat);
        InvokeRepeating("SendPosition", timeRepeat, timeRepeat);
    }
    private void Update()
    {
    }


    ////////////////////////
    // Methods for the tank of this device
    ////////////////////////

    // wrapping a player’s tank position into an array of bytes and transferring this array to an enemy device
    void SendPosition() {
        byte[] position = new byte[9];
        byte[] posX = new byte[4];
        byte[] posY = new byte[4];
        posX = NetworkManager.instance.FloatToBytes(directtionThisPlayer.x);
        posY = NetworkManager.instance.FloatToBytes(directtionThisPlayer.y);
        position[0] = 0;
        position[1] = posX[0];
        position[2] = posX[1];
        position[3] = posX[2];
        position[4] = posX[3];
        position[5] = posY[0];
        position[6] = posY[1];
        position[7] = posY[2];
        position[8] = posY[3];
        NetworkManager.instance.WriteMessage(position); // message transfer
    }

    // delay between shots
    //IEnumerator ShotPlayer() {
    //    while (true) {
    //        if (playerHealth > 0) {
    //            Vector2 vector = directtionThisPlayer;
    //            vector.x += 1;
    //            GameObject newBullet = Instantiate(bullet, vector, Quaternion.identity);
    //            newBullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 100);
    //            NetworkManager.instance.WriteMessage(new byte[1] { (byte)1 }); // message transfer
    //        }
    //        yield return new WaitForSeconds(0.3f);
    //    }
    //}

    /////////////////////////////
    // Commands from another device
    /////////////////////////////

    // If the buffer is not empty, then the movement of the enemy tank begins.
    // tank movement
    void MoveEnemy() {
        if (DirectionEnemyBuffer.Count > 0) directionOtherPlayer = DirectionEnemyBuffer.Dequeue();
        //if (enemyHealth > 0) {
            
        //}
    }

    // The resulting positions are converted from an array of bytes to coordinates of type Vector2 and added to the buffer.
    public void PutInBufferDirection(byte[] position) {
        Vector2 currentPosition;
        byte[] posX = new byte[4];
        byte[] posY = new byte[4];
        posX[0] = position[1];
        posX[1] = position[2];
        posX[2] = position[3];
        posX[3] = position[4];
        posY[0] = position[5];
        posY[1] = position[6];
        posY[2] = position[7];
        posY[3] = position[8];
        currentPosition.x = NetworkManager.instance.BytesToFloat(posX);
        currentPosition.y = NetworkManager.instance.BytesToFloat(posY);
        DirectionEnemyBuffer.Enqueue(currentPosition);
    }
    // shot of an enemy tank
    //public void ShotEnemy() {
    //    Vector2 vector = joystickInputOtherPlayer.transform.position;
    //    vector.x -= 1;
    //    GameObject newBullet = Instantiate(bullet, vector, Quaternion.identity);
    //    newBullet.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * 100);
    //}

    // hitting the player's tank
    //public void HitByPlayer(byte health) {
    //    playerHealth = (int)health;
    //    UpdateVievHealth();
    //}

    // player tank destruction
    //public void DestroyTankPlayer() {
    //    joystickInputThisPlayer.transform.position = new Vector2(-3.256f, 20);
    //    playerHealth = 0;
    //    playerDeath++;
    //    UpdateVievHealth();
    //    UpdateScore();
    //}

    // the return of the player’s tank to the starting position
    //public void ReturnPlayerToStartingPosion() {
    //    joystickInputThisPlayer.transform.position = new Vector2(-3.256f, 0);
    //    frame = 0;
    //    playerHealth = 100;
    //    UpdateVievHealth();
    //}


    // View
    //void UpdateVievHealth() {
    //    playerHealthText.text = playerHealth + "%";
    //    enemyHealthText.text = enemyHealth + "%";
    //}
    //void UpdateScore() {
    //    scoreText.text = playerDeath + ":" + enemyDeath;
    //}
    // Input
    public void SetDirectionOfThisPlayer(Vector2 direction) {
        directtionThisPlayer = direction;
    }
    public void ExitScene() {
        NetworkManager.instance.ExitGameScene();
    }
}
