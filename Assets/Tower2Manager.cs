using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower2Manager : MonoBehaviour
{
    private List<Tower2> towers = new List<Tower2>();  // �洢���е���
    public GameObject electricPathPrefab;  // ����·����Ԥ�Ƽ�

    private void Start()
    {
        // ��ʼ��ʱ������������
        towers.AddRange(FindObjectsOfType<Tower2>());
        Debug.Log(towers.Count);
    }

    private void Update()
    {
        // ÿ֡�����֮��ĵ���
        CheckElectricCurrentBetweenTowers();
    }

    // ����Ƿ�����������������
    private void CheckElectricCurrentBetweenTowers()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            for (int j = i + 1; j < towers.Count; j++)
            {
                Tower2 towerA = towers[i];
                Tower2 towerB = towers[j];
                if (towerA.IsAlignedWith(towerB) && towerA.IsPathClear(towerB))  // �������ͬһˮƽ��ֱ���ϣ�����·����ͨ
                {
                    // ��������·��
                    Debug.Log("creat");
                    CreateElectricPathCollider(towerA, towerB);
                }
                else
                {
                    // ���õ���·��
                    towerA.SetElectricPathActive(false);
                    towerB.SetElectricPathActive(false);
                }
            }
        }
    }

    // ��������·����Collider
    private void CreateElectricPathCollider(Tower2 towerA, Tower2 towerB)
    {
        // ȷ��ֻ��һ������·������
        if (towerA.electricPathCollider == null || towerB.electricPathCollider == null)
        {
            // �Ƚ�������·��
            towerA.SetElectricPathActive(false);
            towerB.SetElectricPathActive(false);

            // �������·������ʼ�ͽ�����
            Vector3 positionA = towerA.transform.position;
            Vector3 positionB = towerB.transform.position;
            Debug.Log($"Creating electric path between {positionA} and {positionB}");
            // ��������·����Collider
            GameObject path = Instantiate(electricPathPrefab, (positionA + positionB) / 2, Quaternion.identity);
            BoxCollider2D collider = path.GetComponent<BoxCollider2D>();

            if (collider != null)
            {
                collider.size = new Vector2(Mathf.Abs(positionA.x - positionB.x), Mathf.Abs(positionA.y - positionB.y));
                collider.isTrigger = true;

                // �������·���ĳ���
                float angle = Mathf.Atan2(positionB.y - positionA.y, positionB.x - positionA.x) * Mathf.Rad2Deg;
                path.transform.rotation = Quaternion.Euler(0, 0, angle);

                // Ϊ�����õ���·��
                towerA.electricPathCollider = collider;
                towerB.electricPathCollider = collider;

                // �������·��
                towerA.SetElectricPathActive(true);
                towerB.SetElectricPathActive(true);
            }
        }
    }
}
