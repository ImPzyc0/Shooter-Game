using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDelete : MonoBehaviour
{
    [SerializeField] float timeToDelete;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(DeleteSelf());
    }

    IEnumerator DeleteSelf()
    {
        yield return new WaitForSeconds(timeToDelete);

        Destroy(gameObject);

    }
}
