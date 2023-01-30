using UnityEngine;

	public class CachedMonoBehaviour : MonoBehaviour
	{
		internal Transform CachedTransform;
		internal GameObject CachedGameObject;

		protected virtual void Awake () 
		{
			CachedTransform = transform;
			CachedGameObject = gameObject;
		}
	}
