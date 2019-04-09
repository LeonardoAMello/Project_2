using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnityStandardAssets.Characters.FirstPerson.FirstPersonController
{
    float HP = 100f;

    public void TakeDamage(float damage, string cause)
    {
        HP -= damage;

        if (HP < 0)
        {
            print(cause);
            Destroy(gameObject);
        }
    }
}
