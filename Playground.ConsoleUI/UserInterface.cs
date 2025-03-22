using Playground.ConsoleUI.Core;
using Playground.ConsoleUI.DisplayComponents;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Playground.ConsoleUI
{
    //public static class UserInterface
    //{
    //    public static void Display(params IDisplayComponent[] components)
    //    {
    //        foreach (var component in components)
    //        {
    //            foreach (var line in component.GetContents())
    //            {
    //                Display(line);
    //            }
    //        }
    //    }

    //    public static void Display(params DisplayedLine[] displayedLines)
    //    {
    //        foreach (var line in displayedLines)
    //        {
    //            var position = line.GetPosition();

    //            Console.SetCursorPosition(position.X, position.Y);

    //            foreach (var text in line.Contents)
    //            {
    //                Console.ForegroundColor = text.Color.Font;
                 
    //                Console.BackgroundColor = text.Color.Background;

    //                Console.Write(text.Value);
    //            }
    //        }
    //    }
    //}

    public class UserInterface : IDisposable
    {
        private readonly ConcurrentQueue<IDisplayComponent> _displayQueue = new();
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly object _consoleLock = new object();
        private bool _disposed = false;

        public UserInterface()
        {
            Task.Run(ProcessDisplayQueueAsync);
        }

        ~UserInterface()
        {
            Dispose();
        }

        public void Display(params IDisplayComponent[] components)
        {
            foreach (var component in components)
            {
                _displayQueue.Enqueue(component);
            }
        }

        private async Task ProcessDisplayQueueAsync()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (_displayQueue.TryDequeue(out var component))
                {
                    foreach (var line in component.GetContents())
                    {
                        lock (_consoleLock)
                        {
                            Console.SetCursorPosition(line.GetPosition().X, line.GetPosition().Y);
                            foreach (var text in line.Contents)
                            {
                                Console.ForegroundColor = text.Color.Font;
                                Console.BackgroundColor = text.Color.Background;
                                Console.Write(text.Value);
                            }
                        }
                    }
                }
                else
                {
                    await Task.Delay(2);
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                var stopwatch = Stopwatch.StartNew();
                var timeout = TimeSpan.FromSeconds(10);

                while (!_displayQueue.IsEmpty && stopwatch.Elapsed < timeout)
                {
                    Thread.Sleep(10);
                }
            
                _cancellationTokenSource.Cancel();

                if (!_displayQueue.IsEmpty)
                {
                    //TODO: Log if not all components are printed before UserInterface is disposed?
                }

                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}
