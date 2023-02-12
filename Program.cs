using AscMuteSounds.Properties;
using System;
using System.Diagnostics;
using System.IO;

namespace AscMuteSounds
{
    internal class Program
    {
        static void Exit()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            string sounds = "sounds.txt";
            if (!File.Exists(sounds))
            {
                File.Create(sounds).Dispose();
                Console.WriteLine(
                    "1st time running! please modify the sounds.txt file to" +
                    "\ninclude any in-game sounds that you wish to mute."
                );

                string ogg = "empty.ogg";
                File.Create(ogg).Dispose();
                File.SetAttributes(ogg, File.GetAttributes(ogg) | FileAttributes.Hidden);

                string wav = "empty.wav";
                File.Create(wav).Dispose();
                using (StreamWriter writer = new StreamWriter(wav))
                {
                    writer.WriteLine(
                        "RIFF$   WAVEfmt \u0010   \u0001 \u0001 D¬  ˆX\u0001 \u0002 \u0010 data"
                    );
                    // Empty wav file
                }
                File.SetAttributes(wav, File.GetAttributes(wav) | FileAttributes.Hidden);

                string mpq = "MPQEditor.exe";
                File.WriteAllBytes(mpq, Resources.MPQEditor);
                File.SetAttributes(mpq, File.GetAttributes(mpq) | FileAttributes.Hidden);

                Exit();
            }

            FileInfo fileInfo = new FileInfo(sounds);
            if (fileInfo.Length == 0)
            {
                Console.WriteLine(
                    "add the sounds you want to mute in the sounds.txt file." +
                    "\nput each sound on a new line. check the instructions folder."
                );
                Exit();
            }
            else
            {
                string mpq = "patch-MS.mpq";
                if (File.Exists(mpq))
                {
                    File.Delete(mpq);
                }
                Process.Start("MPQEditor.exe", "n " + mpq + " 0x8000");
                using (StreamReader reader = new StreamReader(sounds))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Process.Start("MPQEditor.exe", "a " + mpq + " empty.wav Sound\\" + line + ".wav");
                        Process.Start("MPQEditor.exe", "a " + mpq + " empty.ogg Sound\\" + line + ".ogg");
                    }
                }
                Console.WriteLine(
                    "please move the generated patch into the Ascension_Client/Data folder," +
                    "\nthen log into the game to confirm whether the specified audio files" +
                    "\nhave been successfully muted." +
                    "\n\nif you experience any issues, report them on the GitHub repository" +
                    "\n(https://github.com/mattibalize-lab/AscMuteSounds)"
                );
                Exit();
            }
        }
    }
}
