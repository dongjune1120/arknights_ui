using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotation : MonoBehaviour
{
	public bool fixedZ = true;

	private Vector3 point = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
		var tempVector3 = transform.localPosition;
		tempVector3.y = tempVector3.z = 0;
		point = tempVector3;

		Debug.Log(point);
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
		if (Input.GetKey(KeyCode.A))
		{
			transform.RotateAround(point, Vector3.up, -Time.deltaTime * 20);
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.RotateAround(point, Vector3.up, Time.deltaTime * 20);
		}

		if (fixedZ)
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, 0, 0));
#endif
	}
}
