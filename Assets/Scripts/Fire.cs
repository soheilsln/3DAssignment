using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(DestroyFire(GameManager.instance.GetFireDuration()));
    }

    private IEnumerator DestroyFire(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

}
