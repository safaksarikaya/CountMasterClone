using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
  public bool isEnemyDummy;
  public EnemyDummyScript enemyDummyScript;
  [SerializeField] Color enemyColor, normalColor;
  [SerializeField] Renderer dummyRenderer;
  bool goTarget, isLocalLerp;
  Vector3 startPos, targetPos;
  float timer, goTargetSpeed = 1;
  DummyScript targetDummy;
  public ParticleSystem dieParticleSystem;
  private void Start()
  {
    SetColor();
  }
  private void Update()
  {
    if (goTarget)
    {
      timer += Time.deltaTime * goTargetSpeed;
      if (isLocalLerp)
      {
        transform.localPosition = Vector3.Lerp(startPos, targetPos, timer);
      }
      else
      {
        transform.position = Vector3.Lerp(startPos, targetPos, timer);
      }
      if (timer >= 1f)
      {
        timer = 0;
        goTarget = false;
      }
    }
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<DummyScript>() && isEnemyDummy && !other.GetComponent<DummyScript>().isEnemyDummy && other.GetComponent<DummyScript>() == targetDummy)
    {
      var ds = other.GetComponent<DummyScript>();
      PlayDieParticle();
      ds.PlayDieParticle();
      enemyDummyScript.RemoveDummy(this);
      CharacterScript.Instance.RemoveDummy(ds);
    }
  }
  public void SetColor()
  {
    var p = dieParticleSystem.main;
    if (isEnemyDummy)
    {
      dummyRenderer.material.color = enemyColor;
      p.startColor = enemyColor;
    }
    else
    {
      dummyRenderer.material.color = normalColor;
      p.startColor = normalColor;
    }
  }
  public void GoTarget(Vector3 targetPos, float speed = 1, bool isLocal = true, DummyScript targetDummy = null, float delay = 0)
  {
    startPos = (isLocal ? transform.localPosition : transform.position);
    this.targetPos = targetPos;
    this.targetDummy = targetDummy;
    timer = 0;
    goTargetSpeed = speed;
    isLocalLerp = isLocal;
    Invoke("GoTarget", delay);
  }
  void GoTarget()
  {
    goTarget = true;
  }
  public void PlayDieParticle()
  {
    var p = Instantiate(dieParticleSystem.gameObject, dieParticleSystem.transform.position, Quaternion.identity, this.transform);
    p.transform.parent = null;
    p.GetComponent<ParticleSystem>().Play();
    Destroy(p, 1.2f);
  }
}