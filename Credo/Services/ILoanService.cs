using Credo.Models;
using System.Collections.Generic;

namespace Credo.Service
{
    public interface ILoanService
    {

        UserLoan AddLoan(UserLoan employee);

        List<UserLoan> GetLoans();

        void UpdateLoan(UserLoan employee);

        void DeleteLoan(string email);

        UserLoan GetLoan(string email);
    }
}