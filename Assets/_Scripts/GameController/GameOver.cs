// Author(s): Paul Calande
// Return the player to the main scene after a short amount of time.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [Tooltip("How many seconds to remain within the game over scene.")]
    public float gameOverTime;
    [Tooltip("The name of the scene to return to once the timer runs out.")]
    public string mainSceneName;

    private void Start()
    {
        StartCoroutine(ReturnToGame());
    }

    IEnumerator ReturnToGame()
    {
        yield return new WaitForSeconds(gameOverTime);
        SceneManager.LoadScene(mainSceneName);
    }
}