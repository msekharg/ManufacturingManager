﻿using System.Data;

namespace ManufacturingManager.Core.Repositories
{
    public interface IUsersRepository
    {
        User Users(int userId);
        User Users(string loginName);
        Task<IEnumerable<User>> GetUserList(bool onlyActive);
        // Task<string> GetEmailsForAdministratorsAndCoordinators();
        Task<bool> Add(User user, int createdBy);
        Task<bool> Update(User user, int updatedBy);
        Task<bool> UpdateLoginDate(int userId);
        void AssignValues(User r, DataRow dr);
    }
}
