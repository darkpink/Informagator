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
    public class OutputFolderConsumer : IProcessingStage
    {
        [ConfigurationParameter]
        public string FolderPath { get; set; }

        public IMessage Execute(IMessage msgIn)
        {
            ValidateSettings();

            var fileName = Guid.NewGuid().ToString();
            var fullPath = Path.Combine(FolderPath, fileName);

            using (FileStream outFileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                outFileStream.Write(msgIn.BinaryData, 0, msgIn.BinaryData.Length);
                outFileStream.Close();
            }

            return null;
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
