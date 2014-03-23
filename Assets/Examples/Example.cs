using UnityEngine;
using UnityEngineEx;
using System.Collections.Generic;

public class Example : MonoBehaviour
{
	[Component("Lamps")] IList<MeshRenderer> lamps;
	[Component("Button")] Button button;

	bool lampsAreOn = false;
	Color[] lampColors = new Color[] {
		Color.red,
		Color.green,
		Color.blue,
	};

	// Use this for initialization
	void Start()
	{
		this.Decompose();

		button.OnCommand = () => {
			if (!lampsAreOn) {
				for (int i = 0; i < lamps.Count; i++) {
					lamps[i].material.color = lampColors[MathfEx.Repeat(i, lamps.Count)];
				}

				lampsAreOn = true;
			}
			else {
				for (int i = 0; i < lamps.Count; i++) {
					lamps[i].material.color = Color.white;
				}

				lampsAreOn = false;
			}
		};
	}


	// Update is called once per frame
	void Update()
	{
	
	}
}
