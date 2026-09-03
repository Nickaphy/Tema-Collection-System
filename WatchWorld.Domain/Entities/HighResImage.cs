using System;
using System.Collections.Generic;
using System.Text;

namespace WatchWorld.Domain.Entities
{
    public class HighResImage
    {
        public Guid HighResId { get; private set; }
        public string Name { get; private set; }
        public string ContentType { get; private set; }
        public long FileSizeBytes { get; private set; }
        public DateTime UploadedAt { get; private set; }
    }
}
