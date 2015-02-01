using Informagator.Contracts;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.ErrorHandlers
{
    //TODO: need to add a date stamp on the file, bool config param to cycle per date, cleanup after x days,
    //maybe even allow the filename to include a regexish date stamp
    public class LogFileErrorHandler : IMessageErrorHandler
    {
        [ConfigurationParameter(DisplayName = "Log Folder")]
        public string FolderPath { get; set; }

        [ConfigurationParameter(DisplayName = "File Name")]
        public string FileName { get; set; }

        public IList<string> ContextInfo { get; set; }

        public void Handle(IList<string> info, Exception ex, IMessage message)
        {
            var fullPath = Path.Combine(FolderPath, FileName);

            using (FileStream outFileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(outFileStream))
            {
                //TODO add context info
                writer.WriteLine(info);
                writer.WriteLine(ex.ToString());
                outFileStream.Write(message.BinaryData, 0, message.BinaryData.Length);
                outFileStream.Close();
            }
        }

        public void ValidateSettings()
        {
            if (String.IsNullOrWhiteSpace(FolderPath))
            {
                throw new ConfigurationException("FolderPath must be configured for LogFileErrorHandler");
            }

            if (String.IsNullOrWhiteSpace(FileName))
            {
                throw new ConfigurationException("FileName must be configured for LogFileErrorHandler");
            }

            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException("Unable to create directory " + FolderPath, ex);
                }
            }
        }
    }
}
