using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyWizard : MonoBehaviour
{
    GameObject[] movingPoints;
    GameObject adventurer;
    bool takePositionDiff = true;
    bool movingDirection = true;
    Vector3 positionDiff;
    RaycastHit2D ray;
    public LayerMask layermask;
    public GameObject spell;
    int positionDiffCounter = 0;
    int speed = 4;
    float spellingTime = 0;
    void Start()
    {
        movingPoints = new GameObject[transform.childCount];
        adventurer = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < movingPoints.Length; i++)
        {
            movingPoints[i] = transform.GetChild(0).gameObject;
            movingPoints[i].transform.SetParent(transform.parent);
        }
    }

    void FixedUpdate()
    {
        AttackMode();
        if (ray.collider.tag == "Player")  //enemyWizard seeing our adventurer
        {
            speed = 9;
            useSpell();
        }
        else     //enemyWizard can't  see our adventurer
        {
            speed= 3;
        }
        enemyMoving();
       
    }

    void useSpell()
    {
        spellingTime += Time.deltaTime;
        if (spellingTime > Random.Range(0.2f, 1))
        {
            Instantiate(spell, transform.position, Quaternion.identity);
            spellingTime = 0;

        }
    }
    void AttackMode()
    {
        Vector3 rayDirection = adventurer.transform.position - transform.position;
        ray = Physics2D.Raycast(transform.position, rayDirection, 1000, layermask);
        Debug.DrawLine(transform.position, ray.point, Color.magenta);
    }

    void enemyMoving()
    {
        if (takePositionDiff)

        {
            positionDiff = (movingPoints[positionDiffCounter].transform.position - transform.position).normalized;
            takePositionDiff = false;
        }
        float distance = Vector3.Distance(transform.position, movingPoints[positionDiffCounter].transform.position);
        transform.position += positionDiff * Time.deltaTime * speed;
        if (distance < 0.5f)
        {
            takePositionDiff = true;
            if (positionDiffCounter == movingPoints.Length - 1)
            {
                movingDirection = false;
            }
            else if (positionDiffCounter == 0)
            {
                movingDirection = true;
            }
            if (movingDirection)
            {
                positionDiffCounter++;
            }
            else
            {
                positionDiffCounter--;
            }
        }
    }

    public Vector2 GetDirection()
    {
        return (adventurer.transform.position-transform.position).normalized;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }

    }



#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyWizard))]
[System.Serializable]
class enemyWizardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyWizard script = (EnemyWizard)target;
        if (GUILayout.Button("Generate"))
        {
            GameObject newObject = new GameObject();
            newObject.transform.parent = script.transform;   //gameobject enemies parentinin childi olarak olusturuluyor
            newObject.transform.position = script.transform.position;
            newObject.name = script.transform.childCount.ToString();

        }
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("layermask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("spell"));


        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();

    }

}
#endif

