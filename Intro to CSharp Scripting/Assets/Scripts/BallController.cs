using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private int damage = 5;
    [SerializeField] private float lifetime = 3;
    [SerializeField] private string tagToDamage;
    //[SerializeField] private LayerMask layersToDamage; //check by layer

    public void SetDirection(Vector2 dir)
    {
        dir = dir.normalized;
        GetComponent<Rigidbody2D>().velocity = dir * speed;
        Invoke(nameof(DestroySelf), lifetime);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Check by layer 
        //if ((layersToDamage.value & (1 << other.gameObject.layer)) > 0)

        if (other.transform.CompareTag(tagToDamage))
        {
            other.transform.GetComponent<HealthSystem>()?.Damage(damage);
        }

    }
}