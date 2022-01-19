using System.Collections.Generic;
using UnityEngine;
public class CharacterScript : MonoBehaviour
{
  public static CharacterScript Instance;
  [SerializeField] float speed;
  [SerializeField] Transform dummies;
  [SerializeField] DummyCounterCanvasScript dummyCounterCanvasScript;
  int dummyCount = 1;
  List<DummyScript> dummyList = new List<DummyScript>();
  float mouseX, _speed;
  bool tap2Start;
  CapsuleCollider capsuleCollider;
  private void Awake()
  {
    Instance = this;
  }
  private void Start()
  {
    _speed = speed;
    dummyList.Add(dummies.GetChild(0).GetComponent<DummyScript>());
    dummyCounterCanvasScript.SetText(dummyCount.ToString());
    capsuleCollider = GetComponent<CapsuleCollider>();
  }
  private void Update()
  {
    if (!tap2Start)
    {
      if (Input.GetMouseButtonDown(0))
      {
        tap2Start = true;
        UIScript.Instance.Tap2StartSetActive(false);
      }
      return;
    }
    if (Input.GetMouseButton(0))
    {
      if (Input.GetAxis("Mouse X") < 0)
      {
        mouseX = -1;
      }
      if (Input.GetAxis("Mouse X") > 0)
      {
        mouseX = 1;
      }
    }
    if (Input.GetMouseButtonUp(0))
    {
      mouseX = 0;
    }
    transform.position = new Vector3(Mathf.Clamp(transform.position.x + (mouseX * Time.deltaTime) * _speed, -2.5f, 2.5f), transform.position.y, transform.position.z + (Time.deltaTime * _speed));
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<GatesScript>())
    {
      var gateScript = other.GetComponent<GatesScript>();
      gateScript.GetComponent<Collider>().enabled = false;
      if (transform.position.x < gateScript.transform.position.x)
      {
        InGate(gateScript.GetGateVal(true), gateScript.GetGateOperator(true));
        gateScript.CloseGate(true);
      }
      else
      {
        InGate(gateScript.GetGateVal(false), gateScript.GetGateOperator(false));
        gateScript.CloseGate(false);
      }
    }
    if (other.GetComponent<EnemyDummyScript>())
    {
      _speed = 0;
      SortByLocalPosZ();
      other.GetComponent<EnemyDummyScript>().AttackDummies(dummyList);
    }
    if (other.CompareTag("Finish"))
    {
      _speed = 0;
      UIScript.Instance.GameSuccess();
    }
  }
  void InGate(int val = 1, string gateOperator = "Add")
  {
    var startVal = dummyCount;
    if (gateOperator == "Add")
    {
      dummyCount += val;
      AddDummy(startVal, dummyCount);
      dummyCounterCanvasScript.GateText("+" + val.ToString());

    }
    else if (gateOperator == "Subtract")
    {
      dummyCount -= val;
      RemoveDummy(startVal, dummyCount);
      dummyCounterCanvasScript.GateText("-" + val.ToString());
    }
    else if (gateOperator == "Multiply")
    {
      dummyCount *= val;
      AddDummy(startVal, dummyCount);
      dummyCounterCanvasScript.GateText("x" + val.ToString());
    }
    else if (gateOperator == "Divide")
    {
      dummyCount /= val;
      RemoveDummy(startVal, dummyCount);
      dummyCounterCanvasScript.GateText("/" + val.ToString());
    }
    dummyCounterCanvasScript.SetText(dummyCount.ToString());
    PlaceDummy();
    SortByLocalPosZ();
  }
  void AddDummy(int startValue, int endValue)
  {
    for (int i = startValue; i < endValue; i++)
    {
      var dummy = ObjectPoolScript.Instance.GetDummy();
      dummy.transform.parent = dummies;
      dummyList.Add(dummy);
      dummy.transform.localPosition = Vector3.zero;
      dummy.isEnemyDummy = false;
      dummy.SetColor();
    }
  }
  void RemoveDummy(int startValue, int endValue)
  {
    if (endValue < 0)
      endValue = 0;
    for (int i = startValue - 1; i >= endValue; i--)
    {
      RemoveDummy(dummyList[i]);
    }
    dummyCounterCanvasScript.SetText(dummyList.Count.ToString());
    if (endValue == 0)
    {
      GameOver();
    }
  }
  void PlaceDummy()
  {
    float radius = .75f;
    float dummyCountInCircle = 4;
    float dummyIndex = 0;
    float circleCount = 1;
    for (int i = 0; i < dummyCount; i++)
    {
      float angle = dummyIndex++ * Mathf.PI * 2f / dummyCountInCircle;
      var pos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
      dummyList[i].GoTarget(pos, 4, true);
      if (dummyIndex == dummyCountInCircle)
      {
        circleCount++;
        dummyIndex = 0;
        radius += 1f;
        dummyCountInCircle += dummyCountInCircle;
      }
    }
    capsuleCollider.radius = circleCount + .5f;
  }
  void SortByLocalPosZ()
  {
    dummyList.Sort((p1, p2) => p1.transform.localPosition.z.CompareTo(p2.transform.localPosition.z));
  }
  void ResetSpeed()
  {
    _speed = speed;
  }
  public void Continue()
  {
    ResetSpeed();
    PlaceDummy();
  }
  public void RemoveDummy(DummyScript dummy)
  {
    dummyList.Remove(dummy);
    ObjectPoolScript.Instance.RelaseDummy(dummy);
    dummyCount = dummyList.Count;
    dummyCounterCanvasScript.SetText(dummyCount.ToString());
    if (dummyCount == 0)
    {
      GameOver();
    }
  }
  void GameOver()
  {
    dummyCounterCanvasScript.gameObject.SetActive(false);
    _speed = 0;
    UIScript.Instance.GameOver();
  }
}