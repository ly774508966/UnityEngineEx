UnityEngineEx
=============

Unity3D UnityEngine extension library.



Object creation
---------------

If prefab GameObject `pref` is loaded from resources or set up in editor it can be instantiated by New exteension method.

	GameObject prefab = Resources.Load("prefabs/ObjectPrefab");
	GameObject instance = prefab.New(
		new Action<Transform>((Transform t) => {
			t.localPosition = new Vector3(10, 20, 0);
		}),
		new Action<MeshRenderer>((MeshRenderer r) => {
			r.sharedMaterial = Resources.Load("materials/CustomSkin");
		}),
		new Action<MyComponent>((MyComponent mc) => {
			mc.somedata = new SomeData();
		})
		);

So object will be instantiated and all Actions would be called as component constructors before `Awake()` and `Start()` of any objects Component
is called.



Object extension
----------------

Any object can be dynamicaly extended by a Component by calling new AddComponent method which accepts constructor o template object

	instance.AddComponent<MyOtherComponent>((MyOtherComponent moc) => {
		moc.Initialize();
	});

	instance.AddComponent<MyOtherComponent>(new {
		somePrivateSerializeFiled = "Hi there"
	});



Hierarchy calls
---------------

Extension method `CallRecursive` recursively calls provided `Action` on GameObject and all it's child objects.

	instance.CallRecursive((GameObject o) => {
		var r = o.GetComponent<Renderer>();
		if (r != null) {
			r.SetSoringLayer("Gui", -10 + r.sortingOrder);
		}
	});

	instance.CallRecursive((GameObject o, int depth) => {
		var r = o.GetComponent<Renderer>();
		if (r != null) {
			r.SetSoringLayer("Gui", -10 + depth);
		}
	});



Declarative scene linkage
-------------------------

All GameObjects or their components can be dynamically linked to code objects by decalring member fields with `LinkToScene` attribute.


	public class Description : MonoBehaviour
	{
		[LinkToScene("Icon")] SpriteRenderer icon;
		[LinkToScene("Description")] TextMesh description;
		
		void Awake()
		{
			this.LinkSceneNodes(); // this call does actual linkage

			// so
			// "Icon" child node will be found and icon set to it's PriteRenderer component if it exists.
			// equivalent to code 
			// var o = gameObject.Find("Icon");
			// icon = o != null ? o.GetComponent<SpriteRenderer>();
		}

		void Start()
		{
			description.text = "Item description";
		}
	}

list of objects can be linked also. `LinkSceneNodes` handles list creation by itself.


	public class Page : MonoBehaviour
	{
		[LinkToScene("ItemDescriptions")] IList<Description> descriptions;
		
		void Awake()
		{
			this.LinkSceneNodes(); // this call does actual linkage

			// equivalent to code
			// descriptions = new List<Description>();
			// foreach (var child in gameObject.Find("ItemDescriptions")) {
			// 	descriptions.Add(child.GetComponent<Description>());
			// }
		}

		void Start()
		{
			foreach (var description in descriptions) {
				description.UpdateDescription();
			}
		}
	}

just not to make many simple MonoBehaviurs for trivial tasks it is possible to link GameObjects to standalone classes

	public class Page : MonoBehaviour
	{
		// It's ok to make this structire public, private, or internal
		struct Portrait
		{
			[LinkToScene("Icon")] SpriteRenderer icon;			
		}

		// WARN: this struct must have public (or internal) visibility or Mono will fail to link it in WebPlayer builds.
		// beacuse it is used in IList<> linkage.
		public struct Description
		{
			[LinkToScene] GameObject gameObject; // LinkToScene without "name" will link to object itself.
			[LinkToScene("Icon")] SpriteRenderer icon;
			[LinkToScene("Description")] TextMesh description;
		}

		[LinkToScene("ItemDescriptions")] IList<Description> descriptions;
		[LinkToScene("ItemPortrait")] Portrait portrait;
		
		void Awake()
		{
			this.LinkSceneNodes(); // this call does actual linkage
		}

		void Start()
		{
			foreach (var description in descriptions) {
				description.UpdateDescription();
			}
		}
	}

also any GameObject can be linked to any class or structure

	public struct Description
	{
		[LinkToScene] GameObject gameObject; // LinkToScene without "name" will link to object itself.
		[LinkToScene("Icon")] SpriteRenderer icon;
		[LinkToScene("Description")] TextMesh description;
	}

	var description = new Description();
	instance.LinkToSceneNode(description);
	// or one in line
	var description = instance.LinkToSceneNode(new Description());


Also many other extensions
--------------------------

Vector extensions in VectorEx. GameObject enumerators. Dictionary data loaders. Some Unity API "fixes". And Editor extensions like "Svae All",
resource path to clipboard, object path to clipboard and other stuff.

