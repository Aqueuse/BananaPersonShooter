using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoaxGames
{
    public class AddonAdaptToSurfaceSlope : MonoBehaviour
    {
        [SerializeField, Tooltip("With this parameter you can choose the prefered algorithm. For most action triggered animations like for example sliding, COMPLEX_SURFACE_NORMAL should give better visual results.")]
        protected AlignmentAlgorithm m_alignmentAlgorithm = AlignmentAlgorithm.COMPLEX_SURFACE_NORMAL;
        [SerializeField, Tooltip("A list of all animator states (in the animator controller), that will force adapt the characters rotation to the surface slope when they are active. Layer == animator layer. Animator state == animator state name.")]
        protected List<FootIK.AnimatorStateEntry> m_forceAdaptRotationToSurfaceSlopeAnimStates = new List<FootIK.AnimatorStateEntry>();
        [SerializeField, Tooltip("The transition speed with which the adaption to the surface slope takes place.")]
        protected float m_adaptRotationToSurfaceSlopeTransitionSpeed = 10.0f;
        [SerializeField, Tooltip("The transition speed with which the positional correction offset regarding to surface slope takes place.")]
        protected float m_positionOffsetTransitionSpeed = 10.0f;
        [SerializeField, Tooltip("This offset is additionally applied to the calculated grounded position in regard to the surface slope")]
        protected float m_positionalHeightOffset = 0.0f;
        [SerializeField, Tooltip("If true surface rotation is calculated every frame. If false surface rotation is calculated only on every animation entry-event.")]
        protected bool m_updateTargetRotationContinously = true;
        [SerializeField, Tooltip("If true surface rotation is always applied. Default is false so that this addon can be triggered by the above defined animation states.")]
        protected bool m_forceAdaptRotationToSurfaceSlope = false; public bool forceAdaptRotationToSurfaceSlope { get { return m_forceAdaptRotationToSurfaceSlope; } set { m_forceAdaptRotationToSurfaceSlope = value; } }

        protected Quaternion m_surfaceDeltaRot = Quaternion.identity;
        protected List<KeyValuePair<int, int>> m_adaptRotationToSurfaceSlopeAnimStates = new List<KeyValuePair<int, int>>();

        protected Transform m_transform;
        protected Animator m_animator;
        protected FootIK m_footIK;

        protected bool m_prevValidationState = false;
        protected Quaternion m_targetRotation = Quaternion.identity;

        protected bool m_prevForceAdaptRotation = false;

        protected Vector3 m_currOffsetVec = Vector3.zero;

        public enum AlignmentAlgorithm
        {
            SIMPLE_SURFACE_NORMAL,
            COMPLEX_SURFACE_NORMAL
        }

        protected virtual void Awake()
        {
            m_transform = this.transform;
            m_animator = this.GetComponent<Animator>();
            m_footIK = this.GetComponent<FootIK>();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            foreach (var entry in m_forceAdaptRotationToSurfaceSlopeAnimStates)
            {
                m_adaptRotationToSurfaceSlopeAnimStates.Add(new KeyValuePair<int, int>(entry.layer, Animator.StringToHash(entry.animatorState)));
                m_footIK.tryAddToForceInvalidOnAnimatorState(entry);
            }
        }

        // Update is called once per frame
        protected virtual void OnAnimatorIK(int layerIdx)
        {
            if (m_footIK == null) return;
            if (layerIdx != m_footIK.animatorIkPassLayerIndex) return;

            GroundedResult groundedResult = m_footIK.getGroundedResult();
            if (groundedResult == null) return;

            Vector3 offsetVec = Vector3.zero;

            bool validationState = isValidForAdaptingRotationToSurfaceSlope();

            if (validationState != m_prevValidationState || m_updateTargetRotationContinously)
            {
                if (validationState && groundedResult.isGrounded)
                {
                    if (m_alignmentAlgorithm == AlignmentAlgorithm.SIMPLE_SURFACE_NORMAL)
                    {
                        m_targetRotation = Quaternion.FromToRotation(m_transform.up, groundedResult.groundedNormal);
                    }
                    else
                    {
                        Vector3 right = Vector3.Cross(groundedResult.groundedNormal, m_transform.up);
                        Vector3 up = Vector3.Cross(right, groundedResult.groundedNormal);
                        Vector3 projForwardVecOnLeft = Vector3.Project(m_transform.forward, right);
                        Vector3 projForwardVecOnDown = Vector3.Project(m_transform.forward, up);
                        Vector3 projForwardVec = projForwardVecOnLeft + projForwardVecOnDown;
                        m_targetRotation = Quaternion.FromToRotation(m_transform.forward, projForwardVec.normalized);
                    }

                    Vector3 distToGroundedPointVec = groundedResult.groundedPoint - m_transform.position;
                    offsetVec = Vector3.Project(distToGroundedPointVec, groundedResult.groundedNormal);
                    offsetVec += m_positionalHeightOffset * groundedResult.groundedNormal;
                }
                else
                {
                    m_targetRotation = Quaternion.identity;
                }

                m_prevValidationState = validationState;
            }

            m_surfaceDeltaRot = Quaternion.Slerp(m_surfaceDeltaRot, m_targetRotation, Time.deltaTime * m_adaptRotationToSurfaceSlopeTransitionSpeed);
            m_currOffsetVec = Vector3.Lerp(m_currOffsetVec, offsetVec, Time.deltaTime * m_positionOffsetTransitionSpeed);

            Vector3 distVec = m_animator.bodyPosition - m_transform.position;
            m_animator.bodyPosition = m_transform.position + m_surfaceDeltaRot * distVec + m_currOffsetVec;
            m_animator.bodyRotation = m_surfaceDeltaRot * m_animator.bodyRotation;
        }

        protected virtual bool isValidForAdaptingRotationToSurfaceSlope()
        {
            if (m_forceAdaptRotationToSurfaceSlope != m_prevForceAdaptRotation)
            {
                if (m_forceAdaptRotationToSurfaceSlope) m_footIK.setIsValidAndShouldCheckForGrounded(FootIK.ValidationType.FORCE_INVALID);
                else m_footIK.setIsValidAndShouldCheckForGrounded(FootIK.ValidationType.CHECK_IS_GROUNDED);

                m_prevForceAdaptRotation = m_forceAdaptRotationToSurfaceSlope;
            }

            if (m_forceAdaptRotationToSurfaceSlope) return true;

            foreach (var entry in m_adaptRotationToSurfaceSlopeAnimStates)
            {
                if (m_animator.layerCount > entry.Key)
                {
                    var nextAnimState = m_animator.GetNextAnimatorStateInfo(entry.Key);
                    if (nextAnimState.shortNameHash == entry.Value && nextAnimState.normalizedTime > 0) return true;

                    bool hasStartedTransitionToAnotherState = nextAnimState.shortNameHash != entry.Value && nextAnimState.normalizedTime > 0;
                    if (m_animator.GetCurrentAnimatorStateInfo(entry.Key).shortNameHash == entry.Value && hasStartedTransitionToAnotherState == false) return true;
                }
            }

            return false;
        }
    }
}