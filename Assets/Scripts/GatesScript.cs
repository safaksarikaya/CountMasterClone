using UnityEngine;
using System.Collections.Generic;
public class GatesScript : MonoBehaviour
{
  [SerializeField] int leftGateValue = 1;
  [SerializeField] GateOperator leftGateOperator;
  [SerializeField] int rightGateValue = 1;
  [SerializeField] GateOperator rightGateOperator;
  [SerializeField] TextMesh lefGateTextMesh, rightGateTextMesh;
  [SerializeField] Transform leftGatePlane, rightGatePlane;
  private void Start()
  {
    lefGateTextMesh.text = (leftGateOperator == GateOperator.Add ? "+" : leftGateOperator == GateOperator.Subtract ? "-" : leftGateOperator == GateOperator.Multiply ? "x" : "/") + leftGateValue;
    rightGateTextMesh.text = (rightGateOperator == GateOperator.Add ? "+" : rightGateOperator == GateOperator.Subtract ? "-" : rightGateOperator == GateOperator.Multiply ? "x" : "/") + rightGateValue;
  }
  enum GateOperator
  {
    Add,
    Subtract,
    Multiply,
    Divide
  }
  public int GetGateVal(bool isLeft = true)
  {
    return isLeft ? leftGateValue : rightGateValue;
  }
  public string GetGateOperator(bool isLeft = true)
  {
    return isLeft ? leftGateOperator.ToString() : rightGateOperator.ToString();
  }
  public void CloseGate(bool left = true)
  {
    if (left)
      leftGatePlane.gameObject.SetActive(false);
    else
      rightGatePlane.gameObject.SetActive(false);
  }
}