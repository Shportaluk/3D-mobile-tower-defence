using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Create Enemy")]
public class EnemyData : ScriptableObject
{
    [Tooltip("Move Speed"), Range(0.1f, 10f)]
    public float MoveSpeed;

    [Tooltip("Health"), Range(1f, 1500f)]
    public float Health;

    [Tooltip("Enemy Damage"), Range(1f, 5000f)]
    public float EnemyDamage;
}
