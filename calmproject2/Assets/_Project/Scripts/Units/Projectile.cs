using UnityEngine;

namespace SurvivalChaos
{
    public class Projectile : MonoBehaviour
    {
        [HideInInspector] public float  damage;
        [HideInInspector] public Entity target;

        [SerializeField] float speed        = 6f;
        [SerializeField] float maxLifetime  = 4f;
        [SerializeField] bool  rotateSprite = true;   // turn off if you’re using a particle or mesh that shouldn’t spin

        float _life;

        void Update()
        {
            // ─── target vanished? self-destruct ───
            if (target == null || target.CurrentHP <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            // ─── travel ───
            Vector3 dir = (target.transform.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;

            // ─── face the travel direction (XY plane) ───
            if (rotateSprite && dir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                // We want the sprite’s “up” to point at the target, so rotate around Z
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
                //        ^ adjust by -90° if your art points up; tweak if it points right
            }

            // ─── impact check ───
            if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
            {
                target.ReceiveDamage(damage);
                Destroy(gameObject);
            }

            // ─── safety cleanup ───
            _life += Time.deltaTime;
            if (_life > maxLifetime) Destroy(gameObject);
        }
    }
}
