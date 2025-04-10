using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Victory : MonoBehaviour
{
    public TMP_Text coinText;
  
    public void Setup(int scoreCoin)
    {
        gameObject.SetActive(true);
        coinText.text = scoreCoin + (scoreCoin != 1 ? " coins" : " coin");
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
