using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotation : MonoBehaviour
{
	[SerializeField] private bool oppsiteMove = false;
	[SerializeField] private bool fixedX = false;
	[SerializeField] private bool fixedY = false;
	[SerializeField] private bool fixedZ = true;
	[SerializeField] private float speed = 1f;

	private Vector3 originalPosition;
	private Vector3 prevAcceleration;
	private Coroutine comebackCoroutine;
	private bool isIdle;

	private float lerpTime = 0f;

	private float accelerometerUpdateInterval = 1.0f / 9.0f;
	private float lowPassKernelWidthInSeconds = 1.0f;
	private float lowPassFilterFactor;
	private Vector3 lowPassValue;

	// Start is called before the first frame update
	private void Start()
	{
		originalPosition = transform.localPosition;
		prevAcceleration = lowPassValue = Input.acceleration;
		lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
	}

	// Update is called once per frame
	private void Update()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidAccelerate();
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
		EditorTestAccelerate();
#endif
		if (isIdle)
		{
			if (comebackCoroutine == null)
			{
				comebackCoroutine = StartCoroutine(Comeback());
			}
		}
		else
		{
			if (comebackCoroutine != null)
			{
				StopCoroutine(comebackCoroutine);
				comebackCoroutine = null;
			}
		}

		transform.localPosition = new Vector3
		{
			x = fixedX ? originalPosition.x : transform.localPosition.x,
			y = fixedY ? originalPosition.y : transform.localPosition.y,
			z = fixedZ ? originalPosition.z : transform.localPosition.z
		};
	}

	public void AndroidAccelerate()
	{
		var lowPassAcceleration = LowPassFilterAccelerometer();
		var acceleration = (lowPassAcceleration - prevAcceleration) * 100;
		transform.localPosition += (oppsiteMove ? acceleration : -acceleration) * speed * 5;

		if (acceleration.x < 0.1f && acceleration.y < 0.1f && acceleration.z < 0.1f && !isIdle)
		{
			lerpTime += Time.deltaTime;

			if (lerpTime > 1f)
			{
				isIdle = true;
				lerpTime = 0f;
			}
			else
			{
				isIdle = false;
			}
		}

		prevAcceleration = lowPassAcceleration;
	}

	public void EditorTestAccelerate()
	{
		var mousePosition = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		transform.localPosition += (oppsiteMove ? mousePosition : -mousePosition) * speed;

		isIdle = mousePosition == Vector3.zero ? true : false;
	}

	public Vector3 LowPassFilterAccelerometer()
	{
		return lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, lowPassFilterFactor);
	}

	public IEnumerator Comeback()
	{
		while (transform.localPosition != Vector3.zero)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime);

			yield return null;
		}
		yield return null;
	}
}
