using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amedisys.Interop.Btk;
using Amedisys.Interop.Btk.Stages;

namespace DemoInterface
{
    public class FolderToFolderSequence : ProcessingSequence
    {
        public FolderToFolderSequence()
        {
            var errorHandler = new DevNullErrorHandler();

            var source = new OldestFileFromFolderSource(errorHandler);
            source.FolderPath = @"C:\Demo\Source";

            var destination = new ConsumeToOutputFolder(errorHandler);
            destination.FolderPath = @"C:\Demo\Destination";

            Stages = new ProcessingStage[] { source, destination};
        }
    }
}
