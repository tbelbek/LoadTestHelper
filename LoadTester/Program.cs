using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadTester
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = "Testlerin bulunduğu klasörü seçiniz";
            fbd.ShowNewFolderButton = false;
            string testsFolder = string.Empty;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                testsFolder = fbd.SelectedPath;
            }
            fbd.Description = "jMeter klasörünü seçiniz";
            fbd.ShowNewFolderButton = false;
            string jmeterFolder = string.Empty;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                jmeterFolder = Path.Combine(fbd.SelectedPath, "bin");
            }

            var testsPath = Path.Combine(testsFolder);
            string[] allfiles = System.IO.Directory.GetFiles(testsPath, "*.jmx", System.IO.SearchOption.AllDirectories);
            allfiles.ToList().ForEach(i =>
            {
                int index = allfiles.ToList().FindIndex(a => a == i);
                Console.Write($"{index + 1}-{i.Split('\\').Last()}\n");
            });
            Console.WriteLine("Test seciniz");
            var testIndexOption = Console.ReadLine();
            var testIndex = 0;
            Int32.TryParse(testIndexOption?.ToString(), out testIndex);
            var testName = allfiles.ToList()[testIndex - 1];

            Console.WriteLine("Thread,zaman ve ramp sayısı ayarlamak istiyor musunuz? (y/n)");
            var testTuning = Console.ReadKey();
            string command;
            if (testTuning.KeyChar.ToString() == "y")
            {

                Console.WriteLine("Thread sayısı seçiniz");
                var testThreadOption = Console.ReadLine();
                var testThread = 0;
                Int32.TryParse(testThreadOption?.ToString(), out testThread);
                Console.WriteLine("Test süresini seçiniz (saniye cinsinden)");
                var testTimeOption = Console.ReadLine();
                var testTime = 0;
                Int32.TryParse(testTimeOption?.ToString(), out testTime);
                Console.WriteLine("Test istek aralık süre uzunluğunu seçiniz (saniye cinsinden)");
                var testRampOption = Console.ReadLine();
                var testRamp = 0.1;
                Double.TryParse(testRampOption?.ToString(), out testRamp);


                command = $"{Path.Combine(jmeterFolder, "jmeter")} -Jthreads={testThread} -Jrampup={testRamp} -Jduration={testTime} -n -t \"{Path.Combine(testsFolder, testName)}\"";
            }
            else
            {
                command = $"{Path.Combine(jmeterFolder, "jmeter")} -n -t \"{Path.Combine(testsFolder, testName)}\"";
            }



            ExecuteCommand(command);
            Console.ReadKey();
        }

        static void ExecuteCommand(string command)
        {
            //int exitCode;
            //ProcessStartInfo processInfo;
            //Process process;

            //processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            //processInfo.CreateNoWindow = true;
            //processInfo.UseShellExecute = false;
            //// *** Redirect the output ***
            //processInfo.RedirectStandardError = true;
            //processInfo.RedirectStandardOutput = true;

            //process = Process.Start(processInfo);
            //process.WaitForExit();

            //// *** Read the streams ***
            //// Warning: This approach can lead to deadlocks, see Edit #2
            //string output = process.StandardOutput.ReadToEnd();
            //string error = process.StandardError.ReadToEnd();

            //exitCode = process.ExitCode;

            //Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            //Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            //Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            //process.Close();
            Process.Start(@"cmd", $"/c {command}");
        }
    }
}
