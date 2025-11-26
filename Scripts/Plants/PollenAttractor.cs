using UnityEngine;

public class PollenAttractor : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public float attractionSpeed = 10f;
    [SerializeField] private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    public bool attractionActive = false;


    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void LateUpdate()
    {
        if (target == null || !attractionActive || BeeInventory.Instance.ReachedMaxLoad()) return;

        int aliveCount = ps.GetParticles(particles);
        for (int i = 0; i < aliveCount; i++)
        {
            Vector3 direction = (target.position - particles[i].position).normalized;
            particles[i].position += direction * attractionSpeed * Time.deltaTime;
        }
        ps.SetParticles(particles, aliveCount);
    }
}

