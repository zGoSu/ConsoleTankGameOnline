using ConsoleTankGameOnline.Source.Interface;
using ConsoleTankGameOnline.Source.Structure;

namespace ConsoleTankGameOnline.Source
{
    public class Listener
    {
        public delegate void ShellDestroyHandler();
        public static event ShellDestroyHandler? OnShellDestroyed;

        public delegate void WorldHandler(char[,] map);
        public static event WorldHandler? OnWorldCreated;

        public delegate void PlayerHandler(CharacterBase character, bool sendPacket);
        public static event PlayerHandler? OnPlayerAdded;

        public delegate void MoveHandler(string objectName, Position position, bool sendPacket);
        public static event MoveHandler? OnMove;

        public static void DestroyShell()
        {
            OnShellDestroyed?.Invoke();
        }

        public static void CreateWorld(char[,] map)
        {
            OnWorldCreated?.Invoke(map);
        }

        public static void AddPlayer(CharacterBase character, bool sendPacket)
        {
            OnPlayerAdded?.Invoke(character, sendPacket);
        }

        public static void MoveTo(string objectName, Position position, bool sendPacket)
        {
            OnMove?.Invoke(objectName, position, sendPacket);
        }
    }
}
