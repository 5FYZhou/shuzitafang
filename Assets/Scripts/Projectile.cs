using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Monster target;

    private Tower1 parent;

    void Update()
    {
        MoveToTarget();
    }

    public void Initialize(Tower1 parent)
    {
        this.target = parent.Target;
        this.parent = parent;
    }

    private void MoveToTarget()
    {
        if (target != null/*&&target.IsActive�������*/)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);
            /*//�ӵ�����
            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            */

        }
        /*else if (!target.isActive)
        {
            GameObject.Instance.Pool.ReleaseObject(gameObject);
        }�����뿪��ͼ������ʱ�ӵ���ʧ*/
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (target.gameObject == other.gameObject)
            {
                target.attack(parent.Damage);//�Թ�������˺�
                Destroy(gameObject);
            }
        }
    }
}
