namespace ConsoleTankGameOnline.Source
{
    public class Logger
    {
        public Logger()
        {
            CreateFile();
            Task.Run(SaveFile);
        }

        private const string _file = "error.log";
        private readonly Queue<string> _errors = [];
        public static readonly Logger Instance = new();

        public void Write(string message)
        {
            message = $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] ERROR {message}";
            _errors.Enqueue(message);
        }
        public void Write(Exception exception)
        {
            Write($"{exception.Message}\nStackTrace: {exception.StackTrace}");
        }

        private void CreateFile()
        {
            var file = $"{Directory.GetCurrentDirectory()}\\{_file}";

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            File.Create(file).Close();
        }

        private void SaveFile()
        {
            while (true)
            {
                using var stream = new StreamWriter($"{Directory.GetCurrentDirectory()}\\{_file}", true);

                while (_errors.Count > 0)
                {
                    var error = _errors.Dequeue();

                    stream.Write(error);
                }

                Task.Delay(1000);
            }
        }
    }
}
