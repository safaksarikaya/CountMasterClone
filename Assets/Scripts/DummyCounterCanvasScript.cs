using UnityEngine;
using UnityEngine.UI;
public class DummyCounterCanvasScript : MonoBehaviour
{
  [SerializeField] Text counterText;
  [SerializeField] GameObject gateText;
  public void SetText(string text = "1")
  {
    counterText.text = text;
  }
  public void GateText(string text)
  {
    var t = Instantiate(gateText, this.transform);
    t.GetComponent<Text>().text = text;
    Destroy(t, 1f);
  }
}