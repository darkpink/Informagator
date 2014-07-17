using Acadian.Informagator.Configuration;
using Acadian.Informagator.Infrastructure;
using Acadian.Informagator.Messages;
using Acadian.Informagator.Stages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acadian.Informagator.CommonComponents.SupplierStages
{
    public class OldestFileFromFolderSupplier : SupplierStage
    {
        public OldestFileFromFolderSupplier(IMessageErrorHandler errorHandler)
            : base(errorHandler)
        {
        }

        [ConfigurationParameter]
        public string FolderPath { get; set; }

        protected override IMessage GetMessage()
        {
            ByteArrayMessage result = null;

            string[] files = Directory.GetFiles(FolderPath);
            var fileCreation = new Dictionary<string, DateTime>();
            foreach (string file in files)
            {
                string fullPath = Path.Combine(FolderPath, file);
                fileCreation.Add(file, File.GetCreationTime(fullPath));
            }

            foreach (var file in fileCreation.OrderBy(kvp => kvp.Value))
            {
                using (FileStream inFile = new FileStream(file.Key, FileMode.Open, FileAccess.Read))
                {
                    if (inFile.Length > 0)
                    {
                        byte[] bytes = new byte[inFile.Length];
                        inFile.Read(bytes, 0, bytes.Length);
                        result = new ByteArrayMessage();
                        result.Body = bytes;
                        result.Attributes.Add("OriginalFileName", file.Key);
                        inFile.Close();
                        File.Delete(file.Key);
                        break;
                    }
                }
            }

            return result;
        }

    }
}
