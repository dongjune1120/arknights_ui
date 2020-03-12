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
	private Vector3 mousePosition;
	private Coroutine comebackCoroutine;
	private bool isIdle;

	// Start is called before the first frame update
	private void Start()
	{
		originalPosition = transform.localPosition;
		prevAcceleration = Input.acceleration;
	}

	// Update is called once per frame
	private void Update()
	{
#if UNITY_ANDROID && UNITY_IOS

		Vector3 acceleration = Input.acceleration - prevAcceleration;
		Debug.Log(acceleration);
		if (acceleration.sqrMagnitude > 1)
		{
			acceleration.Normalize();
		}
		Debug.Log(acceleration);
		Vector3 accPosition = new Vector3 { x = acceleration.x };
		transform.localPosition -= accPosition * 10;

		isIdle = Mathf.Abs(acceleration.x) < Mathf.Epsilon ? true : false;

		prevAcceleration = Input.acceleration;
		Debug.Log(isIdle);
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
		mousePosition = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		transform.localPosition += (oppsiteMove ? mousePosition : -mousePosition) * speed;

		isIdle = mousePosition == Vector3.zero ? true : false;
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
