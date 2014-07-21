using Acadian.Informagator.Configuration;
using Acadian.Informagator.Exceptions;
using Acadian.Informagator.Infrastructure;
using Acadian.Informagator.Messages;
using Acadian.Informagator.Stages;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.CommonComponents.ConsumerStages
{
    public class OutputFolderConsumer : IConsumerStage
    {
        [ConfigurationParameter]
        public string FolderPath { get; set; }

        public void ConsumeMessage(IMessage message)
        {
            ValidateSettings();

            var fileName = Guid.NewGuid().ToString();
            var fullPath = Path.Combine(FolderPath, fileName);

            using (FileStream outFileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                outFileStream.Write(message.BinaryData, 0, message.BinaryData.Length);
                outFileStream.Close();
            }
        }

        protected void ValidateSettings()
        {
            if (String.IsNullOrWhiteSpace(FolderPath))
            {
                throw new ConfigurationException("FolderPath must be configured for OutputFolderConsumer");
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
