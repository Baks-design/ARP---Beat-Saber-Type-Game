using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using Baks.Core.Utils;

namespace Baks.Core.Managers
{
   [RequireComponent(typeof(ARFace))]
   public class PlayerManager : Singleton<PlayerManager> 
   {
      [SerializeField]
      private ARKitBlendShapeLocation m_blendShapeToTrack = ARKitBlendShapeLocation.JawOpen;

      public float Coefficient { private set; get; }
      public bool FaceUpdated { get; set; } = true;
      public ARFace Face { get => _face; set => _face = value; }

      private Dictionary<ARKitBlendShapeLocation, float> _caheBlendShape;
      private ARKitFaceSubsystem _faceSubsystem;
      private ARFace _face;

      private void Awake() 
      {
         _face = GetComponent<ARFace>();

         _caheBlendShape = new Dictionary<ARKitBlendShapeLocation, float>();
         _caheBlendShape.Add(m_blendShapeToTrack, 0);
      }

      private void OnEnable() 
      {
         var faceManager = FindObjectOfType<ARFaceManager>();
         if (faceManager != null)
            _faceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;
         
         _face.updated += OnUpdated;
      }

      private void OnDisable() => _face.updated -= OnUpdated;
      
      private void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
      {
         UpdateFaceFeatures();
         FaceUpdated = true;
      }

      private void UpdateFaceFeatures()
      {
         using (var blendShapes = _faceSubsystem.GetBlendShapeCoefficients(Face.trackableId, Allocator.Temp))
         {
            foreach (var featuresCoefficient in blendShapes)
            {
               if (_caheBlendShape.TryGetValue(featuresCoefficient.blendShapeLocation, out float coefficient))
               {
                  Coefficient = coefficient;
                  _caheBlendShape[featuresCoefficient.blendShapeLocation] = featuresCoefficient.coefficient;
               }
            }
         }
      }
   }
}