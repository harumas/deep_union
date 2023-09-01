using Module.Task;
using VContainer;
using VContainer.Unity;
using Wanna.DebugEx;

namespace GameMain.Presenter.Resource
{
    /// <summary>
    /// タスクとリソース管理クラスのプレゼンタークラス
    /// </summary>
    public class ResourcePresenter : IInitializable
    {
        private readonly ResourceContainer resourceContainer;
        private readonly CollectableTask[] collectableTasks;

        [Inject]
        public ResourcePresenter(ResourceContainer resourceContainer)
        {
            this.resourceContainer = resourceContainer;
            collectableTasks = TaskUtil.FindSceneTasks<CollectableTask>();

            resourceContainer.OnResourceChanged += (prev, current) =>
            {
                DebugEx.Log($"リソースが増えました{prev} => {current}");
            };
        }

        public void Initialize()
        {
            foreach (var collectableTask in collectableTasks)
            {
                collectableTask.OnCollected += count =>
                {
                    resourceContainer.Add(count);
                };
            }
        }
    }
}