using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Baks.Core.Managers;

namespace Baks.Core.Main
{
    public class Movement : MonoBehaviour 
    {
        [SerializeField]
        [Range(0, 10)]
        private float m_speed = 1.0f;

        [SerializeField]
        [Range(0, 10)]
        private float m_destroyAfterDead = 1.0f;

        public float Speed { get => m_speed; set => m_speed = value; }
        private Vector3 TargetPosition { get; set; } = Vector3.zero;

        private bool _avoided = false;
        private bool _targetWasPlanned = false;

        ARFaceManager _faceManager;
        ARFace _face;

        private void Awake() => _faceManager = FindObjectOfType<ARFaceManager>();
            
        private void OnEnable() => _faceManager.facesChanged += FacesChanged;

        private void OnDisable() => _faceManager.facesChanged -= FacesChanged;

        private void Update() 
        {
            if (TargetPosition != Vector3.zero)
            {
                float step = m_speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, TargetPosition, step);
            }
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (_avoided)
                return;
            
            if (other.CompareTag("Player"))
            {
                if (!_avoided)
                {
                    _avoided = true;
                    UIManager.Instance.DecrementPoints();
                    Destroy(gameObject, m_destroyAfterDead);
                }
            }

            if (other.CompareTag("ScoreWall"))
            {
                if (!_avoided)
                {
                    _avoided = true;
                    UIManager.Instance.IncrementPoints();
                    Destroy(gameObject, m_destroyAfterDead);
                }
            }
        }

        private void FacesChanged(ARFacesChangedEventArgs aRFacesChangedEventArgs)
        {
            if (aRFacesChangedEventArgs.updated != null && aRFacesChangedEventArgs.updated.Count > 0 && !_targetWasPlanned)
            {
                _face = aRFacesChangedEventArgs.updated[0];

                var detectorPosition = _face.transform.Find("Detector").position;

                TargetPosition = _face.transform.position + detectorPosition;
                _targetWasPlanned = true;
            }
        }
    }
}