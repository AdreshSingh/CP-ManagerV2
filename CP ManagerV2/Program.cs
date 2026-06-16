using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;


namespace CP_ManagerV2
{
    internal class Program
    {
        private static List<string> mcqParts = new List<string>();
        private static int currentIndex = 0;
        private static IKeyboardMouseEvents globalHook;
        private static int userChoice = 0;

        delegate void ParseMethod(string text);
        private static Dictionary<int, ParseMethod> methods;

        [STAThread]
        private static void Main(string[] args)
        {
            Console.WriteLine("MCQ Clipboard Manager Running...");
            Console.WriteLine("Copy an MCQ and then use Ctrl+V to paste sequential parts.");
            Console.WriteLine("Press ESC to exit.\n");
            Console.WriteLine("Select Parsing Mode:");
            Console.WriteLine("1. Full MCQ (Question, Options, Explanation)");
            Console.WriteLine("2. For MCQs without Question (Options, Explanation)");
            Console.WriteLine("3. For MCQs Options");

            methods = new Dictionary<int, ParseMethod>
            {
                { 1, ParseMcq },
                { 2, ParseMcqOptionsExplanation },
                { 3, ParseMcqOptions }
            };

            userChoice = int.Parse(Console.ReadLine() ?? "0");



            // Hook into global keyboard events
            globalHook = Hook.GlobalEvents();
            globalHook.KeyDown += GlobalHook_KeyDown;

            // Keep app running
            Application.Run();
        }
        private static void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            // Detect Ctrl+C -> check clipboard
            if (e.Control && e.KeyCode == Keys.C)
            {
                Console.WriteLine("Copying detected..");
                Thread.Sleep(200); // wait a moment for clipboard to update
                string text = Clipboard.GetText();
                methods[userChoice](text);
            }

            // Detect Ctrl+V -> paste next part
            if (e.Control && e.KeyCode == Keys.V)
            {
                Console.WriteLine("Pasting detected..");
                if (mcqParts.Count > 0 && currentIndex < mcqParts.Count)
                {
                    Clipboard.SetText(mcqParts[currentIndex]);
                    currentIndex++;
                }
            }

            // Reset sequence with Ctrl+Shift+R
            if (e.Control && e.Shift && e.KeyCode == Keys.R)
            {
                currentIndex = 0;
                mcqParts.Clear();
                Console.WriteLine("🔄 Sequence reset.");
            }

            // Exit with ESC
            if (e.KeyCode == Keys.Escape)
            {
                Console.WriteLine("👋 Exiting...");
                globalHook.Dispose();
                Application.Exit();
            }
        }

        private static void ParseMcq(string text)
        {
            mcqParts.Clear();
            currentIndex = 0;

            Console.WriteLine("Parsing MCQ..");

            // 1. Extract question- Q 1., Question 1.
            var questionMatch = Regex.Match(text, @"^\s*(?:(?:Question|Q)\s*)?(\d+)[.)]?\s*(.+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (questionMatch.Success)
            {
                Console.WriteLine("Question Detected...");
                mcqParts.Add(questionMatch.Groups[2].Value.Trim());
            }

            // 2. Extract options: lines starting with a), b), c), d), A), B), C), D), A., B., C., D. etc.
            var optionMatches = Regex.Matches(text, @"^\s*[a-dA-D][\.\)]\s+(.+)", RegexOptions.Multiline);
            foreach (Match opt in optionMatches)
            {
                Console.WriteLine("Option Detected...");
                mcqParts.Add(opt.Groups[1].Value.Trim());
            }

            // 3. Extract explanation/rationale
            var rationaleMatch = Regex.Match(text, @"^\s*(?:rationale|explanation|solution)[\s:]+(.+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (rationaleMatch.Success)
            {
                Console.WriteLine("Rationale Detected...");
                mcqParts.Add(rationaleMatch.Groups[1].Value.Trim());
            }

            Console.WriteLine($"📋 Parsed MCQ into {mcqParts.Count} parts.");
            foreach (String d in mcqParts)
            {
                Console.WriteLine($" - {d}");
            }
        }


        private static void ParseMcqOptionsExplanation(string text)
        {
            mcqParts.Clear();
            currentIndex = 0;

            Console.WriteLine("Parsing MCQ..");

            // 1. Extract question
            //var questionMatch = Regex.Match(text, @"^\s*Q\d+[\.\)]?\s*(.+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //if (questionMatch.Success)
            //{
            //    Console.WriteLine("Question Detected...");
            //    mcqParts.Add(questionMatch.Groups[1].Value.Trim());
            //}

            // 2. Extract options: lines starting with a), b), c), d)
            var optionMatches = Regex.Matches(text, @"^\s*[a-dA-D][\.\)]\s+(.+)", RegexOptions.Multiline);
            foreach (Match opt in optionMatches)
            {
                Console.WriteLine("Option Detected...");
                mcqParts.Add(opt.Groups[1].Value.Trim());
            }

            // 3. Extract explanation/rationale
            var rationaleMatch = Regex.Match(text, @"^\s*(?:rationale|explanation|solution)[\s:]+(.+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (rationaleMatch.Success)
            {
                Console.WriteLine("Rationale Detected...");
                mcqParts.Add(rationaleMatch.Groups[1].Value.Trim());
            }

            Console.WriteLine($"📋 Parsed MCQ into {mcqParts.Count} parts.");
            foreach (String d in mcqParts)
            {
                Console.WriteLine($" - {d}");
            }
        }

        private static void ParseMcqOptions(string text)
        {
            mcqParts.Clear();
            currentIndex = 0;

            Console.WriteLine("Parsing MCQ..");

            // 1. Extract question
            //var questionMatch = Regex.Match(text, @"^\s*Q\d+[\.\)]?\s*(.+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //if (questionMatch.Success)
            //{
            //    Console.WriteLine("Question Detected...");
            //    mcqParts.Add(questionMatch.Groups[1].Value.Trim());
            //}

            // 2. Extract options: lines starting with a), b), c), d)
            var optionMatches = Regex.Matches(text, @"^\s*[a-dA-D][\.\)]\s+(.+)", RegexOptions.Multiline);
            foreach (Match opt in optionMatches)
            {
                Console.WriteLine("Option Detected...");
                mcqParts.Add(opt.Groups[1].Value.Trim());
            }

            // 3. Extract explanation/rationale
            //var rationaleMatch = Regex.Match(text, @"^\s*(?:rationale|explanation|solution)[\s:]+(.+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //if (rationaleMatch.Success)
            //{
            //    Console.WriteLine("Rationale Detected...");
            //    mcqParts.Add(rationaleMatch.Groups[1].Value.Trim());
            //}

            Console.WriteLine($"📋 Parsed MCQ into {mcqParts.Count} parts.");
            foreach (String d in mcqParts)
            {
                Console.WriteLine($" - {d}");
            }
        }

    }
}
