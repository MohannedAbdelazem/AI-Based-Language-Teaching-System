using AI_based_Language_Teaching.Models;
using System;

namespace AI_based_Language_Teaching.Service
{
    public interface IProgressService
    {
        Progress GetProgressByUserId(string userId);
        void UpdateProgress(Progress progress);
    }
}