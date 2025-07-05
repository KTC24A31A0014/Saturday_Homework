using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private Player player_ = null;

	[Header("検知設定")]
	[SerializeField] private float detectionRange_ = 5.0f;   // 検知距離
	[SerializeField, Range(0f, 180f)] private float viewAngle_ = 60.0f; // 視野角（度）

	[Header("移動設定")]
	[SerializeField] private float moveSpeed_ = 2.0f;

	private void Update()
	{
		if (player_ == null) return;

		Vector3 toPlayer = player_.transform.position - transform.position;
		float distance = toPlayer.magnitude;

		if (distance > detectionRange_) return;

		// プレイヤーが視野角内にいるかを確認
		Vector3 forward = transform.forward;
		Vector3 directionToPlayer = toPlayer.normalized;
		float angleToPlayer = Vector3.Angle(forward, directionToPlayer);

		if (angleToPlayer <= viewAngle_ * 0.5f) // 視野角の半分以内にいる
		{
			// プレイヤーに向かって移動
			directionToPlayer.y = 0;
			transform.position += directionToPlayer * moveSpeed_ * Time.deltaTime;

			// 向きもプレイヤーの方向へ
			if (directionToPlayer != Vector3.zero)
			{
				Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5.0f * Time.deltaTime);
			}
		}
	}

	public void SetTarget(bool enable)
	{
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.materials[0].color = enable ? Color.red : Color.white;
	}
}
