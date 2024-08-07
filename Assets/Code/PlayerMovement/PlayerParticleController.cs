using UnityEngine;

namespace Code.PlayerMovement
{
    public class PlayerParticleController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;

        private ParticleSystem.EmissionModule emission;
        
        private void Awake()
        {
            emission = _particle.emission;
            emission.enabled = false;
        }

        public void EnableParticle(Vector2 velocity)
        {
            emission.enabled = velocity is not { x: 0, y: 0 };
        }
    }
}
