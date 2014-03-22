using UnityEngine;
using System.Collections;

public class GuiController : MonoBehaviour
{
	void Update()
	{
#if UNITY_STANDALONE
		if (Input.GetMouseButtonDown(0)) {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				var button = hit.transform.GetComponent<Button>();

				if (button != null)
					button.OnCommand();
			}
		}
#else
		if (Input.touchCount > 0) {
			var touch = Input.GetTouch(0);

			var ray = Camera.main.ScreenPointToRay(touch.position);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				var button = hit.transform.GetComponent<Button>();

				if (button != null && touch.phase == TouchPhase.Began)
					button.OnCommand();
			}
		}
#endif
	}
}
