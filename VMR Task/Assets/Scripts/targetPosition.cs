using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetPosition : MonoBehaviour {

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
