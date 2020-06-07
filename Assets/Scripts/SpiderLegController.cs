using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpiderLegController : MonoBehaviour
{
	public Transform m_TargetTransform;
	private Vector3 m_PreviewTargetPosition;
	public Transform m_FixedRayCastTransform;
	public Vector3 desirePosition;
	public LayerMask m_FloorLayerMask;
	public float m_StepDistance = 0.5f;
	public AnimationCurve m_TargetUpwardCure;
	public AnimationCurve m_TargetTowardCure;
	private float m_StepDuration = .3f;
	private float m_LastStepTimeStamp;
	private bool m_Moving = false;
	private void Start()
	{
		m_PreviewTargetPosition = m_TargetTransform.position;
		m_LastStepTimeStamp = Time.time;
	}
	private void Update()
	{
		if(!m_Moving)m_TargetTransform.position = m_PreviewTargetPosition;
		UpdateIkTarget();
		RaycastHit hitInfo;
		if (Physics.Raycast(m_FixedRayCastTransform.position,Vector3.down, out hitInfo, 2, m_FloorLayerMask))
		{
			desirePosition = hitInfo.point;
			if (Vector3.Distance(m_FixedRayCastTransform.position, m_TargetTransform.position) > m_StepDistance)
			{
				Vector3 position = desirePosition+new Vector3(0,0.5f,0);
				m_PreviewTargetPosition = position;
				m_LastStepTimeStamp = Time.time;
				m_Moving = true;
			}
		}
	}

	void UpdateIkTarget()
	{
		if (!m_Moving) return;
		float percent = Mathf.Clamp01((Time.time-m_LastStepTimeStamp)/m_StepDuration);
		Vector3 targetTransformPosition = m_TargetTransform.position;
		float offsetY = targetTransformPosition.y-m_PreviewTargetPosition.y;
		Vector3 directionYCurve = m_TargetUpwardCure.Evaluate(percent)*0.25f*Vector3.up-new Vector3(0,offsetY,0);
		Debug.Log("offset:"+directionYCurve);
		Debug.Log("distance:"+Vector3.Distance(m_PreviewTargetPosition,targetTransformPosition));
		m_TargetTransform.position = Vector3.Lerp(targetTransformPosition, m_PreviewTargetPosition,m_TargetTowardCure.Evaluate(percent))+directionYCurve;
		if (Mathf.Approximately(1,percent)) m_Moving = false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(m_TargetTransform.position,m_FixedRayCastTransform.position);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(desirePosition,0.03f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(m_FixedRayCastTransform.position,desirePosition);
	}
}
