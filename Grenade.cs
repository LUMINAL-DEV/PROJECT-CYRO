using UnityEngine;

public class Grenade : MonoBehaviour
{
   [Header("Explosion Prefab")]
   [SerializeField] private GameObject explosionEffectPrefab; // explosion prefab refrence
   [SerializeField] private Vector3 explosionParticleOffset = new Vector3(0, 1, 0);


   [Header("Explosion Settings")]
   [SerializeField] private float explosionDelay = 3f; // delay befor exposion
   [SerializeField] private float explosionForce = 700f; // force applied by explosion
   [SerializeField] private float explosionRadius = 5f; // radius of explosion


   [Header("Audio Effects")]
   private float countdown;
   private bool hasExploded = false;

    private void Start()
    {
        countdown = explosionDelay;
    }

    private void Update()
    {
        if (!hasExploded)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                Explode();
                hasExploded = true;
            }
        }
    }
    
    void Explode()
    {
        GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position + explosionParticleOffset, Quaternion.identity);
        Destroy(explosionEffect, 4f);

        // play sound

        NearbyForceApply();

        Destroy(gameObject);
    }

    void NearbyForceApply()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }

 
}
