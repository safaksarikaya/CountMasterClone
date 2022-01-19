using UnityEngine;
using UnityEngine.Pool;
public class ObjectPoolScript : MonoBehaviour
{
  public static ObjectPoolScript Instance;
  [SerializeField] DummyScript dummy;
  private ObjectPool<DummyScript> _dummyPool;
  private void Awake()
  {
    Instance = this;

     _dummyPool = new ObjectPool<DummyScript>(() =>
    {
      return Instantiate(dummy);
    }, dummy =>
    {
      dummy.gameObject.SetActive(true);
    }, dummy =>
    {
      dummy.gameObject.SetActive(false);
    }, dummy =>
    {
      Destroy(dummy.gameObject);
    }, false, 10, 150);
  }
  public DummyScript GetDummy()
  {
    return _dummyPool.Get();
  }
  public void RelaseDummy(DummyScript d)
  {
    _dummyPool.Release(d);
  }
}