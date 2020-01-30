using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowersSystem : MonoBehaviour
{
    public int buildPrice = 250;
    public float range = 5f;
    public float shootInterval = 1f;
    public float damage = 10f;
    public float speedRotation = 3f;
    private float _nextTimeToShoot = 0;
    public AudioClip audioClipShoot;
    private AudioSource _audioSource;
    private bool _isRotatedToTarget = false;

    public Transform transformRotateElem;
    private Vector3 _castlePos;

    void Start()
    {
        _castlePos = GameObject.Find("Castle").transform.position;
        _audioSource = GetComponent<AudioSource>();
    }

    private List<EnemySystem> targetsInRange = new List<EnemySystem>();

    public void DeleteTargetByInstaceId(int instaceId)
    {
        for (int i = 0; i < targetsInRange.Count; i++)
        {
            if (targetsInRange[i].GetInstanceID() == instaceId)
            {
                DeleteTargetById(i);
                //Destroy(targetsInRange[i]);
                return;
            }
        }
    }
    private void DeleteTargetById(int id)
    {
        //Debug.Log(id + " " + targetsInRange.Count);
        targetsInRange.RemoveAt(id);
    }
    
    public void AddTargetToSystem(GameObject targetGO)
    {
        targetsInRange.Add(targetGO.GetComponent<EnemySystem>());
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("enter" + other.name);
        AddTargetToSystem(other.gameObject);
    }
    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("exit" + other.name);
        DeleteTargetByInstaceId(other.gameObject.GetInstanceID());
    }

    void RotateTo(Vector3 target)
    {
        target.y = transform.position.y;
        var targetDir = Quaternion.LookRotation(target - transform.position);
        float angle = Vector3.Angle(target - transformRotateElem.position, transformRotateElem.forward);
        //Debug.Log(angle);
        if(angle < 20)
        {
            _isRotatedToTarget = true;
        }
        else
        {
            _isRotatedToTarget = false;
        }
        transformRotateElem.rotation = Quaternion.Slerp(transformRotateElem.rotation, targetDir, speedRotation * Time.deltaTime);
    }
    void Shoot(EnemySystem target)
    {
        if (Time.time >= _nextTimeToShoot && _isRotatedToTarget)
        {
            _nextTimeToShoot = Time.time + shootInterval;
            _audioSource.PlayOneShot(audioClipShoot);
            target.DamageMe(this, damage);
        }
    }
    EnemySystem GetTarget()
    {
        if (targetsInRange.Count == 0)
            return null;

        float[] distanceEnemyToCastle = new float[targetsInRange.Count];
        float minDistance = 999999999f;
        float distance;
        int t = 0;
        for (int i = 0; i < targetsInRange.Count; i++)
        {
            if( targetsInRange[i] == null )
            {
                DeleteTargetById(i);
                return null;
            }
            distance = Vector3.Distance(_castlePos, targetsInRange[i].transform.position);
            if (distance < minDistance)
            {
                t = i;
                minDistance = distance;
            }
        }

        return targetsInRange[t];
    }
    void Update()
    {
        EnemySystem target = GetTarget();
        if(target != null)
        {
            RotateTo(target.transform.position);
            Shoot(target);
        }
    }
}
