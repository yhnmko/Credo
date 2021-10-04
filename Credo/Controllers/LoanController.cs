using Credo.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Credo.Service;
using Serilog;

namespace Credo.Controllers
{
    public class LoanController : Controller
    {
        private readonly AppDBContext _db;
        private readonly ILoanService _loanService;
        private readonly UserManager<IdentityUser> _userManager;

        //Initialize DBContext's
        public LoanController(AppDBContext db, UserManager<IdentityUser> userManager, ILoanService loanService)
        {
            _db = db;
            _userManager = userManager;
            _loanService = loanService;
        }

        #region UI
        public async Task<IActionResult> Loan()
        {
            var user =  await _userManager.GetUserAsync(User);
            var oldLoan = _db.UserLoan.Where(a => a.Email == user.Email).AsNoTracking();

            if(oldLoan.Count() == 1)
            {
                LoanViewModel Load = new LoanViewModel(oldLoan.FirstOrDefault().LoanType.ToString(), oldLoan.FirstOrDefault().Amount, oldLoan.FirstOrDefault().Currency, oldLoan.FirstOrDefault().Period, oldLoan.FirstOrDefault().Email, oldLoan.FirstOrDefault().Status);
                return View(Load);
            }

            return View();
        }

        public IEnumerable<UserLoan> Loans { get; set; }

        [HttpPost]
        public async Task<IActionResult> Loan(LoanViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserLoan newLoan = new UserLoan();
                var user = await _userManager.GetUserAsync(User);

                newLoan.Amount = model.amount;
                newLoan.Currency = model.Currency;
                newLoan.Email = user.Email;
                newLoan.LoanType = Int32.Parse(model.LoanType);
                newLoan.Period = model.Period;
                newLoan.Status = "Submitted";
                var oldLoan = _db.UserLoan.Where(a => a.Email == user.Email).AsNoTracking();

                //TODO move Status as Enum
                if (oldLoan.Count() == 0)
                {
                    _db.Add(newLoan);
                }
                else if (!oldLoan.FirstOrDefault().Status.Equals("Denied") && !oldLoan.FirstOrDefault().Status.Equals("Confirmed"))
                    _db.Update(newLoan);
                else
                    ModelState.AddModelError(string.Empty, string.Format("Your loan is in {0} status", oldLoan.FirstOrDefault().Status) );

                _db.SaveChanges();
            }
            return View(model);
        }
        #endregion

        #region REST
        //REST Services
        [HttpGet]
        [Route("[action]")]
        [Route("api/Loan/GetLoans")]
        public IEnumerable<UserLoan> GetLoans()
        {
            try
            {
                return _loanService.GetLoans();
            }
            catch
            {
                Log.Error("Custom Error msg");
                //return empty list
                return new List<UserLoan>();

            }
        }

        [HttpPost]
        [Route("[action]")]
        [Route("api/Loan/AddLoan")]
        public IActionResult AddLoan([FromBody] UserLoan loan)
        {
            try
            {
                Log.Information("adding new loan");
                _loanService.AddLoan(loan);
                Log.Information("loan succesfully added");
                return Ok(loan);
            }
            catch
            {
                Log.Error("Error Happend adding Loan");
                //return empty list
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("[action]")]
        [Route("api/Loan/UpdateLoan")]
        public IActionResult UpdateLoan([FromBody] UserLoan loan)
        {
            _loanService.UpdateLoan(loan);
            return Ok();
        }

        //ToDo
        [HttpDelete]
        [Route("[action]")]
        [Route("api/Loan/DeleteLoan")]
        public IActionResult DeleteLoan([FromBody] string email)
        {
            var existingLoan = _loanService.GetLoan(email);
            if (existingLoan != null)
            {
                _loanService.DeleteLoan(email);
                return Ok();
            }
            return NotFound($"Loan Not Found with Email : {email}");
        }

        [HttpGet]
        [Route("GetLoan")]
        public UserLoan GetEmployee([FromBody] string email)
        {
            return _loanService.GetLoan(email);
        }
        #endregion
    }
}
