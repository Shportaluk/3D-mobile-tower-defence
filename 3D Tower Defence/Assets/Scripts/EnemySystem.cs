using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    public EnemyData enemyData;
    void Start()
    {
        Debug.Log(enemyData.MoveSpeed);
    }

    void Update()
    {
        
    }
}
