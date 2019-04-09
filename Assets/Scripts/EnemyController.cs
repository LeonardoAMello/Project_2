using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;
    public float distanceTarget = 1f;
    public float distanceAttack = 1f;
    public float damage = 10f;
    public float attackRate = 1f;
    private float nextTimeToAttack = 0f;
    public float HP = 30f;
    public int reward = 30; //money gained to kill;
    public WaveControl waveControl;

    public AudioSource hitSound;

    public bool followTarget;

    void Start()
    {
        agent.stoppingDistance = distanceTarget;
    }

    void Update()
    {
        if (target != null)
        {
            if (followTarget)
                agent.SetDestination(target.transform.position);
            if (Vector3.Distance(transform.position, target.transform.position) < distanceAttack && Time.time >= nextTimeToAttack)
            {
                nextTimeToAttack = Time.time + 1f / attackRate;
                Attack(target.GetComponent<FirstPersonController>());
            }
        }
    }

    private void Attack(FirstPersonController target)
    {
        target.TakeDamage(damage, "An enemy killed you!");
        hitSound.Play();
    }

    public int TakeDamage(float damage)
    {
        int moneyToGain = 0;
        HP -= damage;

        if (HP < 0)
        {
            moneyToGain = reward;
            if (waveControl != null) // Se este inimigo foi spawnado pelo WaveControl, ele se excluirá da lista de inimigos ativos
                waveControl.enemies.Remove(this);
        }
        return moneyToGain;
    }
}
