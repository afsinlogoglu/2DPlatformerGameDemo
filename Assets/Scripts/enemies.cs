using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class enemies : MonoBehaviour
{
    GameObject[] movingPoints;
    bool takePositionDiff = true;
    bool movingDirection = true;
    Vector3 positionDiff;
    int positionDiffCounter=0;
    void Start()
    {
        movingPoints = new GameObject[transform.childCount];
        for (int i = 0; i < movingPoints.Length; i++)
        {
            movingPoints[i] = transform.GetChild(0).gameObject;
            movingPoints[i].transform.SetParent(transform.parent);
        }
    }

    void FixedUpdate()
    {
        enemyMoving();
    }

    void enemyMoving()
    {
        if (takePositionDiff)

        {
            positionDiff = (movingPoints[positionDiffCounter].transform.position - transform.position).normalized;
            takePositionDiff = false;
        }
        float distance = Vector3.Distance(transform.position, movingPoints[positionDiffCounter].transform.position);
        transform.position += positionDiff * Time.deltaTime * 4;
        if (distance<0.5f)
        {
            takePositionDiff = true;
            if (positionDiffCounter == movingPoints.Length-1)
            {
                movingDirection = false;
            }
            else if (positionDiffCounter==0)
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
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }
        for (int i = 0; i  < transform.childCount-1; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(enemies))]
[System.Serializable]
class enemiesEditor :Editor
{
    public override void OnInspectorGUI()
    {
        enemies script = (enemies)target;
        if (GUILayout.Button("Generate"))
        {
            GameObject newObject = new GameObject();
            newObject.transform.parent = script.transform;   //gameobject enemies parentinin childi olarak olusturuluyor
            newObject.transform.position = script.transform.position;
            newObject.name = script.transform.childCount.ToString();


        }
    }

}
#endif