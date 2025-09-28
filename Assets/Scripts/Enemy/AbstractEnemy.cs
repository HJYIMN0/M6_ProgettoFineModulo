using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractEnemy;

public class AbstractEnemy : MonoBehaviour
{
    [SerializeField] protected SO_EnemyData enemyData;



    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        switch (enemyData.GizmoShape)
        {
            case CheckerShape.Sphere:
                Gizmos.DrawWireSphere(transform.position, enemyData.GizmoRadius);
                break;

            case CheckerShape.Box:
                Gizmos.DrawWireCube(transform.position, Vector3.one * enemyData.GizmoRadius);
                break;

            case CheckerShape.RectangularBox:
                Vector3 size = new Vector3(3, 1 * enemyData.GizmoRadius, 3);
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(Vector3.zero, size);
                break;
        }
    }


    public virtual bool IsPlayerInRange(out GameObject player)
    {
        Collider[] playersInRange = null;

        switch (enemyData.GizmoShape)
        {
            case CheckerShape.Sphere:
                playersInRange = Physics.OverlapSphere(transform.position, enemyData.GizmoRadius, enemyData.PlayerLayer);
                break;

            case CheckerShape.Box:
                playersInRange = Physics.OverlapBox(transform.position, Vector3.one * enemyData.GizmoRadius, Quaternion.identity, enemyData.PlayerLayer);
                break;

            case CheckerShape.RectangularBox:
                Vector3 boxSize = new Vector3(3, 1 * enemyData.GizmoRadius, 3);
                playersInRange = Physics.OverlapBox(transform.position, boxSize * 0.5f, transform.rotation, enemyData.PlayerLayer);
                break;
        }

        if (playersInRange != null && playersInRange.Length > 0)
        {
            player = playersInRange[0].gameObject;
            return true;
        }

        player = null;
        return false;
    }
}

