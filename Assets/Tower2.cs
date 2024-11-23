using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower2 : MonoBehaviour
{
    // ��������·����Collider
    public BoxCollider2D electricPathCollider;  // ���ڵ���·����Collider
    public LayerMask layer;
    //�����������Χ
    private float AttackTimer = 0;
    [SerializeField]
    private float AttackCooldown;
    [SerializeField]
    private float AttackRange;
    //������
    [SerializeField]
    private float AttackPower;
    public float Damage
    {
        get { return AttackPower; }
    }
    
    void Start()
    {
        GiveAttackRange();
    }
    void Update()
    {

    }

    private void GiveAttackRange()
    {
        Vector2 range = new Vector2(AttackRange, AttackRange);
        TowerAttackRange towerRange = GetComponentInChildren<TowerAttackRange>();
        if (towerRange)
        {
            towerRange.SetAttackRange(range);
        }
    }

    public bool IsAlignedWith(Tower2 other)
    {
        // �ж����Ƿ���ͬһˮƽ�߻�ֱ����
        return (Mathf.Approximately(transform.position.x, other.transform.position.x) && Mathf.Abs(transform.position.y - other.transform.position.y) <= 8)
            || (Mathf.Approximately(transform.position.y, other.transform.position.y) && Mathf.Abs(transform.position.x - other.transform.position.x) <= 8);
    }

    public void SetElectricPathActive(bool isActive)
    {
        if (electricPathCollider != null)
        {
            Debug.Log($"Electric path collider enabled: {isActive}");
            electricPathCollider.enabled = isActive;
        }
    }

    public bool IsPathClear(Tower2 other)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = other.transform.position;

        // ʹ��Physics2D.Raycast�����֮���·��
        RaycastHit2D hit = Physics2D.Raycast(startPos + (endPos - startPos) * 0.5f, endPos - startPos, Vector3.Distance(startPos, endPos),layer);

        // ������߼�⵽�����岻��Ŀ������˵��·�������������赲
        if (hit.collider != null && hit.collider.gameObject != other.gameObject)
        {
            return false;  // ·�����ϰ���
        }

        return true;  // ·����ͨ
    }
    // ���µ���·����λ�á���С����ת
    public void UpdateElectricPath(Tower other)
    {
        if (electricPathObject == null) return;

        Vector3 startPos = transform.position;
        Vector3 endPos = other.transform.position;

        // ���µ���·����λ��
        electricPathObject.transform.position = (startPos + endPos) / 2;

        // �������·���ĳ���
        float distance = Vector3.Distance(startPos, endPos);
        electricPathCollider.size = new Vector2(distance, electricPathCollider.size.y);  // �޸�Collider�Ĵ�С��ƥ���³���

        // �������·���ĽǶ�
        float angle = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * Mathf.Rad2Deg;
        electricPathObject.transform.rotation = Quaternion.Euler(0, 0, angle);  // ����·������ת
    }
}
