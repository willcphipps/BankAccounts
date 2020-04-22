using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models {
    public class Login {
        [EmailAddress]
        [Display (Name = "Email : ")]
        [Required(ErrorMessage="Must enter email")]
        public string LoginEmail { get; set; }
        [Display(Name="Password : ")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Must enter a password")]
        public string LoginPassword { get; set; }
    }
}