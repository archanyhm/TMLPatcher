﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Consolation.Common.Framework.OptionsSystem
{
    public class ConsoleOptions : ICloneable, IList<ConsoleOption>
    {
        #region ConsoleOptions Code

        public ConsoleOption this[int index]
        {
            get => _options[index];

            set => _options[index] = value;
        }

        public string OptionText { get; }

        public bool DisplayReturn { get; set; } = true;

        public bool DisplayGoBack { get; set; } = true;

        private readonly List<ConsoleOption> _options;

        private readonly ConsoleOptions _prevOptionsState;

        public ConsoleOptions(string optionText, params ConsoleOption[] options)
        {
            OptionText = optionText;
            _prevOptionsState = ConsoleAPI.SelectedOptionSet;

            // Assign index values to the ConsoleOption types
            for (int i = 0; i < options.Length; i++)
                options[i].Index = i;

            _options = options.ToList();
        }

        public virtual void ListForOption()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(ToString());
                string key = Console.ReadLine();

                switch (key)
                {
                    case null:
                        ConsoleAPI.Window.WriteAndClear("Whoops! The entered value returned null. Please only enter actual numbers (1, 5, 27, etc.).");
                        continue;

                    case "/" when DisplayReturn:
                        ConsoleAPI.Window.WriteAndClear("Returned to the start!", ConsoleColor.Green);
                        ConsoleAPI.SelectedOptionSet = ConsoleAPI.Window.DefaultOptions;
                        ConsoleAPI.SelectedOptionSet.ListForOption();
                        return;

                    case "." when DisplayGoBack:
                        if (_prevOptionsState == null)
                            ConsoleAPI.Window.WriteAndClear("No previous state was found, falling back to the beginning...");
                        else
                        {
                            ConsoleAPI.Window.WriteAndClear("Returning to the previous options menu...", ConsoleColor.Green);
                            _prevOptionsState.ListForOption();
                        }
                        return;
                }

                if (!int.TryParse(key, out int option))
                {
                    ConsoleAPI.Window.WriteAndClear("Whoops! We weren't able to parse your response. Please only enter actual numbers (1, 5, 27, etc.).");
                    continue;
                }

                if (option < 0 || option > Count)
                {
                    ConsoleAPI.Window.WriteAndClear("Whoops! The number entered does not correspond to any of the available options.");
                    continue;
                }

                this.First(x => x.Index == option - 1).Execute();
                break;
            }
        }

        public override string ToString()
        {
            string text = this.Aggregate($" {OptionText}", (current, option) => current + $"\n{option}");

            if (_prevOptionsState != null && DisplayGoBack)
                text += "\n  [.] Return to the previous set of options.";

            if (DisplayReturn)
                return text + "\n  [/] Return to the start.";

            return text;
        }

        #endregion
        
        #region Painful Interface Code

        public int IndexOf(ConsoleOption item) => _options.IndexOf(item);

        public void Insert(int index, ConsoleOption item) => _options.Insert(index, item);

        public void RemoveAt(int index) => _options.RemoveAt(index);

        public IEnumerator<ConsoleOption> GetEnumerator() => _options.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public object Clone() => MemberwiseClone();

        public void Add(ConsoleOption item) => _options.Add(item);

        public void Clear() => _options.Clear();

        public bool Contains(ConsoleOption item) => _options.Contains(item);

        public void CopyTo(ConsoleOption[] array, int arrayIndex) => _options.CopyTo(array, arrayIndex);

        public bool Remove(ConsoleOption item) => _options.Remove(item);

        public int Count => _options.Count;

        public bool IsReadOnly => false;
        
        #endregion
    }
}
