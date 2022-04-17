using UnityEngine;

namespace Hsinpa.SRP
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/HealthBarSRP", order = 5)]

    public class HealthbarSRP : ScriptableObject
    {
        public Color[] barColor;
        public Color emptyColor;

        public int barCount => barColor.Length;
    }
}