using UnityEngine;

namespace Core
{
    public class DelayedAudio : MonoBehaviour
    {
        public float delay;

        private void Awake()
        {
            var source = GetComponent<AudioSource>();
            if (source != null)
                source.PlayDelayed(delay);
        }
    }
}
