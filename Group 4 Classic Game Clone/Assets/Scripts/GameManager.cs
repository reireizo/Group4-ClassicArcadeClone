using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Enemy[] enemies;

    public Player player;

    public Transform junk;

    public int score { get; private set; }
    public int lives { get; private set; }
    public int enemyFactor { get; private set; } = 1;

    public TMP_Text livesText;
    public TMP_Text scoreText;
    public TMP_Text deathMessage;
    public TMP_Text gameOverText;
    public Image gameOverBG;
    public Image deathMessageBG;

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        livesText.SetText("Lives: " + lives);
        scoreText.SetText("Score: " + score);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (this.lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);

        this.deathMessage.enabled = false;
        this.deathMessageBG.enabled = false;
        this.gameOverText.enabled = false;
        this.gameOverBG.enabled = false;

        NewRound();
    }

    private void NewRound()
    {
        foreach (Transform junk in this.junk)
        {
            junk.gameObject.SetActive(true);
        }

        ResetBoard();
    }

    private void ResetBoard()
    {
        ResetEnemyFactor();
        this.deathMessage.enabled = false;
        this.deathMessageBG.enabled = false;

        for (int i = 0; i < this.enemies.Length; i++)
        {
            this.enemies[i].ResetState();
        }

        this.player.ResetState();
    }

    private void GameOver()
    {
        for (int i = 0; i < this.enemies.Length; i++)
        {
            this.enemies[i].gameObject.SetActive(false);
        }

        this.player.gameObject.SetActive(false);

        this.deathMessage.enabled = false;
        this.deathMessageBG.enabled = false;

        this.gameOverText.enabled = true;
        this.gameOverBG.enabled = true;
    }

    private void SetScore(int score)
    {
        this.score = score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void EnemyDefeated(Enemy enemies)
    {
        int points = enemies.points * this.enemyFactor;
        SetScore(this.score + points);
        this.enemyFactor++;
    }

    public void PlayerDefeated()
    {
        this.player.gameObject.SetActive(false);
        this.deathMessageBG.enabled = true;
        this.deathMessage.enabled = true;

        SetLives (this.lives - 1);
        if (this.lives > 0)
        {
            Invoke(nameof(ResetBoard), 3.0f);
        }
        else
        {
            GameOver();
        }
    }

    public void JunkCollected(Junk junk)
    {
        junk.gameObject.SetActive(false);

        SetScore(this.score + junk.points);

        if (!HasRemainingJunk())
        {
            this.player.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerUpCollected(PowerUp junk)
    {
        for (int i = 0; i < this.enemies.Length; i++)
        {
            this.enemies[i].frightened.Enable(junk.duration);
        }
        JunkCollected(junk);
        CancelInvoke();
        Invoke(nameof(ResetEnemyFactor), junk.duration);
    }

    private bool HasRemainingJunk()
    {
        foreach (Transform junk in this.junk)
        {
            if (junk.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetEnemyFactor()
    {
        this.enemyFactor = 1;
    }
}
