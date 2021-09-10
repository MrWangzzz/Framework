using UnityEngine;
namespace FrameWork
{
    /// <summary>
    /// 音频事件驱动器
    /// </summary>
    public class AudioEventDriver : MonoBehaviour
    {

        /// <summary>
        /// 委托定义
        /// </summary>
        /// <param name="Percentage">百分比</param>
        public delegate void OnUpdateEventHandler(float Percentage, float time);
        public delegate void OnCompleteEventHandler();
        /// <summary>
        ///  委托实例
        /// </summary>
        public OnUpdateEventHandler onUpdate = null;
        public OnCompleteEventHandler onComplete = null;
        private AudioSource audioSource = null;
        /// <summary>
        /// OnUpdate 驱动方法
        /// </summary>
        /// <param name="audioSource_"></param>
        /// <param name="onUpdateEventHandler"></param>
        public void OnUpdate(AudioSource audioSource_, AudioEventDriver.OnUpdateEventHandler onUpdateEventHandler)
        {
            onUpdate = onUpdateEventHandler;
            audioSource = audioSource_;
        }
        public void OnComplete(AudioSource audioSource_, AudioEventDriver.OnCompleteEventHandler onCompleteEventHandler)
        {
            audioSource = audioSource_;
            onComplete = onCompleteEventHandler;
        }
        private void Update()
        {
            //OnUpdate 驱动
            if (onUpdate != null && audioSource != null)
            {
                if (audioSource.isPlaying)
                {
                    onUpdate(audioSource.time / audioSource.clip.length, audioSource.time);
                }
            }
            //OnComplete 驱动
            if (audioSource.time == audioSource.clip.length)
            {
                onComplete();
            }
        }
    }
}