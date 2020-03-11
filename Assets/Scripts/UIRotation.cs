using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotation : MonoBehaviour
{
	[SerializeField] private bool fixedZ = true;
	private Vector3 mousePosition;
	private float rotationSpeed = 5f;
	private Vector3 point = new Vector3();
	private Coroutine comebackCoroutine;
	private bool isIdle;

    // Start is called before the first frame update
    void Start()
    {
		var tempVector3 = transform.localPosition;
		tempVector3.y = tempVector3.z = 0;
		point = tempVector3;
		point = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID && UNITY_IOS
		Vector3 dir = new Vector3
		{
			x = -Input.acceleration.y,
			z = Input.acceleration.x,
			y = Input.acceleration.z
		};
		Debug.Log(dir);
		if (dir.sqrMagnitude > 1)
		{
			dir.Normalize();
		}

		dir *= Time.deltaTime;

		transform.RotateAround(point, Vector3.up, dir.z * rotationSpeed);

		isIdle = dir.z == 0 ? true : false;

#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
		mousePosition = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		transform.localPosition = (-mousePosition) + transform.localPosition;

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

		if (fixedZ)
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, 0, 0));
	}

	public IEnumerator Comeback()
	{
		while (transform.localPosition != Vector3.zero)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime);

			yield return null;
		}
		yield return null;
	}
}
