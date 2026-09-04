using System;
using System.Collections.Generic;
using System.Text;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Entities
{
    public class HighResImage : Aggregateroot
    {
	public Guid Id { get; set; }
	public required string Url { get; set; }
	public int Width { get; set; }
	public int Height { get; set; }

        private HighResImage() { }
    }
}
