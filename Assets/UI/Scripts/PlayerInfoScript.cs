using UnityEngine;

public class PlayerInfoScript : MonoBehaviour
{
  public static PlayerInfoScript Instance;
  private void Awake()
  {
    Instance = this;
  }
  public void SetCoin(int amount = 0)
  {
    PlayerPrefs.SetInt("coin", GetCoin() + amount);
  }
  public int GetCoin()
  {
    var coin = 0;
    if (PlayerPrefs.HasKey("coin"))
    {
      coin = PlayerPrefs.GetInt("coin");
    }
    return coin;
  }
  public void SaveInfo()
  {
    PlayerPrefs.Save();
  }
}
