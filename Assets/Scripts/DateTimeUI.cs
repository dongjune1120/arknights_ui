using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DateTimeUI : MonoBehaviour
{
	private TextMeshProUGUI textMesh;

	// Start is called before the first frame update
	void Start()
	{
		textMesh = GetComponent<TextMeshProUGUI>();

		StartCoroutine(DateTimeCheck());
	}

	public IEnumerator DateTimeCheck()
	{
		while (true)
		{
			textMesh.text = DateTime.Now.ToString("yyyy'/'MM'/'dd HH:mm");

			yield return new WaitForSeconds(1f);
		}
	}
}
