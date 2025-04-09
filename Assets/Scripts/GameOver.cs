using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
  public TMP_Text coinText;
  
  public void Setup(int scoreCoin)
  {
    gameObject.SetActive(true);
    coinText.text = scoreCoin + (scoreCoin != 1 ? " coins" : " coin");
  }

  public void RestartGame()
  {
    Debug.Log("Restarting game");
    var currentScene = SceneManager.GetActiveScene();
    Debug.Log(currentScene.name);
    SceneManager.LoadScene(currentScene.name);
  }
}
