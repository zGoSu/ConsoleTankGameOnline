namespace ConsoleTankGameOnline.Source
{
    public class Listener
    {
        public delegate void ShellDestroyHandler();
        public static event ShellDestroyHandler? OnShellDestroyed;

        public delegate void WorldHandler(string path);
        public static event WorldHandler? OnWorldSelected;

        public delegate void PlayerHandler(string path);
        public static event PlayerHandler? OnPlayerSelected;

        public static void ShellDestroy()
        {
            OnShellDestroyed?.Invoke();
        }

        public static void SelectWorld(string path)
        {
            OnWorldSelected?.Invoke(path);
        }

        public static void SelecterPlayer(string path)
        {
            OnPlayerSelected?.Invoke(path);
        }
    }
}
