using UnityEngine;

public class GhostAttack : MonoBehaviour
{
    public Transform player;
    public GameObject darkBeamPrefab;

    public float attackDistance = 7f;
    public float beamSpeed = 12f;
    public float cooldown = 3f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < attackDistance && timer > cooldown)
        {
            Shoot();
            timer = 0;
        }
    }

    void Shoot()
    {
        Vector3 spawnPos = transform.position + transform.forward * 1.2f;

        GameObject beam = Instantiate(darkBeamPrefab, spawnPos, Quaternion.identity);

        Vector3 dir = (player.position - spawnPos).normalized;

        beam.GetComponent<Rigidbody>().linearVelocity = dir * beamSpeed;

        Destroy(beam, 3f);
    }
}