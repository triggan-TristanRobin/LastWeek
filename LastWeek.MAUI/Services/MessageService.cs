namespace LastWeek.MAUI.Services
{
    public class MessageService
    {
        private int timeout = 5;

        private string message = string.Empty;
        public string Message
        {
            get => message;
            set
            {
                if (message != value)
                {
                    message = value;
                    NotifyMessageUpdated?.Invoke();
                    if (!string.IsNullOrEmpty(value))
                    {
#pragma warning disable CA2011 // Avoid infinite recursion
                        Task.Delay(timeout * 1000).ContinueWith(o => Message = "").Wait();
#pragma warning restore CA2011 // Avoid infinite recursion
                    }
                }
            }
        }

        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    NotifyErrorMessageUpdated?.Invoke();
                    if (!string.IsNullOrEmpty(value))
                    {
#pragma warning disable CA2011 // Avoid infinite recursion
                        Task.Delay(timeout * 1000).ContinueWith(o => ErrorMessage = "").Wait();
#pragma warning restore CA2011 // Avoid infinite recursion
                    }
                }
            }
        }


        public event Action NotifyMessageUpdated = null;
        public event Action NotifyErrorMessageUpdated = null;
    }
}
