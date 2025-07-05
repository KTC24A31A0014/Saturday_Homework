using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private Player player_ = null;

	private const float viewAngle_ = 40f;
	private const float maxTurnAnglePerFrame_ = 10f;
	private const float moveAmountPerFrame_ = 0.2f;

	private void Update()
	{
		if (player_ == null) return;

		Vector3 toPlayer = player_.transform.position - transform.position;
		toPlayer.y = 0;

		if (toPlayer == Vector3.zero) return;

		float angleToPlayer = Vector3.Angle(transform.forward, toPlayer.normalized);

		if (angleToPlayer <= viewAngle_ * 0.5f)
		{
			Quaternion targetRotation = Quaternion.LookRotation(toPlayer);
			targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

			Quaternion currentRotation = transform.rotation;
			currentRotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, 0);

			transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, maxTurnAnglePerFrame_);

			transform.position += transform.forward * moveAmountPerFrame_;

			SetTarget(true);
		}
		else
		{
			SetTarget(false); 
		}
	}

	public void SetTarget(bool enable)
	{
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
		if (meshRenderer != null && meshRenderer.materials.Length > 0)
		{
			meshRenderer.materials[0].color = enable ? Color.red : Color.white;
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (player_ == null) return;

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, player_.transform.position);

		float detectionDistance = 5f;
		Vector3 forward = transform.forward * detectionDistance;

		Quaternion leftRot = Quaternion.Euler(0, -viewAngle_ * 0.5f, 0);
		Quaternion rightRot = Quaternion.Euler(0, viewAngle_ * 0.5f, 0);

		Vector3 leftDir = leftRot * forward;
		Vector3 rightDir = rightRot * forward;

		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position, transform.position + leftDir);
		Gizmos.DrawLine(transform.position, transform.position + rightDir);
	}
}
