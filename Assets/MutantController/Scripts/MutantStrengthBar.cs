using UnityEngine;
using UnityEngine.UI;

namespace Assets.MutantController.Scripts
{
    public class MutantStrengthBar : MonoBehaviour
    {
        public Image slider;
        public Mutant mutant;

        public void SetMaxMutationStrength(float maxStrength)
        {
            slider.fillAmount = maxStrength;
        }

        public void SetMutantStrength(float strength)
        {
            slider.fillAmount = strength;
        }

        public void Update()
        {
            slider.fillAmount = mutant.CurrentMutantStrength / mutant.maxMutantStrength;
        }
    }
}
