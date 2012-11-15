using IstLight.Services;

namespace IstLight
{
    public enum AsyncState
    {
        Running = 0,
        Error,
        Completed
    }

    public static class AsyncStateExtensions
    {
        public static AsyncState GetState<T>(this IAsyncResult<T> ar)
        {
            return !ar.IsCompleted ?
                AsyncState.Running
                :
                ar.Error != null ?
                    AsyncState.Error
                    :
                    AsyncState.Completed;
        }
    }
}
