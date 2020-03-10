using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotation : MonoBehaviour
{
	[SerializeField] private bool fixedZ = true;
	private Vector3 mousePosition;
	private float rotationSpeed = 0.05f;
	private Vector3 point = new Vector3();

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
			z = Input.acceleration.x
		};
		Debug.Log(dir);
		if (dir.sqrMagnitude > 1)
		{
			dir.Normalize();
		}

		dir *= Time.deltaTime;

		transform.RotateAround(point, Vector3.up, dir.x);

		

#elif UNITY_EDITOR

		mousePosition = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		transform.localPosition = (-mousePosition) + transform.localPosition;

		if (fixedZ)
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, 0, 0));
#endif
	}
}
