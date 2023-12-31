namespace ConsoleTankGameOnline.Source
{
    public class Listener
    {
        public delegate void ShellDestroyHandler();
        public static event ShellDestroyHandler? OnShellDestroyed;

        public static void ShellDestroy()
        {
            OnShellDestroyed?.Invoke();
        }
    }
}
