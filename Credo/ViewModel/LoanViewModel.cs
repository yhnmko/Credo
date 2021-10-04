using Credo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Credo.ViewModel
{
    public class LoanViewModel
    {
        public LoanViewModel()
        {
            LoanType = "";
            amount = 6;
            Currency = "";
            Period = 7;
            Email = "";
            Status = "";
        }
        public LoanViewModel(string _loanType, int _amount, string _currency, Int32 _period, string _Email, string _status)
        {
            LoanType = _loanType;
            amount = _amount;
            Currency = _currency;
            Period = _period;
            Email = _Email;
            Status = _status;
        }

        [Required]
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; }
        
        [Required]
        [Display(Name = "Amount")]
        
        public int amount { get; set; }
        [Required]
        [Display(Name = "Currency")]
        public string Currency { get; set; }

        [Required]
        [Display(Name = "Period in Months")]
        public Int32 Period { get; set; }

        public string Email { get; set; }
        private String Status { get; set; }
    }
}
