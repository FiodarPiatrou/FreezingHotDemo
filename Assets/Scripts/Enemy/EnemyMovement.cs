using System.Collections;
using Enemy.Interfaces;
using Entities;
using UnityEngine;

namespace Enemy
{
    public class EnemyMovement : EntityMovement, IKnockbackable
    {
        [Header("Obstacle Avoidance")]
        [SerializeField] private float obstacleCheckDistance = 1f;
        [SerializeField] private LayerMask obstacleLayerMask;
        [SerializeField] private float angleSearchStep = 15f; // Шаг поиска угла
        [SerializeField] private float maxSearchAngle = 180f; // Максимальный угол поиска

        private Transform _playerTransform;
        private float _currentAngleOffset = 0f; // Текущее смещение угла
        private bool _isKnockedBack;

        protected override void AwakeInitialize()
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            _playerTransform = (playerObject != null) ? playerObject.transform : null;
            
        }

        bool IsPathBlocked(Vector2 dir)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                Rb.position,
                dir,
                obstacleCheckDistance,
                obstacleLayerMask
            );

            return hit.collider;
        }

        Vector2 FindWalkableDirection(Vector2 targetDir)
        {
            // Сначала проверяем текущее направление с учетом смещения
            Vector2 offsetDir = Quaternion.Euler(0, 0, _currentAngleOffset) * targetDir;
            if (!IsPathBlocked(offsetDir))
            {
                return offsetDir;
            }

            // Ищем свободный путь, чередуя лево-право
            for (float angle = angleSearchStep; angle <= maxSearchAngle; angle += angleSearchStep)
            {
                // Проверка слева
                Vector2 leftDir = Quaternion.Euler(0, 0, angle) * targetDir;
                if (!IsPathBlocked(leftDir))
                {
                    _currentAngleOffset = angle;
                    return leftDir;
                }

                // Проверка справа
                Vector2 rightDir = Quaternion.Euler(0, 0, -angle) * targetDir;
                if (!IsPathBlocked(rightDir))
                {
                    _currentAngleOffset = -angle;
                    return rightDir;
                }
            }

            // Если ничего не найдено, сбрасываем смещение и останавливаемся
            _currentAngleOffset = 0f;
            return Vector2.zero;
        }

        protected override void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(MoveInput.x);
            transform.localScale = scale;
        }

        protected override void MoveEntity()
        {
            if (!Rb || _isKnockedBack) return;

            if (!_playerTransform)
            {
                MoveInput = Vector2.zero;
                base.MoveEntity();
                return;
            }

            Vector2 directionToPlayer = (_playerTransform.position - transform.position).normalized;

            // Ищем проходимое направление
            Vector2 walkableDir = FindWalkableDirection(directionToPlayer);

            // Если нашли прямой путь к игроку, сбрасываем смещение
            if (!IsPathBlocked(directionToPlayer))
            {
                _currentAngleOffset = 0f;
                walkableDir = directionToPlayer;
            }

            MoveInput = walkableDir;
            base.MoveEntity();
        }

        private void OnDrawGizmosSelected()
        {
            if (!enabled) return;

            Vector2 origin = (Rb) ? Rb.position : (Vector2)transform.position;

            Vector2 dir;
            if (_playerTransform)
                dir = (_playerTransform.position - transform.position).normalized;
            else if (MoveInput != Vector2.zero)
                dir = MoveInput.normalized;
            else
                dir = transform.up;

            // Рисуем текущее направление движения
            Gizmos.color = Color.cyan;
            Vector2 currentDir = Quaternion.Euler(0, 0, _currentAngleOffset) * dir;
            DrawObstacleRay(origin, currentDir);

            // Рисуем направление к игроку
            if (_playerTransform)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(origin, origin + dir.normalized * obstacleCheckDistance * 0.8f);
            }

            // Рисуем зону проверки препятствий
            DrawObstacleRay(origin, dir);
        }

        private void DrawObstacleRay(Vector2 origin, Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, obstacleCheckDistance, obstacleLayerMask);
            Gizmos.color = (hit.collider) ? Color.red : Color.green;
            Gizmos.DrawLine(origin, origin + direction.normalized * obstacleCheckDistance);
            Gizmos.DrawSphere(origin + direction.normalized * obstacleCheckDistance, 0.05f);
        }

     
        public void ApplyKnockback(Vector2 force, float stunDuration)
        {
            _isKnockedBack = true;
            
            Rb.linearVelocity = Vector2.zero; 
            
            Rb.AddForce(force, ForceMode2D.Impulse);
            
            StopAllCoroutines(); 
            StartCoroutine(ResetKnockbackRoutine(stunDuration));
        }

        private IEnumerator ResetKnockbackRoutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            
            Rb.linearVelocity = Vector2.zero; 
        
            _isKnockedBack = false;
        }

       
    }
}
