using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveControl : MonoBehaviour
{
    public int wave = 1;
    public int spawnMultiplier; // Quantos vezes o uso dos spawns aumentará a cada round
    public GameObject enemy, target;

    public float spawnInterval = 1f; // Intervalo de tempo entre o spawn de cada inimigo
    [Range(0.001f, 0.999f)]
    public float spawnIntervalDecreaseRate = .075f; // A taxa com a qual o spawnInterval vai diminuindo ao passar das waves
    private float nextTimeToSpawn;
    public float nextWaveInterval = 2f; //Intervalo entre o fim de uma wave e o início de outra
    private bool isStartingWave = false;
    private float remainingEnemiesToSpawn = 0f;
    private bool waveSpawning = false; // Para testar se há uma wave sendo spawnada no momento

    private SpawnLocation[] spawns;
    public List<EnemyController> enemies;

    public Text waveIndicator;
    
	void Start ()
    {
        spawns = GetComponentsInChildren<SpawnLocation>();
        enemies = new List<EnemyController>();
    }
	void Update ()
    {
        waveIndicator.text = "Round\n" + (wave - 1);
        if (enemies.Count == 0 && !isStartingWave && !waveSpawning)
        {
            Invoke("StartWave", nextWaveInterval);
            isStartingWave = true;
        }

        if (Time.time > nextTimeToSpawn && remainingEnemiesToSpawn > 0)
        {
            SpawnUnit();
        }
    }

    
    private void StartWave()
    {
        waveSpawning = true;
        isStartingWave = false;
        int qtdSpawns = spawnMultiplier * wave;
        remainingEnemiesToSpawn = qtdSpawns;
    }

    //Spawn an enemy
    private void SpawnUnit()
    {
        if (remainingEnemiesToSpawn == spawnMultiplier * wave) // Caso seja o primeiro inimigo a ser spawnado, uma nova wave estará sendo iniciada
        {
            wave++; //Adicionando a nova wave
        }

        SpawnLocation possibleSpawn = spawns[(int)Random.Range(0, spawns.Length)];
        if (possibleSpawn.TestSpawn())
        {
            GameObject newEnemy = Instantiate(enemy, possibleSpawn.transform.position, possibleSpawn.transform.rotation);
            enemies.Add(newEnemy.GetComponent<EnemyController>());
            newEnemy.GetComponentInChildren<EnemyController>().target = target;
            newEnemy.GetComponentInChildren<EnemyController>().waveControl = this;
            remainingEnemiesToSpawn--;
            nextTimeToSpawn = Time.time + spawnInterval;
        }

        if(remainingEnemiesToSpawn == 0) // Caso seja a ultima wave
        {
            spawnInterval *= 1f - spawnIntervalDecreaseRate; // Diminuindo o intervalo de spawn para a proxima wave
            waveSpawning = false; // A wave acabou, então não está mais spawnando
        }
    }
}
