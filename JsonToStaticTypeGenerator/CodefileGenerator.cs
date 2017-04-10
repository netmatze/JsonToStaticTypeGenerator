using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JsonToStaticTypeGenerator
{
    public class CodeFileGenerator
    {
        public void Generate(string path, string objectname, string code)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                var fileToWrite = string.Format("{0}\\{1}.cs", path, objectname);
                FileInfo fileInfo = new FileInfo(fileToWrite);
                using (FileStream fileStream = fileInfo.Create())
                {                   
                    using (StreamWriter streamWriter = 
                        new StreamWriter(fileStream, Encoding.Unicode)) 
                    {
                        streamWriter.Write(code);
                    }                                       
                }                
            }
        }
    }
}
