using ConsoleTankGameOnline.Source.Interface;
using ConsoleTankGameOnline.Source.Structure;

namespace ConsoleTankGameOnline.Source
{
    public class Listener
    {
        public delegate void ShellDestroyHandler(CharacterBase owner);
        public static event ShellDestroyHandler? OnShellDestroyed;

        public delegate void WorldHandler(char[,] map);
        public static event WorldHandler? OnWorldCreated;

        public delegate void PlayerHandler(CharacterBase character, bool sendPacket);
        public static event PlayerHandler? OnPlayerAdded;
        public static event PlayerHandler? OnDie;

        public delegate void MoveHandler(string objectName, Position position, bool sendPacket);
        public static event MoveHandler? OnMove;
        public static event MoveHandler? OnShellMove;

        public static void DestroyShell(CharacterBase owner)
        {
            OnShellDestroyed?.Invoke(owner);
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

        public static void Die(CharacterBase target, bool sendPacket)
        {
            OnDie?.Invoke(target, sendPacket);
        }

        public static void ShellMoveTo(string ownerName, Position shellPosition, bool sendPacket)
        {
            OnShellMove?.Invoke(ownerName, shellPosition, sendPacket);
        }
    }
}
