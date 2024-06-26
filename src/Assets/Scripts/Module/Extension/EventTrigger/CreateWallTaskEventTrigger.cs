using Module.Extension.Task;
using Module.Task;
using UnityEngine;

namespace Module.Extension.EventTrigger
{
    public class CreateWallTaskEventTrigger : TaskEventTrigger<CreateWallTask> // ←これを継承する <>内は対象のタスクの型
    {
        //AudioClipを用意する
        [SerializeField] private AudioClip CreateSound;

        protected override void OnStart()
        {
            Task.OnStarted += OnTaskStarted;
            Task.OnCanceled += OnTaskCanceled;
            Task.OnCompleted += OnTaskCompleted;
        }

        private void OnTaskStarted(BaseTask _)
        {
            // 作っている音をループで鳴らす
            AudioSource.PlayOneShot(CreateSound);
        }

        private void OnTaskCanceled(BaseTask _)
        {
            // なっている音を止める
            //AudioSource.Stop();
        }

        private void OnTaskCompleted(BaseTask _) { }
    }
}