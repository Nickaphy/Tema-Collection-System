using System;
using System.Collections.Generic;
using System.Text;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Entities
{
    public class HighResImage : Aggregateroot
    {
        public string Name { get; private set; }
        public string ContentType { get; private set; }
        public long FileSizeBytes { get; private set; }
        public DateTime UploadedAt { get; private set; }


        private HighResImage() { }
    }
}
