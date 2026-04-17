using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    // fields for dealing with damage
    [SerializeField] private int damage = 5;
    [SerializeField] private string tagToDamage;
    //[SerializeField] private LayerMask layersToDamage; //check by layer

    /*
        OTHER UNCHANGED CODE OMMITTED BUT LEAVE IT THERE!
    */

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Check by layer 
        //if ((layersToDamage.value & (1 << other.gameObject.layer)) > 0)

        if (other.transform.CompareTag(tagToDamage))
        {
            other.transform.GetComponent<HealthSystem>()?.Damage(damage);
            /*
             * ? is easier to type than
             * if (other.transform.GetComponent<HealthSystem>() != null) { ...
             */
        }
    }
}