﻿using Informagator.Contracts;
using Informagator.Contracts.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxCustom
{
    public class AlternatingFolderTransform : ITransformStage
    {
        private bool sendToOneOrTwo = false;
        public string Name
        {
            get { return "Alternating Folder Consumer"; }
        }

        public IEnumerable<IMessage> TransformMessage(IMessage message)
        {
            string folderName = sendToOneOrTwo ? @"C:\Demo\Out\" : @"C:\Demo\Out2";
            sendToOneOrTwo = !sendToOneOrTwo;
            message.Attributes.Add("FolderName", folderName);
            return new[] { message };
        }


        public void ValidateSettings()
        {
        }
    }
}
