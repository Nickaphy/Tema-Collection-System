using FluentResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WatchWorld.Application.Ports.OutBound;
using WatchWorld.Domain.Entities;
using WatchWorld.Infrastructure.Database;

namespace WatchWorld.Infrastructure.Adapters
{
    public class SqlServerHighResImageRepository : IImageRepository
    {
        private readonly AppDbContext _context;

        public SqlServerHighResImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<HighResImage?>>> GetAllAsync(CancellationToken ct = default)
        {
            var images = await _context.HighResImages
                .ToListAsync(ct);
            return Result.Ok(images.AsEnumerable());
        }

        public async Task<Result<HighResImage?>> GetImageByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return Result.Fail("Kan ikke finde et billede da intet billede er blevet valgt");
            var image = await _context.HighResImages
                .FirstOrDefaultAsync(w => w.Id == id, ct);

            return Result.Ok(image);
        }


        public async Task<Result<HighResImage>> CreateImageAsync(HighResImage image, CancellationToken ct = default)
        {
            await _context.HighResImages.AddAsync(image, ct);
            await _context.SaveChangesAsync(ct);
            return Result.Ok(image);
        }

        public async Task<Result> DeleteImageAsync(Guid id, CancellationToken ct = default)
        {
            var image = await _context.HighResImages.FindAsync(new object[] { id }, ct);
            if (image == null)
            {
                return Result.Fail($"Billedet kunne ikke findes.");
            }
            _context.HighResImages.Remove(image);
            await _context.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}
