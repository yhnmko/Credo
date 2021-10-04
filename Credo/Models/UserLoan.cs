using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Credo.Models
{
    public class UserLoan
    {
        [Key]
        public string Email { get; set; }
        public Int32 LoanType { get; set; }
        public Int32 Amount { get; set; }
        public string Currency { get; set; }
        public Int32 Period { get; set; }
        public string Status { get; set; }

    }
}
