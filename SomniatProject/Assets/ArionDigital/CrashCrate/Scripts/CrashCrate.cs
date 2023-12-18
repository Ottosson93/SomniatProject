namespace ArionDigital
{
    using UnityEngine;
    using UnityEngine.AI;

    public class CrashCrate : MonoBehaviour
    {
        [Header("Whole Create")]
        public MeshRenderer wholeCrate;
        public BoxCollider boxCollider;
        [Header("Fractured Crate")]
        public GameObject fracturedCrate;
        [Header("Audio")]
        public AudioSource crashAudioClip;
        public NavMeshObstacle navMeshObstacle;
        public ItemDropSystem itemDropSystem;

        private void Start()
        {
            itemDropSystem = GetComponent<ItemDropSystem>();
        }

        private void OnTriggerEnter(Collider other)
        {
            wholeCrate.enabled = false;
            boxCollider.enabled = false;
            navMeshObstacle.enabled = false;
            fracturedCrate.SetActive(true);
            itemDropSystem.HandleBoxDrop(transform.position);
            crashAudioClip.Play();
            Destroy(gameObject.transform.parent.gameObject, 1.5f);
            //Can change to this if you want lucid objects to stay after destruction
            //Destroy(gameObject, 1.5f);
        }

        [ContextMenu("Test")]
        public void Test()
        {
            wholeCrate.enabled = false;
            boxCollider.enabled = false;
            fracturedCrate.SetActive(true);
        }
    }
}