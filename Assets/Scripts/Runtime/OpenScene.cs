using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dev.ComradeVanti.Wurfel
{

    public class OpenScene : MonoBehaviour
    {

        public void WithName(string sceneName) =>
            SceneManager.LoadScene(sceneName);

    }

}