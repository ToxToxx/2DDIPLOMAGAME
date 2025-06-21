using NUnit.Framework;
using UnityEngine;
using PlayerAttack;
using PlayerMovement;
using PlayerEvent;
using InGameInput;
using UnityEngine.UIElements;


namespace Tests.Runtime
{
    public class PlayerAttackControllerTests
    {
        private PlayerAttackController _controller;
        private PlayerMovementModel _movementModel;
        private FakePlayerEventBus _eventBus;
        private FakeInputService _inputService;
        private Transform _playerTransform;
        private PlayerAttackStats _attackStats;
        private LayerMask _enemyLayer;

        [SetUp]
        public void SetUp()
        {
            // Простая модель перемещения игрока
            _movementModel = new PlayerMovementModel { IsGrounded = true };

            // Фейковые реализации интерфейсов
            _eventBus = new FakePlayerEventBus();
            _inputService = new FakeInputService();

            // Создаём GameObject для игрока
            var playerObject = new GameObject("Player");
            _playerTransform = playerObject.transform;

            // Настройки атаки
            _attackStats = ScriptableObject.CreateInstance<PlayerAttackStats>();
            _attackStats.BoxSize = new Vector2(1, 1);
            _attackStats.BoxOffset = new Vector2(1, 0);
            _attackStats.AttackDuration = 0.5f;
            _attackStats.Damage = 2f;

            _enemyLayer = LayerMask.GetMask("Damageable");

            // Создание контроллера вручную
            _controller = new PlayerAttackController(
                _eventBus,
                _playerTransform,
                _movementModel,
                _attackStats,
                _enemyLayer,
                _inputService
            );
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_playerTransform.gameObject);
            Object.DestroyImmediate(_attackStats);
        }

        [Test]
        public void Attack_Starts_When_AttackPressed_And_Grounded()
        {
            _inputService.AttackPressed = true;

            _controller.Tick();

            Assert.IsTrue(_controller.IsAttacking);
            Assert.IsTrue(_eventBus.AttackRaised);
        }

        [Test]
        public void Attack_DoesNotStart_When_NotGrounded()
        {
            _movementModel.IsGrounded = false;
            _inputService.AttackPressed = true;

            _controller.Tick();

            Assert.IsFalse(_controller.IsAttacking);
            Assert.IsFalse(_eventBus.AttackRaised);
        }

        [Test]
        public void Attack_Damages_Enemy_In_Range()
        {
            // Создаём врага
            var enemyObject = new GameObject("Enemy");

            // 1. Устанавливаем слой вручную
            int testLayer = 8; // Убедись, что этот слой добавлен в Unity как "Damageable"
            enemyObject.layer = testLayer;
            _enemyLayer = 1 << testLayer; // Маска только для этого слоя

            // 2. Ставим его на нужную позицию
            enemyObject.transform.position = _playerTransform.position + Vector3.right * 1.0f;

            // 3. Добавляем физику
            enemyObject.AddComponent<BoxCollider2D>();       
            enemyObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            // 4. Добавляем DamageableMock
            var damageableMock = enemyObject.AddComponent<DamageableMock>();
            damageableMock.Health = 10f;

            // 5. Устанавливаем input
            _inputService.AttackPressed = true;

            // 6. Выполняем Tick
            _controller.Tick();

            // 7. Проверка
            Assert.AreEqual(8f, damageableMock.Health);

            Object.DestroyImmediate(enemyObject);
        }



        // Простые фейковые реализации для тестов:
        private class FakeInputService : IInputService
        {
            public bool AttackPressed { get; set; }

            public bool AttackWasPressed => AttackPressed;

            public Vector2 Movement => throw new System.NotImplementedException();

            public bool JumpWasPressed => throw new System.NotImplementedException();

            public bool JumpIsHeld => throw new System.NotImplementedException();

            public bool JumpWasReleased => throw new System.NotImplementedException();

            public bool RunIsHeld => throw new System.NotImplementedException();

            public bool DashWasPressed => throw new System.NotImplementedException();

            public bool InteractionWasPressed => throw new System.NotImplementedException();

            public bool PauseWasPressed => throw new System.NotImplementedException();
        }

        private class FakePlayerEventBus : PlayerEventBus
        {
            public bool AttackRaised { get; private set; }

            public override void RaiseAttack()
            {
                AttackRaised = true;
            }
        }

        private class DamageableMock : MonoBehaviour, IDamageable
        {
            public float Health;

            public void TakeDamage(float amount)
            {
                Health -= amount;
            }
        }
    }
}
