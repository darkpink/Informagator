using Informagator.Contracts;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Exceptions;
using Informagator.Contracts.Stages;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informagator.CommonComponents.ConsumerStages
{
    public class StaticOutputFolderConsumer : IConsumerStage
    {
        [ConfigurationParameter]
        public string FolderPath { get; set; }

        public string Consume(IMessage message)
        {
            var fileName = Guid.NewGuid().ToString();
            var fullPath = Path.Combine(FolderPath, fileName);

            using (FileStream outFileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                outFileStream.Write(message.BinaryData, 0, message.BinaryData.Length);
                outFileStream.Close();
            }
            
            return FolderPath;
        }

        public void ValidateSettings()
        {
            if (String.IsNullOrWhiteSpace(FolderPath))
            {
                throw new ConfigurationException("FolderPath must be configured for StaticOutputFolderConsumer");
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

        public string Name
        {
            get { return "StaticOutputFolderConsumer"; }
        }
    }
}
