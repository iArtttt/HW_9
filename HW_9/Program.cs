using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace HW_9
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Cmd cmd = new Cmd();
            cmd.Start();

        }
    }
    public class Cmd
    {
        private int _index = 0;
        private string _currentPass = string.Empty;
        private string _defaultPass = "c:\\";
        private List<string> _list;
        private bool _isFinish = false;
        public Cmd()
        {
            _currentPass = _defaultPass;
            _list = Directory.EnumerateFileSystemEntries(_currentPass).ToList();
        }
        public Cmd(string currentPass)
        {
            if (currentPass != null)
                _currentPass = currentPass;
            else
                _currentPass = _defaultPass;
            try
            {
                _list = Directory.EnumerateFileSystemEntries(_currentPass).ToList();
            }
            catch 
            { 
                _currentPass = _defaultPass;
                _list = Directory.EnumerateFileSystemEntries(_currentPass).ToList();
            }
        }

        public void Start()
        {
            while (!_isFinish)
            {
                Print();
                MoveEnter();
            }
            Console.Clear();
            _isFinish = false;
        }
        private void Print()
        {
            Console.Clear();
            try
            {
                _list = Directory.EnumerateFileSystemEntries(_currentPass).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Access denied");
                Console.ResetColor();
                _currentPass = Directory.GetParent(_currentPass).ToString();
                _list = Directory.EnumerateFileSystemEntries(_currentPass).ToList();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
                _currentPass = Directory.GetParent(_currentPass).ToString();
                _list = Directory.EnumerateFileSystemEntries(_currentPass).ToList();
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(_currentPass);
            Console.ResetColor();

            for (int i = 0; i < _list.Count; i++)
            {
                if (i == _index) Console.ForegroundColor = !File.Exists(_list[i]) ? ConsoleColor.DarkGreen : ConsoleColor.Green;
                else Console.ForegroundColor = !File.Exists(_list[i]) ? ConsoleColor.Yellow : ConsoleColor.White;
                Console.WriteLine(string.Join(null, _list[i].Skip(_currentPass.Length)));
                Console.ResetColor();
            }
        }
        private void PrintFile()
        {
            using var j = new StreamReader(_list[_index]);
            var read = j.ReadToEnd();
            Console.Clear();
            foreach (var c in read)
                Console.Write(c);
            j.Close();
            Console.ReadKey();
        }
        private void MoveEnter()
        {
            var key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    if (_index + 1 > _list.Count - 1)
                        _index = 0;
                    else
                        _index++;
                    break;
                case ConsoleKey.UpArrow:
                    if (_index - 1 < 0)
                        _index = _list.Count - 1;
                    else
                        _index--;
                    break;
                case ConsoleKey.Enter:
                    try
                    {
                        PrintFile();
                    }
                    catch
                    {
                        _currentPass = _list[_index];
                        _index = 0;
                    }
                    break;
                case ConsoleKey.Escape:
                    try
                    {
                        _currentPass = Directory.GetParent(_currentPass).ToString();
                        _index = 0;
                    }
                    catch
                    {
                        _isFinish = true;
                    }
                    break;
                case ConsoleKey.Backspace: goto case ConsoleKey.Escape;
            }
        }
    }
}