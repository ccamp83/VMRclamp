//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using System.Net;
//using System.IO;

//namespace IPAlab
//{

//    public class IPAlab_import_data
//    {
//        public static StreamReader openFile(string filePath)
//        {
//            bool isPathURL = filePath.Contains("http");
//            StreamReader file = null;

//            if (!isPathURL)
//            {
//                file = new StreamReader(filePath);
//            }
//            else
//            {
//                WebClient client = new WebClient();
//                Stream stream = client.OpenRead(filePath);
//                file = new StreamReader(stream);
//            }

//            return file;
//        }

//        public IPAlab_import_data(string path, char sep = '\t')
//        {
//            string fileText = null;

//            using (StreamReader streamInput = openFile(path))
//            {
//                fileText = streamInput.ReadToEnd();
//                streamInput.Close();
//            }
//        }
//    }

//    public static class IPAlab_import_data_extensions
//    {

//        public static int totalLines(this IPAlab_import_data data)
//        {
//           // data.filete

//            int i = 0;
//           // while (file.ReadLine() != null) { i++; }
//           // file.Close();

//            return i;
//        }

//        public static string ReadSpecificLine(string filePath, int lineNumber)
//        {
//            string content = null;
//            try
//            {
//                StreamReader file = openFile(filePath);

//                for (int i = 1; i < lineNumber; i++)
//                {
//                    file.ReadLine();

//                    if (file.EndOfStream)
//                    {
//                        Console.WriteLine($"End of file.  The file only contains {i} lines.");
//                        break;
//                    }
//                }
//                content = file.ReadLine();
//                file.Close();
//            }
//            catch (IOException e)
//            {
//                Console.WriteLine("There was an error reading the file: ");
//                Console.WriteLine(e.Message);
//            }

//            return content;

//        }
//    }
    

//}