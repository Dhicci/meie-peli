    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int turn = 1;
    public GameObject[] players;
    public GameObject[] enemies;
    public TextMeshProUGUI turnText;

    public GameObject winImage;
    public GameObject loseImage;
    public GameObject blackScreen;
    public GameObject backButton;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayerTurn());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator PlayerTurn()
    {
        turnText.text = "Player turn";
        turnText.color = Color.white;
        turn = 1;
        foreach (GameObject player in players)
        {
            player.GetComponent<playerScript>().energy = 3;
            player.GetComponent<playerScript>().myTurn = true;
            yield return new WaitUntil(player.GetComponent<playerScript>().MyTurn);
        }
        yield return new WaitForSeconds(0.5f);
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        if (enemies.Length == 0)
        {
            GameOver(true);
        } else
        {
            EnemyTurn();
        }
    }

    public void EnemyTurn()
    {
        turnText.text = "Enemy turn";
        turnText.color = Color.red;
        turn = 2;
        if (enemies.Length == 0)
        {
            GameOver(true);
            return;
        }
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<enemy_controller>().energy = 2;
            enemy.GetComponent<enemy_controller>().DoAction();
        }
        StartCoroutine(WaitforSeconds());
    }

    public void NewTurn()
    {
        StartCoroutine(PlayerTurn());
    }

    IEnumerator WaitforSeconds()
    {
        yield return new WaitForSeconds(1f);
        NewTurn();
    }

    public void GameOver(bool victory)
    {
        if (victory)
        {
            //Display victory
            Debug.Log("victory");
            winImage.SetActive(true);
        } else
        {
            //Display loss
            Debug.Log("Defeat");
            loseImage.SetActive(true);
        }
        backButton.SetActive(true);
        blackScreen.SetActive(true);
    }

}
