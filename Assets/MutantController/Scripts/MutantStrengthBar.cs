using UnityEngine;
using UnityEngine.UI;

namespace Assets.MutantController.Scripts
{
    public class MutantStrengthBar : MonoBehaviour
    {
        public Slider slider;

        public void SetMaxMutationStrength(float maxStrength)
        {
                slider.value = maxStrength;
        }

        public void SetMutantStrength(float strength)
        {
            slider.value = strength;
        }

    }
}
