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
	private Vector3 temp = new Vector3();

	// Start is called before the first frame update
	private void Start()
	{
		originalPosition = transform.localPosition;
		prevAcceleration = Input.acceleration;
	}

	// Update is called once per frame
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			temp.x -= 0.1f;
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			temp.x += 0.1f;
		}
#if UNITY_ANDROID// && !UNITY_EDITOR
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
		var acceleration = Input.acceleration - prevAcceleration;
		if (acceleration.sqrMagnitude > 1) acceleration.Normalize();
		transform.localPosition += (oppsiteMove ? acceleration : -acceleration) * speed * 1000;
		
		

		if (acceleration == Vector3.zero && !isIdle)
		{
			lerpTime += Time.deltaTime;

			if (lerpTime > 1f)
			{
				isIdle = true;
				lerpTime = 0f;
				Debug.Log("TRUE");
			}
			else
			{
				isIdle = false;
			}
		}
		
		prevAcceleration = Input.acceleration;

		//isIdle = Mathf.Abs(acceleration.x) < 0.01f ? true : false;

		

		//var period = 0f;
		//var accelResult = Vector3.zero;
		//for (var i = 0; i < Input.accelerationEventCount; i++)
		//{
		//	AccelerationEvent accelerationEvent = Input.GetAccelerationEvent(i);
		//	accelResult += accelerationEvent.acceleration * accelerationEvent.deltaTime;
		//	period += accelerationEvent.deltaTime;
		//	Debug.Log("i: " + i +  ", AccelEvent: " + accelResult + ", Aceeleration: " + accelerationEvent.acceleration + ", DeltaTime: " + accelerationEvent.deltaTime);
		//}
		//if (period > 0)
		//	accelResult *= 1.0f / period;

		//Debug.Log("AccelResult: " + accelResult);
		//Debug.Log("InputAccel : " + Input.acceleration);
	}

	public void EditorTestAccelerate()
	{
		var mousePosition = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		transform.localPosition += (oppsiteMove ? mousePosition : -mousePosition) * speed;

		isIdle = mousePosition == Vector3.zero ? true : false;
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
