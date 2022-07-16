using ComradeVanti.CSharpTools;
using UnityEngine;
using UnityEngine.UI;

namespace Dev.ComradeVanti.Wurfel.UI
{

    public class LaunchForceDisplay : MonoBehaviour
    {

        [SerializeField] private Slider slider;

        public void OnLaunchForceChanged(Opt<float> launchForce)
        {
            gameObject.SetActive(launchForce.IsSome());
            launchForce.Iter(it => slider.value = it);
        }

    }

}