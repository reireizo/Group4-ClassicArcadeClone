using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Enemy[] enemies;

    public Player player;

    public Transform junk;

    public int score { get; private set; }
    public int lives { get; private set; }
    public int enemyFactor { get; private set; } = 1;

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (this.lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
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
