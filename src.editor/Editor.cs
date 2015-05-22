namespace UnityEditorEx
{
	public class Editor<T> : UnityEditor.Editor
		where T : UnityEngine.Object
	{
		new public T target { get { return (T)base.target; } }
	}
}