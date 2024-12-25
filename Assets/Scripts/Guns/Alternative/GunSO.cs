using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public enum GunType
{
    TypeA,
    TypeB,
    TypeC,
}

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunSO : ScriptableObject
{
    public GunType gunType;
    public string name;
    public GameObject modelPrefab;
    public Vector3 spawnPoint;
    public Vector3 spawnRotation;

    public DamageConfigurationSO damageConfig;
    public ShootConfigurationSO shootConfig;
    public TrailConfigurationSO trailConfig;
    public AudioConfigurationSO audioConfig;

    private MonoBehaviour activeMono;
    private GameObject instanciatedModel;
    private AudioSource shootingAudioSource;

    private float LastShootTime;
    private ParticleSystem shootSystem;
    private ObjectPool<Bullet> bulletPool;
    private ObjectPool<TrailRenderer> trailPool;

    public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        this.activeMono = activeMonoBehaviour;
        LastShootTime = 0;
        trailPool = new ObjectPool<TrailRenderer> (CreateTrail);
        if(!shootConfig.isHitscan)
        {
            bulletPool = new ObjectPool<Bullet>(CreateBullet);
        }

        instanciatedModel = Instantiate(modelPrefab,parent);
        instanciatedModel.transform.localPosition = spawnPoint;
        instanciatedModel.transform.localRotation = Quaternion.Euler(spawnRotation);

        shootSystem = instanciatedModel.GetComponentInChildren<ParticleSystem>();
        shootingAudioSource = instanciatedModel.GetComponent<AudioSource>();    
    }

    public void Shoot()
    {
        if (Time.time > shootConfig.fireRate + LastShootTime)
        {
            LastShootTime = Time.time;
            shootSystem.Play();
            audioConfig.PlayShootingClip(shootingAudioSource);
            Vector3 shootDirection = shootSystem.transform.forward + new Vector3(
                    Random.Range(-shootConfig.spread.x, shootConfig.spread.x),
                    Random.Range(-shootConfig.spread.y, shootConfig.spread.y),
                    Random.Range(-shootConfig.spread.z, shootConfig.spread.z)
                );
            shootDirection.Normalize();
            if (shootConfig.isHitscan)
            {
                DoHitscanShoot(shootDirection);
            }
            else
            {
                DoProjectileShoot(shootDirection);
            }
        }
    }

    private void DoProjectileShoot(Vector3 shootDirection)
    {
        Debug.Log($"Shoot direction: {shootDirection}");
        Bullet bullet = bulletPool.Get();
        bullet.gameObject.SetActive(true);
        bullet.OnCollision += HandleBulletCollision;
        bullet.transform.position = shootSystem.transform.position;
        bullet.Spawn(shootDirection * shootConfig.bulletSpawnForce);

        TrailRenderer trail = trailPool.Get();
        if (trail != null) 
        {
            trail.transform.SetParent(bullet.transform, false);
            trail.transform.localPosition = Vector3.zero;
            trail.emitting = true;
            trail.gameObject.SetActive(true);
        }
    }

    private void HandleBulletCollision(Bullet bullet, Collision col)
    {
        TrailRenderer trail = bullet.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            trail.transform.SetParent (null, true);
            activeMono.StartCoroutine(DelayedDisableTrail(trail));
        }
        bullet.gameObject.SetActive(false);
        bulletPool.Release(bullet);

        if (col != null)
        {
            //Buscando contact points, no siempre el primero en tocar el el index 0
            ContactPoint contactPoint = col.GetContact(0);
            HandleBulletImpact(
                Vector3.Distance(contactPoint.point, bullet.SpawnLocation),
                contactPoint.point,
                contactPoint.normal,
                contactPoint.otherCollider
            );
        }
    }
    private void HandleBulletImpact(float distanceTraveled, Vector3 hitLocation, Vector3 hitNormal, Collider hitCollider)
    {
        if (hitCollider.TryGetComponent(out IDamageable damageable))
        {
            damageable.ReceiveDamage(damageConfig.GetDamage(distanceTraveled));
        }
    }

    private void DoHitscanShoot(Vector3 shootDirection)
    {
        if (Physics.Raycast(
                        shootSystem.transform.position,
                        shootDirection,
                        out RaycastHit hit,
                        float.MaxValue,
                        shootConfig.hitMask
                    ))
        {
            activeMono.StartCoroutine(
                PlayTrail(
                    shootSystem.transform.position,
                    hit.point,
                    hit
                )
            );
        }
        else
        {
            activeMono.StartCoroutine(
                PlayTrail(
                    shootSystem.transform.position,
                    shootSystem.transform.position + (shootDirection * trailConfig.missDistance),
                    new RaycastHit()
                )
            );
        }
    }

    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        TrailRenderer instance = trailPool.Get();
        instance.gameObject.SetActive( true );
        instance.transform.position = startPoint;
        yield return null;
        instance.emitting = true;
        float distance  = Vector3.Distance( startPoint, endPoint );
        float remainingDistance = distance;
        while ( remainingDistance > 0 )
        {
            instance.transform.position = Vector3.Lerp(
                startPoint,
                endPoint,
                Mathf.Clamp01(1 - (remainingDistance/distance))
            );
            remainingDistance -= trailConfig.simulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = endPoint;
        if(hit.collider != null )
        {

            HandleBulletImpact(distance, endPoint, hit.normal, hit.collider);
            
        }

        



        yield return new WaitForSeconds(trailConfig.duration);
        yield return null;  
        instance.emitting = false;  
        instance.gameObject.SetActive ( false );
        trailPool.Release(instance);



    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = trailConfig.Color;
        trail.material = trailConfig.material;
        trail.widthCurve = trailConfig.widthCurve;
        trail.time = trailConfig.duration;

        trail.minVertexDistance = trailConfig.minVertexDistance;
        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;


        return trail;
    }
    private IEnumerator DelayedDisableTrail(TrailRenderer trail)
    {
        yield return new WaitForSeconds(trailConfig.duration);
        yield return null;
        trail.emitting = false;
        trail.gameObject.SetActive ( false );
        trailPool.Release(trail);
    }
    private Bullet CreateBullet()
    {
        return Instantiate(shootConfig.bulletPrefab);
    }

}
