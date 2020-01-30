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

    public Transform transformRotateElem;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private List<GameObject> targetsInRange = new List<GameObject>();

    public void DeleteTarget(int instaceId)
    {
        //Debug.Log(instaceId + "-");
        for (int i = 0; i < targetsInRange.Count; i++)
        {
            if (targetsInRange[i].GetInstanceID() == instaceId)
            {
                Destroy(targetsInRange[i]);
                targetsInRange.RemoveAt(i);
                return;
            }
        }
    }
    public void AddTargetToSystem(GameObject targetGO)
    {
        targetsInRange.Add(targetGO);
    }


    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter" + other.name);
        AddTargetToSystem(other.gameObject);
    }
    public void OnTriggerExit(Collider other)
    {
        Debug.Log("exit" + other.name);
        DeleteTarget(other.gameObject.GetInstanceID());
    }

    void RotateTo(Vector3 target)
    {
        target.y = transform.position.y; //set targetPos y equal to mine, so I only look at my own plane
        var targetDir = Quaternion.LookRotation(target - transform.position);
        transformRotateElem.rotation = Quaternion.Slerp(transformRotateElem.rotation, targetDir, speedRotation * Time.deltaTime);
    }
    void Shoot()
    {
        if (Time.time >= _nextTimeToShoot)
        {
            _nextTimeToShoot = Time.time + shootInterval;
            Debug.Log("Shoot");
            _audioSource.PlayOneShot(audioClipShoot);
        }
    }

    void Update()
    {
        if(targetsInRange.Count > 0)
        {
            RotateTo(targetsInRange[0].transform.position);
            Shoot();
        }
    }
}
