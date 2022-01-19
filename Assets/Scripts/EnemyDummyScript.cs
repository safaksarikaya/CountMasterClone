using UnityEngine;
using System.Collections.Generic;
public class EnemyDummyScript : MonoBehaviour
{
  [SerializeField] Transform dummies;
  [SerializeField] DummyCounterCanvasScript dummyCounterCanvasScript;
  [SerializeField] int dummyCount = 1;
  CapsuleCollider capsuleCollider;
  List<DummyScript> dummyScriptList = new List<DummyScript>();
  private void Start()
  {
    capsuleCollider = GetComponent<CapsuleCollider>();
    for (int i = 0; i < dummyCount; i++)
    {
      var dummy = ObjectPoolScript.Instance.GetDummy();
      dummy.transform.parent = dummies;
      dummy.transform.localPosition = Vector3.zero;
      dummy.isEnemyDummy = true;
      dummy.enemyDummyScript = this;
      dummy.SetColor();
      dummyScriptList.Add(dummy);
    }
    SetCountText(dummyScriptList.Count);
    CirclePlaceDummy();
    SortByLocalPosZ();
  }
  public void AttackDummies(List<DummyScript> targetDummies)
  {
    if (targetDummies.Count <= dummyScriptList.Count)
    {
      for (int i = 0; i < targetDummies.Count; i++)
      {
        dummyScriptList[i].GoTarget(targetDummies[targetDummies.Count - 1 - i].transform.position, 1f, false, targetDummies[targetDummies.Count - 1 - i], i * .015f);
      }
    }
    else
    {
      for (int i = 0; i < dummyScriptList.Count; i++)
      {
        dummyScriptList[i].GoTarget(targetDummies[targetDummies.Count - 1 - i].transform.position, 1f, false, targetDummies[targetDummies.Count - 1 - i], i * .015f);
      }
    }
  }
  public void SetCountText(int val = 1)
  {
    dummyCounterCanvasScript.SetText(val.ToString());
  }
  public void RemoveDummy(DummyScript dummy)
  {
    dummyScriptList.Remove(dummy);
    ObjectPoolScript.Instance.RelaseDummy(dummy);
    SetCountText(dummyScriptList.Count);
    if (dummyScriptList.Count == 0)
    {
      CharacterScript.Instance.Continue();
      Destroy(this.gameObject);
    }
  }
  void CirclePlaceDummy()
  {
    float radius = .75f;
    float dummyCountInCircle = 4;
    float dummyIndex = 0;
    float circleCount = 1;
    for (int i = 0; i < dummyCount; i++)
    {
      float angle = dummyIndex++ * Mathf.PI * 2f / dummyCountInCircle;
      dummies.transform.GetChild(i).transform.localPosition = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
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
    dummyScriptList.Sort((p1, p2) => p1.transform.localPosition.z.CompareTo(p2.transform.localPosition.z));
  }
}