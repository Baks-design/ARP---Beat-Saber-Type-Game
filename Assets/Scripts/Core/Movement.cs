using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Baks.Core.Managers;

namespace Baks.Core
{
    public class Movement : MonoBehaviour 
    {
        [SerializeField]
        [Range(0, 100)]
        private float m_speed = 10.0f;

        [SerializeField]
        [Range(0, 10)]
        private float m_destroyAfterDead = 1.0f;

        public float Speed { get => m_speed; set => m_speed = value; }
        private Vector3 _TargetPosition { get; set; } = Vector3.zero;

        private bool _avoided = false;
        private bool _targetWasPlanned = false;
        private ARFaceManager _faceManager;
        private ARFace _face;
            
        private void OnEnable() 
        {
            _faceManager = FindObjectOfType<ARFaceManager>();
            _faceManager.facesChanged += FacesChanged;
        }

        private void OnDisable() => _faceManager.facesChanged -= FacesChanged;

        private void Update() 
        {
            if (_TargetPosition != Vector3.zero)
            {
                float step = m_speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(this.transform.position, _TargetPosition, step);
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
            if (aRFacesChangedEventArgs.updated != null && 
                aRFacesChangedEventArgs.updated.Count > 0 && 
                !_targetWasPlanned)
            {
                _face = aRFacesChangedEventArgs.updated[0];

                var detectorPosition = _face.transform.Find("Detector").position;

                _TargetPosition = _face.transform.position + detectorPosition;
                _targetWasPlanned = true;
            }
        }
    }
}