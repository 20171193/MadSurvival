using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Jc
{
    public class AnimalAttackTrigger : MonoBehaviour
    {
        [Header("Components")]
        [Space(2)]
        [SerializeField]
        private AttackAnimal owner;

        [SerializeField]
        private SphereCollider col;

        [Space(3)]
        [Header("Balancing")]
        [Space(2)]
        [SerializeField]
        private PlayerTrigger currentTarget;
        public PlayerTrigger CurrentTarget { get { return currentTarget; } }

        public UnityAction<GameObject> OnAttackTrigger;

        private void Awake()
        {
            col.radius = owner.AttackRange - 0.1f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Manager.Layer.playerLM.Contain(other.gameObject.layer)) return;

            currentTarget = other.gameObject.GetComponent<PlayerTrigger>();
            OnAttackTrigger?.Invoke(other.gameObject);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!Manager.Layer.playerLM.Contain(other.gameObject.layer)) return;

            currentTarget = null;
        }
    }
}
