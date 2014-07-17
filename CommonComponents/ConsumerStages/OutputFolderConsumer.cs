using Acadian.Informagator.Configuration;
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
    public class OutputFolderConsumer : ConsumerStage
    {
        [ConfigurationParameter]
        public string FolderPath { get; set; }

        public OutputFolderConsumer(IMessageErrorHandler errorHandler)
            :base(errorHandler)
        {
        }

        protected override void Consume(IMessage message)
        {
            var fileName = Guid.NewGuid().ToString();
            var fullPath = Path.Combine(FolderPath, fileName);

            using (FileStream outFileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                outFileStream.Write(message.Body, 0, message.Body.Length);
                outFileStream.Close();
            }
        }
    }
}
