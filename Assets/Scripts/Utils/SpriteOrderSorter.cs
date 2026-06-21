using UnityEngine;

namespace Utils
{
    public class SpriteOrderSorter : MonoBehaviour
    {
        public Transform point;
        public bool isMoving;
        private SpriteRenderer _sprite;

        [SerializeField] private int staticOffset = 0;

        private void Start()
        {
            _sprite = GetComponent<SpriteRenderer>();

            if (!point)
                point = transform;

            Sort();

            if (!isMoving)
                Destroy(this);
        }

        private void Update()
        {
            if (isMoving)
                Sort();
        }

        private void Sort()
        {
            _sprite.sortingOrder = Mathf.RoundToInt(point.position.y * 100f) * -1;
            _sprite.sortingOrder += staticOffset;
        }
    }
}
