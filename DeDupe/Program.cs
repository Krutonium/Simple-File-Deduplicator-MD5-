using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace DeDupe
{
    class Program
    {
        static List<string> toDelete = new List<string>();
        static void Main(string[] args)
        {
            //C:\Users\pfckr\Dropbox\IFTTT\reddit\yiff
            Searcher(@"C:\Users\pfckr\Dropbox\IFTTT\reddit\", true); //Change this as needed
            Console.WriteLine("Finished Indexing...");
            DeleteFiles();                                           //Delete the Files, since they are no longer being iterated over.
            Console.WriteLine("Done");
            Console.ReadKey();
            
        }
        static private void DeleteFiles()
        {
            foreach(var file in toDelete)
            {
                Console.Write("Deleting ");
                Console.WriteLine(file);
                File.Delete(file);
            }
        }
        static private void Searcher(string rootFolder, bool recursive)
        {
            Files(rootFolder);  //The whole purpose of this is to iterate through all files (and optionally subdirectories) to scan them for dupes.
            if (recursive)      //It calls itself for each sub directory. Once all subdirectories are exhausted, it will exit back to Main().
            {
                foreach (var Sub in Directory.GetDirectories(rootFolder))
                {
                    Searcher(Sub, recursive);
                }
            }
        }
        static private void Files(string folder)
        {
            foreach (var filename in Directory.GetFiles(folder))
            {
                using (var md5 = MD5.Create())
                {
                    try
                    {
                        using (var stream = File.OpenRead(filename))
                        {
                            var preprocess = (md5.ComputeHash(stream));
                            var s = new StringBuilder();
                            foreach (byte b in preprocess)
                            {
                                s.Append(b.ToString("x2").ToLower());
                            }
                            string Hash = s.ToString();                   //Human Readable MD5
                            var filename2 = Path.GetFileName(filename);
                            Console.Write(filename2);
                            if (Hash == "3993028fcea692328e097de50b26f540") //Change this to the file you wish to delete dupes of.
                            {
                                Console.CursorLeft = Console.BufferWidth / 2 - 4;
                                Console.Write("Deleting");
                                toDelete.Add(filename); //Can't Delete while it's being iterated over.
                            }
                            Console.CursorLeft = Console.BufferWidth - Hash.Length;
                            Console.Write(Hash);
                        }
                    }
                    catch { }
                }
            }
        }
    }
}
