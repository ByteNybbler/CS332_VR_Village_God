// Author(s): Paul Calande
// Class for the Mountain God game controller.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Tooltip("Reference to the enemy controller.")]
    public EnemyController enemyController;
    [Tooltip("Reference to the wave controller.")]
    public WaveController waveController;
    [Tooltip("Reference to the village.")]
    public Village village;

    // The number of enemies that have been killed so far.
    private int enemiesKilled = 0;
    // Reference to the stats screen's ImageFade component.
    private ImageFade imageFade;

    private void Start()
    {
        village.AllVillagersDied += Village_AllVillagersDied;
        enemyController.EnemyDied += EnemyController_EnemyDied;

        SceneManager.activeSceneChanged += SceneManager_ActiveSceneChanged;
    }

    private void OnDestroy()
    {
        if (village != null)
        {
            village.AllVillagersDied -= Village_AllVillagersDied;
        }
        if (enemyController != null)
        {
            enemyController.EnemyDied -= EnemyController_EnemyDied;
        }
        if (imageFade != null)
        {
            imageFade.AlphaHitMax -= ImageFade_AlphaHitMax;
        }
        SceneManager.activeSceneChanged -= SceneManager_ActiveSceneChanged;
    }

    private void GameOver()
    {
        //Debug.Log("Game over, baby.");
        // Preserve this object for the statistics in the game over scene.
        DontDestroyOnLoad(gameObject);
        // Go to the new scene.
        SceneManager.LoadScene("GameOver");
    }

    // Return to the actual game.
    private void EndGameOver()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Environment");
    }

    private void SceneManager_ActiveSceneChanged(Scene a, Scene b)
    {
        // Get the stats screen.
        GameObject stats = GameObject.Find("StatsScreen");
        // Get the game over input component.
        GameOverInput goi = stats.GetComponent<GameOverInput>();
        // Get the image fade and subscribe to it.
        imageFade = goi.imageFade;
        imageFade.AlphaHitMax += ImageFade_AlphaHitMax;
        // Set the stats text values appropriately.
        goi.textWaves.text = "You made it to wave " + waveController.GetCurrentWave() + ".";
        goi.textEnemiesKilled.text = "You defeated " + enemiesKilled + " attackers.";
    }

    // Enemy death event callback.
    private void EnemyController_EnemyDied(EnemyStatus enemy, int xp)
    {
        /*
        // Increment XP.
        compLevels.AddXP(xp);
        // Spawn XP text at the enemy position.
        compRisingTextCreatorXP.message = "+" + xp + " " + xpString;
        compRisingTextCreatorXP.CreateRisingText(enemy.transform.position);
        */

        ++enemiesKilled;
    }

    // Village "no more villagers" event callback.
    private void Village_AllVillagersDied()
    {
        GameOver();
    }

    // Image fade in complete event callback.
    private void ImageFade_AlphaHitMax()
    {
        EndGameOver();
    }
}