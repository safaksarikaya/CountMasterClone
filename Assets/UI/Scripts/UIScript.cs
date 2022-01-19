using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIScript : MonoBehaviour
{
  public static UIScript Instance;
  [Header("PANELS")]
  [SerializeField] GameObject gamePanel;
  [SerializeField] GameObject gameOverPanel;
  [SerializeField] GameObject gameSuccessPanel;
  [SerializeField] GameObject coinPanel;
  [Header("COMPONENTS")]
  [SerializeField] Text coinText;
  [SerializeField] GameObject levelText;
  [SerializeField] GameObject levelProgress;
  [SerializeField] Image levelProgressBarImage;
  [SerializeField] GameObject tap2Start;
  private void Awake()
  {
    Instance = this;
  }
  private void Start()
  {
    StartGame();
  }
  void StartGame()
  {
    gameSuccessPanel.SetActive(false);
    gameOverPanel.SetActive(false);
    gamePanel.SetActive(true);
    LevelText();
    ChangeCoinText();
    Tap2StartSetActive(true);
  }
  public void GameOver()
  {
    gamePanel.SetActive(false);
    gameSuccessPanel.SetActive(false);
    gameOverPanel.SetActive(true);
  }
  public void GameSuccess()
  {
    gamePanel.SetActive(false);
    gameOverPanel.SetActive(false);
    gameSuccessPanel.SetActive(true);
    PlayerInfoScript.Instance.SaveInfo();
  }
  public void AddCoin(int amount = 1)
  {
    PlayerInfoScript.Instance.SetCoin(amount);
    ChangeCoinText();
  }
  void ChangeCoinText()
  {
    coinText.text = PlayerInfoScript.Instance.GetCoin().ToString();
  }
  public void LevelText()
  {
    levelText.transform.GetChild(0).transform.GetComponent<Text>().text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex).ToString();
  }
  public void LevelProgressSetActive(bool active = false)
  {
    if (!active)
      levelProgress.SetActive(false);
    else
      levelProgress.SetActive(true);
  }
  public void ChangeLevelProgressValue(float amount = 0)
  {
    levelProgressBarImage.fillAmount = amount;
  }
  public void RestartLevel()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
  public void NextLevel()
  {
    var next = SceneManager.GetActiveScene().buildIndex + 1;
    SceneManager.LoadScene(next < SceneManager.sceneCount ? next : SceneManager.GetActiveScene().buildIndex);
    
  }
  public void Tap2StartSetActive(bool active = false)
  {
    if (!active)
      tap2Start.SetActive(false);
    else
      tap2Start.SetActive(true);
  }
}