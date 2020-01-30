using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    private EnemyData _enemyData;
    public EnemyData enemyData;
    public GameObject prefabExplosion;

    private Vector3[] _corners;
    private int _numCorner = 0;
    private Rigidbody _rigidbody;
    public float speed = 5f;
    void Start()
    {
        _enemyData = Instantiate( enemyData );
        _rigidbody = GetComponent<Rigidbody>();
        Transform pointsTrasform = GameObject.Find("Points").transform;
        int childCount = pointsTrasform.childCount;
        _corners = new Vector3[pointsTrasform.childCount];
        for (int i = 0; i < childCount; i++)
        {
            _corners[i] = pointsTrasform.GetChild(i).position;
        }
    }

    void RotateTo(Vector3 target)
    {
        target.y = transform.position.y;
        var targetDir = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetDir, 5f * Time.deltaTime);
    }
    float GetDistanceTo(Vector3 target)
    {
        return Vector3.Distance(transform.position, target);
    }
    void Move()
    {
        _rigidbody.MovePosition( transform.position + transform.forward * Time.fixedDeltaTime * speed);
    }
    public void DamageCastle()
    {

    }
    public void DamageMe( TowersSystem towersSystemDamager, float damage)
    {
        //Debug.Log(_enemyData.Health + " " + damage);
        _enemyData.Health -= damage;
        if(_enemyData.Health <= 0)
        {
            towersSystemDamager.DeleteTargetByInstaceId(GetInstanceID());
            Die();
        }
    }
    void Die()
    {
        Destroy(Instantiate(prefabExplosion, transform.position, Quaternion.identity), 2);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if(GetDistanceTo(_corners[_numCorner]) < 1f)
        {
            _numCorner++;
        }
        RotateTo(_corners[_numCorner]);
        Move();
    }
}
