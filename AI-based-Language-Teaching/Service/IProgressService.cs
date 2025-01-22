using AI_based_Language_Teaching.Models;
using System;

namespace AI_based_Language_Teaching.Service
{
    public interface IProgressService
    {
        Task<Progress> GetProgressByUserIdAsync(string userId);
        Task UpdateProgressAsync(Progress progress);
    }
}
