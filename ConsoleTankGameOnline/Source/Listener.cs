using ConsoleTankGameOnline.Source.Interface;

namespace ConsoleTankGameOnline.Source
{
    public class Listener
    {
        public delegate void ShellDestroyHandler();
        public static event ShellDestroyHandler? OnShellDestroyed;

        public delegate void WorldHandler(string path);
        public static event WorldHandler? OnWorldCreated;

        public delegate void PlayerHandler(CharacterBase character, bool sendPacket);
        public static event PlayerHandler? OnPlayerAdded;

        public static void DestroyShell()
        {
            OnShellDestroyed?.Invoke();
        }

        public static void CreateWorld(string path)
        {
            OnWorldCreated?.Invoke(path);
        }

        public static void AddPlayer(CharacterBase character, bool sendPacket)
        {
            OnPlayerAdded?.Invoke(character, sendPacket);
        }
    }
}
