UnityEngineEx
=============

Unity3D UnityEngine extension library.


Usage
-----

Checkout `git submodule` to your `Unity Project/Libraries` folder. Than make directory links of `Libraries\UnityEngineEx\Assets\Plugins\*` folders to your `Assets\Plugins\` folder.
And `Libraries\UnityEngineEx\Assets\Editor\*` folders to your `Assets\Editor\` folder if you want to use UnityEditor extensions.


Object creation
---------------

If prefab GameObject `pref` is loaded from resources or set up in editor it can be instantiated by New exteension method.

	GameObject prefab = Resources.Load("prefabs/ObjectPrefab");
	GameObject instance = prefab.New(
		_.a((Transform t) => {
			t.localPosition = new Vector3(10, 20, 0);
		}),
		_.a((MeshRenderer r, MyComponent mc) => {
			r.sharedMaterial = Resources.Load("materials/CustomSkin");
			mc.somedata = new SomeData();
		}));

So object will be instantiated and all Actions would be called as component constructors before `Awake()` and `Start()` of any objects Component
is called.

`_.a(...)` is a helper function from `SystemEx` namespace assisting in conversion of `lambda` functions to `ActionContainer`.

Multiple object initializers are supported by `Decompose` mechanism

Decompose
--------

Every object can be decomposed to it's components. Decomposition is the process of extracting objects `Components` and passing them as the parameters to decompose function.

For example if GameObject `obj` have MeshRenderer, MeshCollider and MyBehaviour components it can be decomposed by calling Decompose function
	
	GameObject obj = ...;
	obj.Decompose(_.a((MeshRenderer mr, MeshCollider mc, MyBehaviour mb) => {
		// do something...
	}));

If GameObject does not have requested Component `null` will be passed as a parameter. Multiple decompose functions can be applied to GameObject - they will be called in order of declaration.
This behaviour is used in `GameObject.New` funcion.

Another variant of decomposition is decomposition to an object. By calling `Decompose` with some tagged object as argument it can be decomposed to its `Components` storing each in objects member.
It was known as `scene linkage` mechanism.

	public class Description : MonoBehaviour
	{
		[Component("Icon")] SpriteRenderer icon;
		[Component("Description")] TextMesh description;
		
		void Awake()
		{
			this.Decompose(); // this decompose object to itself

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

list of objects can be decomposed also. `Decompose` handles list creation by itself.


	public class Page : MonoBehaviour
	{
		[Component("ItemDescriptions")] IList<Description> descriptions;
		
		void Awake()
		{
			this.Decompose(); // this cdecompose object to itself

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

just not to make many simple MonoBehaviurs for trivial tasks it is possible to decompose GameObjects to standalone classes

	public class Page : MonoBehaviour
	{
		// It's ok to make this class public, private, or internal
		// no struct are allowed here
		class Portrait
		{
			[Component("Icon")] SpriteRenderer icon;			
		}

		// WARN: this class must have public (or internal) visibility or Mono will fail to decompose it in WebPlayer builds.
		// beacuse it is used in IList<> component.
		public class Description
		{
			[Component] GameObject gameObject; // Component without "name" will link to object itself.
			[Component("Icon")] SpriteRenderer icon;
			[Component("Description")] TextMesh description;
		}

		[Component("ItemDescriptions")] IList<Description> descriptions;
		[Component("ItemPortrait")] Portrait portrait;
		
		void Awake()
		{
			this.Decompose(); // this decompose object to itself
		}

		void Start()
		{
			foreach (var description in descriptions) {
				description.UpdateDescription();
			}
		}
	}

also any GameObject can be decomposed to any class or structure

	public class Description
	{
		[Component] GameObject gameObject; // Component without "name" will link to object itself.
		[Component("Icon")] SpriteRenderer icon;
		[Component("Description")] TextMesh description;
	}

	var description = new Description();
	instance.Decompose(description);
	// or one in line
	var description = instance.Decompose(new Description());

and finally GameObject can be decomposed to a structure of its components with simplified syntax


	[Component]
	public class Description
	{
		GameObject gameObject;
		MeshRenderer renderer;
		MeshCollider collider;
	}

	var description = new Description();
	instance.Decompose(description);
	// or one in line
	var description = instance.Decompose(new Description());




Object extension
----------------

Any object can be dynamicaly extended by a Component by calling new AddComponent method which accepts constructor o template object

	instance.AddComponent<MyOtherComponent>((MyOtherComponent moc) => {
		moc.Initialize();
	});

	instance.AddComponent<MyOtherComponent>(new {
		somePrivateSerializeFiled = "Hi there"
	});

`ActionContainer` also can by used as `AddComponent` constructor

	instance.AddComponent<MyOtherComponent>(_.a((MyOtherComponent moc, MeshFilter mf) => {
		moc.Initialize(mf.sharedMesh);
	}));



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



Also many other extensions
--------------------------

Vector extensions in VectorEx. GameObject enumerators. Dictionary data loaders.


Unity editor extensions
-----------------------

Hotkeys:
* `Ctrl + Alt + S` - Save All - saves all scenes, prefabs, and other staff. Usefull to do before code commits. Maybe it is like `Save Prject` but with a hotkey.
* `Ctrl + Alt + N` - Create new child object in hierarchy.
* `Ctrl + Alt + C` - Copy selected objects hierarchy path to clipboard.

Menues:
* `Asset/Copy path to clipboard` - copy assets full path to clipboard.

Extensions:
* `Transform/Normalize Parent` - sets parent position to child's position resetting child's position to zero and shifting all other children accordingly.

