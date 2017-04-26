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
    [Tooltip("Reference to the stats GameObject.")]
    public GameObject stats;
    [Tooltip("Reference to the waves text.")]
    public Text textWaves;
    [Tooltip("Reference to the enemies killed text.")]
    public Text textEnemiesKilled;
    [Tooltip("Reference to the ImageFade component that takes the player back to the main scene.")]
    public ImageFade imageFade;

    private int enemiesKilled = 0;

    /*
    [Tooltip("The string that is the icon for XP.")]
    public string xpString = "XP";

    // Component references.
    private XPLevels compLevels;
    private RisingTextCreator compRisingTextCreatorXP;
    */

    //private void Awake()
    //{
        /*
        compLevels = GetComponent<XPLevels>();
        compRisingTextCreatorXP = GetComponent<RisingTextCreator>();
        */
    //}

    private void Start()
    {
        stats.SetActive(false);

        village.AllVillagersDied += Village_AllVillagersDied;
        enemyController.EnemyDied += EnemyController_EnemyDied;
        imageFade.AlphaHitMax += ImageFade_AlphaHitMax;

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

        // Set the stats text values appropriately.
        textWaves.text = "You made it to wave " + waveController.GetCurrentWave() + ".";
        textEnemiesKilled.text = "You defeated " + enemiesKilled + " attackers.";

        // Preserve this object for the statistics in the game over scene.
        DontDestroyOnLoad(gameObject);
        // Go to the new scene.
        SceneManager.LoadScene("EmptyScene");
    }

    // Return to the actual game.
    private void EndGameOver()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Environment");
    }

    private void SceneManager_ActiveSceneChanged(Scene a, Scene b)
    {
        // Enable the stats.
        stats.SetActive(true);
    }

    // Enemy death event callback.
    private void EnemyController_EnemyDied(GameObject enemy, int xp)
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