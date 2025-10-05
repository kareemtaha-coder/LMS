using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Abstractions.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string subfolder, CancellationToken cancellationToken = default);
        void DeleteFile(string? relativePath);
    }
}
