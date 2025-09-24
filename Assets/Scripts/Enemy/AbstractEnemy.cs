using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbstractEnemy;

public class AbstractEnemy : MonoBehaviour
{
    [SerializeField] protected SO_EnemyData enemyData;



    private void OnDrawGizmos()
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
        }
    }

    public bool IsPlayerInRange(out GameObject player)
    {
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, enemyData.GizmoRadius, enemyData.PlayerLayer);

        if (playersInRange.Length > 0)
        {
            player = playersInRange[0].gameObject;
            return true;
        }

        player = null;
        return false;
    }
}

