using Informagator.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informagator.CommonComponents.Messages;
using Informagator.Contracts.Exceptions;
using Informagator.Contracts.Attributes;
using Informagator.Contracts.Stages;

namespace Informagator.CommonComponents.SupplierStages
{
    public class OldestFileFromFolderSupplier : ISupplierStage
    {
        [ConfigurationParameter(DisplayName="Source Folder")]
        public string FolderPath { get; set; }

        public IMessage Supply()
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

        public void ValidateSettings()
        {
            if (String.IsNullOrWhiteSpace(FolderPath))
            {
                throw new ConfigurationException("FolderPath must be configured for OldestFileFromFolderSupplier");
            }

            if (!Directory.Exists(FolderPath))
            {
                throw new ConfigurationException("FolderPath " + FolderPath + " does not exist");
            }

        }

        public string ReceviedFrom
        {
            get { return FolderPath; }
        }

        public string Name
        {
            get { return "OldestFileFromFolderSupplier"; }
        }


        public bool IsBlocking
        {
            get { return false; }
        }


        public void Reply(IMessage reply)
        {
            throw new ConfigurationException("Cannot reply to file system");
        }

        public void Consumed()
        {
            //TODO: this is the appropriate time to delete the file
        }
    }
}
