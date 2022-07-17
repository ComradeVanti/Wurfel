using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dev.ComradeVanti.Wurfel.UI
{

    public class DiceInventory : MonoBehaviour
    {

        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private UnityEvent<GameObject> onDiceSelected;

        private DiceSpawner spawner;


        private void Awake()
        {
            spawner = FindObjectOfType<DiceSpawner>();
            for (var i = 0; i < 2; i++)
                AddOneToInventory();
        }

        public void AddOneToInventory()
        {
            var diceGameObject = spawner.SpawnDice(transform.position);
            var diceFreezer = diceGameObject.GetComponent<MotionFreezer>();
            var diceTransform = diceGameObject.transform;
            
            var buttonGameObject = Instantiate(buttonPrefab, transform);

            var originalScale = diceGameObject.transform.localScale;
            diceTransform.localScale /= 2;
            diceTransform.position = buttonGameObject.transform.position;
            diceTransform.parent = buttonGameObject.transform;
            diceFreezer.IsFrozen = true;

            buttonGameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                diceTransform.parent = null;
                diceTransform.localScale = originalScale;
                Destroy(buttonGameObject);
                diceFreezer.IsFrozen = false;
                onDiceSelected.Invoke(diceGameObject);
            });
        }

    }

}