using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Cannon : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform muzzle;
    public Transform playerPos;
    public CannonBullet bullet;
    private bool canShoot;
    public Color color;
    public Transform canonPos;
    [FormerlySerializedAs("radiusPlayer")] public float playerRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (canShoot)
        {
            StartCoroutine(BulletSpawnner());
        }

        if (Vector3.Distance(transform.position, playerPos.position) < playerRadius)
        {
            Vector3 direction = playerPos.position - transform.position;


            transform.right = direction;
        }

    }


    IEnumerator BulletSpawnner()
    {
        while (Vector3.Distance(transform.position, playerPos.transform.position) < playerRadius)
        {
            canShoot = false;
            var clone = Instantiate(bulletPrefab, muzzle.transform.position, muzzle.rotation);
            clone.GetComponent<CannonBullet>().Shoot(muzzle.transform.right);

            Destroy(clone, 5f);

            yield return new WaitForSeconds(2f);

        }

        canShoot = true;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, playerRadius);

    }
}
