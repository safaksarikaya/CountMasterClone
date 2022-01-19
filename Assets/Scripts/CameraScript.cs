using UnityEngine;
public class CameraScript : MonoBehaviour
{
  [SerializeField] Transform targetTransform;
  Vector3 distance;
  [SerializeField] float minX = -10f, maxX = 10f;
  private void Start()
  {
    distance = transform.position - targetTransform.position;
  }
  private void Update()
  {
    var position = Vector3.Slerp(transform.position, targetTransform.position + distance, 3 * Time.deltaTime);
    transform.position = new Vector3(Mathf.Clamp(position.x, minX, maxX), position.y, position.z);
  }
  public void ChangeTarget(Transform target)
  {
    targetTransform = target;
  }
}