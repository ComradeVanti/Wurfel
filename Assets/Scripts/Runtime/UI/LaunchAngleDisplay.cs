using ComradeVanti.CSharpTools;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel.UI
{

    public class LaunchAngleDisplay : MonoBehaviour
    {

        [SerializeField] private RectTransform rectTransform;


        public void OnLaunchAngleChanged(Opt<float> launchAngle)
        {
            gameObject.SetActive(launchAngle.IsSome());

            launchAngle.Iter(it =>
            {
                var rotation = new Vector3(0, 0, -it);
                rectTransform.localRotation = Quaternion.Euler(rotation);
            });
        }

    }

}