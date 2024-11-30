using System.Collections.Generic;
using UnityEngine;

public class Tower2Manager : MonoBehaviour
{
    private List<Tower2> towers = new List<Tower2>();  // �洢���е���
    public List<ElectricPath> electricPaths = new List<ElectricPath>();
    public GameObject electricPathPrefab;  // ����·����Ԥ�Ƽ�

    //private void Start()
    //{
        // ��ʼ��ʱ������������
        //towers.AddRange(FindObjectsOfType<Tower2>());
        //Debug.Log(towers.Count);
    //}

    private void Update()
    {
        //if (Tower2CountChanged() || Tower2PositionChanged())
        //{
        CheckTower2CountChange();
        CheckElectricCurrentBetweenTowers();
        //}
    }

    private void CheckTower2CountChange()
    {
        Tower2[] newtowers = FindObjectsOfType<Tower2>();
        if (newtowers.Length != towers.Count)
        {
            towers.Clear();
            towers.AddRange(newtowers);
        }
    }

    /*private bool Tower2PositionChanged()
    {
        foreach(Tower2 tower2 in towers)
        {
            if (tower2.PositionChanged())
            {
                return true;
            }
        }
        return false;
    }*/

    // ����Ƿ�����������������
    private void CheckElectricCurrentBetweenTowers()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            for (int j = i + 1; j < towers.Count; j++)
            {
                Tower2 towerA = towers[i];
                Tower2 towerB = towers[j];
                if (towerA.CanAlignedWith(towerB) && !IsElectricityAlreadyExists(towerA,towerB))  // �������ͬһˮƽ��ֱ���ϣ�����·����ͨ +û�е���
                {
                    // ��������·��
                    //Debug.Log($"creat between {towerA} and {towerB}");
                    CreateElectricPathCollider(towerA, towerB);
                }
                //else
                //{
                    // ���õ���·��
                    //towerA.SetElectricPathActive(false);
                    //towerB.SetElectricPathActive(false);
                //}
            }
        }
    }

    // ��������·����Collider
    private void CreateElectricPathCollider(Tower2 towerA, Tower2 towerB)
    {

            // �Ƚ�������·��
            //towerA.SetElectricPathActive(false);
            //towerB.SetElectricPathActive(false);

            // �������·������ʼ�ͽ�����
            Vector3 positionA = towerA.transform.position;
            Vector3 positionB = towerB.transform.position;
//            Debug.Log($"Creating electric path between {towerA} and {towerB}");
            //ʵ��������
            GameObject path = Instantiate(electricPathPrefab, (positionA + positionB) / 2, Quaternion.identity);
            //����
            Vector2 scale = path.transform.localScale;
            scale.x = Mathf.Abs(positionA.x - positionB.x) + Mathf.Abs(positionA.y - positionB.y)-1f;
            path.transform.localScale = scale;
            // �������·���ĳ���
            float angle = Mathf.Atan2(positionB.y - positionA.y, positionB.x - positionA.x) * Mathf.Rad2Deg;
            path.transform.rotation = Quaternion.Euler(0, 0, angle);
            
            // ��������·����Collider
            //BoxCollider2D collider = path.GetComponent<BoxCollider2D>();

            path.GetComponent<ElectricPath>().SetTowers(towerA, towerB, this);

            electricPaths.Add(path.GetComponent<ElectricPath>());
            /*
            if (collider != null)
            {
                collider.size = new Vector2(Mathf.Abs(positionA.x - positionB.x), Mathf.Abs(positionA.y - positionB.y));
                collider.isTrigger = true;
            */
                // Ϊ�����õ���·��
                //towerA.electricPaths.Add(path);
                //towerB.electricPaths.Add(path);

                // �������·��
                //towerA.SetElectricPathActive(true);
                //towerB.SetElectricPathActive(true);

            //towerA.electricPathObject = path;
            //towerB.electricPathObject = path;
           // }
    }

    private bool IsElectricityAlreadyExists(Tower2 tower1, Tower2 tower2)
    {
        foreach (var electricity in electricPaths)
        {
            if ((electricity.towerA == tower1 && electricity.towerB == tower2) ||
                (electricity.towerA == tower2 && electricity.towerB == tower1))
            {
                return true;  // �Ѿ����ڵ�������
            }
        }
        return false;
    }
}
