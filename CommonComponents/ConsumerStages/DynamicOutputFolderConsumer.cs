using Informagator.Configuration;
using Informagator.Exceptions;
using Informagator.Contracts;
using Informagator.Messages;
using Informagator.Stages;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.ConsumerStages
{
    public class DynamicOutputFolderConsumer : IConsumerStage
    {
        [ConfigurationParameter]
        public string FolderPathAttribute { get; set; }

        public string Consume(IMessage message)
        {
            ValidateSettings();

            var fileName = Guid.NewGuid().ToString();
            var directory = message.Attributes[FolderPathAttribute];
            ValidateDirectory(directory);
            var fullPath = Path.Combine(directory, fileName);

            using (FileStream outFileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                outFileStream.Write(message.BinaryData, 0, message.BinaryData.Length);
                outFileStream.Close();
            }
            
            return FolderPathAttribute;
        }

        public void ValidateSettings()
        {
            if (String.IsNullOrWhiteSpace(FolderPathAttribute))
            {
                throw new ConfigurationException("FolderPathAttribute must be configured for DynamicOutputFolderConsumer");
            }
        }
        protected void ValidateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException("Unable to create directory " + directory, ex);
                }
            }
        }

        public string Name
        {
            get { return "DynamicOutputFolderConsumer"; }
        }
    }
}
