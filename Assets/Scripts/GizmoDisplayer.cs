using UnityEngine;
using System.Collections;

public class GizmoDisplayer : MonoBehaviour {

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, transform.name);
    }
}
