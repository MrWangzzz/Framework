using UnityEngine;
namespace FrameWork
{
    public static class AudioExtend
    {

        public static bool isOpenBg
        {
            get { return bool.Parse(PlayerPrefs.GetString("BgSource", "true")); }
            set { PlayerPrefs.SetString("BgSource", value.ToString()); }
        }

        public static bool isOpenSound
        {
            get { return bool.Parse(PlayerPrefs.GetString("soundSource", "true")); }
            set { PlayerPrefs.SetString("soundSource", value.ToString()); }
        }

        public static AudioSource GetAudio(this Transform transform, bool add = false)
        {
            return GetAudio(transform.gameObject, add);
        }

        /// <summary>
        /// 获得一个音频播放器
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static AudioSource GetAudio(this GameObject gameObject, bool isbgSource = false)
        {
            AudioSource audio = gameObject.GetOrAddComponent<AudioSource>();
            audio.mute = isbgSource ? !isOpenBg : !isOpenSound;
            return audio;
        }
        /// <summary>
        /// 设置Clip
        /// </summary>
        /// <param name="audioSource"></param>
        /// <returns></returns>
        public static AudioSource SetClip(this AudioSource audioSource, AudioClip clip)
        {
            if (clip) audioSource.clip = clip;
            return audioSource;
        }

        /// <summary>
        /// 设置Clip
        /// </summary>
        /// <param name="audioSource"></param>
        /// <returns></returns>
        public static AudioSource SetClip(this AudioSource audioSource, string clip)
        {
            SetClip(audioSource, Resources.Load<AudioClip>("Audio/" + clip));
            return audioSource;
        }

        /// <summary>
        /// 获取音频事件驱动
        /// </summary>
        /// <param name="audioSource"></param>
        /// <returns></returns>
        private static AudioEventDriver GetEventDriver(this AudioSource audioSource)
        {
            AudioEventDriver AudioEventDriver = audioSource.gameObject.GetComponent<AudioEventDriver>();
            if (AudioEventDriver == null)
            {
                AudioEventDriver = audioSource.gameObject.AddComponent<AudioEventDriver>();
            }
            return AudioEventDriver;
        }

        /// <summary>
        /// 音频播放中回调
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="onUpdateEventHandler"></param>
        /// <returns></returns>
        public static AudioSource OnUpdate(this AudioSource audioSource, AudioEventDriver.OnUpdateEventHandler onUpdateEventHandler)
        {
            GetEventDriver(audioSource).OnUpdate(audioSource, onUpdateEventHandler);
            return audioSource;
        }

        /// <summary>
        /// 音频播放完成回调
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="onUpdateEventHandler"></param>
        /// <returns></returns>
        public static AudioSource OnComplete(this AudioSource audioSource, AudioEventDriver.OnCompleteEventHandler onCompleteEventHandler)
        {
            GetEventDriver(audioSource).OnComplete(audioSource, onCompleteEventHandler);
            return audioSource;
        }
    }
}