using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace LibNoiseShaderEditor
{
    public sealed class TaskCompletionNotifier<TResult> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TaskCompletionNotifier(Task<TResult> task)
        {
            Task = task;

            if (!task.IsCompleted)
            {
                var scheduler = (SynchronizationContext.Current == null) ? TaskScheduler.Current : TaskScheduler.FromCurrentSynchronizationContext();

                task.ContinueWith(t =>
                {
                    var propertyChanged = PropertyChanged;

                    if (propertyChanged != null)
                    {
                        propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));

                        if (t.IsCanceled)
                        {
                            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCanceled)));
                        }
                        else if (t.IsFaulted)
                        {
                            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsFaulted)));
                            propertyChanged(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
                        }
                        else
                        {
                            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsSuccessfullyCompleted)));
                            propertyChanged(this, new PropertyChangedEventArgs(nameof(Result)));
                        }
                    }
                },
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                scheduler);
            }
        }

        public Task<TResult> Task { get; }

        public TResult Result => IsSuccessfullyCompleted ? Task.Result : default;

        public bool IsCompleted => Task.IsCompleted;

        public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;

        public bool IsCanceled => Task.IsCanceled;

        public bool IsFaulted => Task.IsFaulted;

        public string ErrorMessage => Task.Exception?.Message;
    }
}
