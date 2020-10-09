using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellControl : MonoBehaviour
{
    // Start is called before the first frame update
    EnemyWizard EnemyWizard;
    private Rigidbody2D physics;

    public spellControl(EnemyWizard enemyWizard)
    {
        this.EnemyWizard = enemyWizard;
    }

    void Start()
    {
        EnemyWizard = GameObject.FindGameObjectWithTag("EnemyWizard").GetComponent<EnemyWizard>();
        physics = GetComponent<Rigidbody2D>();
        physics.AddForce(EnemyWizard.GetDirection() * 1000);
    }

    
}
