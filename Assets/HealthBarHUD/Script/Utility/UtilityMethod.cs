using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Hsinpa.Utility {
	public class UtilityMethod {


		/// <summary>
		///  Load single sprite from multiple mode
		/// </summary>
		/// <param name="spriteArray"></param>
		/// <param name="spriteName"></param>
		/// <returns></returns>
		public static Sprite LoadSpriteFromMulti(Sprite[] spriteArray, string spriteName) {
			foreach (Sprite s in spriteArray) {

				if (s.name == spriteName) return s;
			}
			return null;
		}

		/// <summary>
		/// Clear every child gameobject
		/// </summary>
		/// <param name="parent"></param>
		public static void ClearChildObject(Transform parent) {
			foreach (Transform t in parent) {
				GameObject.Destroy(t.gameObject);
			}
		}

		/// <summary>
		///  Insert gameobject to parent
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="prefab"></param>
		/// <returns></returns>
		public static T CreateObjectToParent<T>(Transform parent, GameObject prefab) where T : MonoBehaviour
		{
			GameObject item = GameObject.Instantiate(prefab);
			item.transform.SetParent(parent);
			item.transform.localScale = Vector3.one;
			item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 1);
			item.transform.localPosition = new Vector3(0, 0, 1);
			return item.GetComponent<T>();
		}

		public static GameObject FindObject(GameObject parent, string name) {
			Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
			foreach (Transform t in trs) {
				if (t.name == name) {
					return t.gameObject;
				}
			}
			return null;
		}


		/// <summary>
		/// Rolls the dice, only return 1 or 0.
		/// </summary>
		/// <returns>The dice.</returns>
		public static int RollDice() {
			return Mathf.RoundToInt(Random.Range(0, 1));
		}

		/// <summary>
		/// Possibilities the match.
		/// </summary>
		/// <returns><c>true</c>, if match was possibilityed, <c>false</c> otherwise.</returns>
		public static bool PercentageGame(float rate) {
			float testValue = Random.Range(0f, 1f);
			return (rate >= testValue) ? true : false;
		}

		public static T PercentageTurntable<T>(T[] p_group, float[] percent_array) {
			float percent = Random.Range(0f, 100f);
			float max = 100;

			for (int i = 0; i < percent_array.Length; i++) {
				float newMax = max - percent_array[i];
				if (max >= percent && newMax <= percent) return p_group[i];

				max = newMax;
			}
			return default(T);
		}

		public static T PercentageTurntable<T>(T[] p_group, int[] percent_array) {
			float[] convertFloat = System.Array.ConvertAll(percent_array, s => (float)s);
			return PercentageTurntable<T>(p_group, convertFloat);
		}

		public static Vector3 ScaleToWorldSize(Vector3 p_vector3, int target_value) {
			return new Vector3(target_value / p_vector3.x, target_value / p_vector3.y, target_value / p_vector3.z);
		}


		public static T SafeDestroy<T>(T obj) where T : Object {
			if (Application.isEditor)
				Object.DestroyImmediate(obj);
			else
				Object.Destroy(obj);

			return null;
		}

		public static T SafeDestroyGameObject<T>(T component) where T : Component
		{
			if (component != null)
				SafeDestroy(component.gameObject);
			return null;
		}

		public static T ParseEnum<T>(string value) {
			return (T)System.Enum.Parse(typeof(T), value, true);
		}

		public static Texture2D toTexture2D( RenderTexture rTex, Texture2D tex2D)
		{
			//Texture2D tex = new Texture2D(size, size, TextureFormat.RGB24, false);
			RenderTexture.active = rTex;
			tex2D.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
			tex2D.Apply();
			return tex2D;
		}

		public static Color[] ToColor(Texture2D tex)
		{
			Color[] col = tex.GetPixels();
			return tex.GetPixels();
		}

		public static T GetFromDict<T>(Dictionary<string, T> dict, string key, T default_value) {
			if (dict.TryGetValue(key, out T p_value)) {
				return p_value;
			}
	
			return default_value;
		}

		public static Dictionary<string, T> SaveFromDict<T>(Dictionary<string, T> dict, string key, T default_value)
		{
			if (dict.ContainsKey(key))
			{
				dict[key] = default_value;
			}
			else {

				dict.Add(key, default_value);
			}

			return dict;
		}

		public static async Task DoDelayWork(float p_delay, System.Action p_action)
		{
			await Task.Delay(System.TimeSpan.FromSeconds(p_delay));

			if (p_action != null)
				p_action();
		}
	}
}