using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
	[SerializeField] private Sprite[] batterySprites = new Sprite[4];
	private Image batteryImage;
	private float batteryLevel;

	// Start is called before the first frame update
	void Start()
	{
		batteryImage = GetComponent<Image>();

		StartCoroutine(BatteryCheck());
	}

	public IEnumerator BatteryCheck()
	{
		while (true)
		{
			batteryLevel = SystemInfo.batteryLevel;

			if (batteryLevel == -1)
			{
				break;
			}
			else if (batteryLevel > 0.85f)
			{
				batteryImage.sprite = batterySprites[3];
			}
			else if (batteryLevel >= 0.5f)
			{
				batteryImage.sprite = batterySprites[2];
			}
			else if (batteryLevel >= 0.15f)
			{
				batteryImage.sprite = batterySprites[1];
			}
			else if (batteryLevel >= 0)
			{
				batteryImage.sprite = batterySprites[0];
			}

			yield return new WaitForSeconds(1f);
		}
	}
}
