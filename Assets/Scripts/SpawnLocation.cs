using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    // dependentDoor serve para verificar se há alguma porta bloqueando o Spawn
    public GameObject doorLocking;
    public bool isSpawnable = false;


    void Update ()
    {
        // Caso não tenha nenhuma porta associada, o Spawn poderá ser usado
        TestSpawn();
	}

    public bool TestSpawn()
    {
        if (doorLocking == null)
            isSpawnable = true;

        return isSpawnable;
    }
}
