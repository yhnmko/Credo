using Credo.Models;
using System.Collections.Generic;
using System.Linq;

namespace Credo.Service
{
    public class UserLoanService : ILoanService
    {
        private readonly AppDBContext _loanDbContext;
        public UserLoanService(AppDBContext loanDbContext)
        {
            _loanDbContext = loanDbContext;
        }
        public UserLoan AddLoan(UserLoan employee)
        {
            _loanDbContext.UserLoan.Add(employee);
            _loanDbContext.SaveChanges();
            return employee;
        }
        public List<UserLoan> GetLoans()
        {
            return _loanDbContext.UserLoan.ToList();
        }

        public void UpdateLoan(UserLoan employee)
        {
            _loanDbContext.UserLoan.Update(employee);
            _loanDbContext.SaveChanges();
        }

        public void DeleteLoan(string email)
        {
            var employee = _loanDbContext.UserLoan.FirstOrDefault(x => x.Email == email);
            if (employee != null)
            {
                _loanDbContext.Remove(employee);
                _loanDbContext.SaveChanges();
            }
        }

        public UserLoan GetLoan(string email)
        {
            return _loanDbContext.UserLoan.FirstOrDefault(x => x.Email == email);
        }

    }
}