using UnityEngine;
using Baks.Core.Managers;

namespace Baks.Core
{
    public class Spawner : MonoBehaviour 
    {
        [SerializeField]
        private GameObject m_prefab = default;

        [SerializeField]
        [Range(1, 10)]
        private float m_spawnInMinSeconds = 1.0f;

        [SerializeField]
        [Range(1, 10)]
        private float m_spawnInMaxSeconds = 1.0f;

        [SerializeField]
        [Range(0, 100)]
        private float m_movementSpeed = 10.0f;

        [SerializeField]
        [Range(0, 1)]
        private float m_gizmoSize = 0.1f;

        [SerializeField]
        [Range(0, 100)]
        private float m_destroyInSeconds = 20.0f;

        private float _generatedSeconds;
        private float _spawnTimer;

        private void Awake() => _spawnTimer = _generatedSeconds = Random.Range(m_spawnInMinSeconds, m_spawnInMaxSeconds);

        private void Start() 
        {
            var anchor = AnchorManager.Instance.ARAnchorManager.AddAnchor(new Pose(transform.position, transform.rotation));
            transform.parent = anchor.transform;    
        }

        private void Update() 
        {
            if (_spawnTimer >= _generatedSeconds)
            {
                var go = Instantiate(m_prefab, transform.position, m_prefab.transform.rotation);
                go.transform.parent = transform;

                var movement = go.GetComponent<Movement>();
                movement.Speed = m_movementSpeed;

                Destroy(go, m_destroyInSeconds);

                _spawnTimer = 0;
                _generatedSeconds = Random.Range(m_spawnInMinSeconds, m_spawnInMaxSeconds);
            }
            else
                _spawnTimer += Time.deltaTime * 1.0f;
        }

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, m_gizmoSize);
        }
    }
}