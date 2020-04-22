using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BankAccounts.Models {
    public class User {
        [Key]
        public int UserId { get; set; }

        [EmailAddress (ErrorMessage = "Must be valid Email Address")]
        [Required (ErrorMessage = "Must Enter Email")]
        [Display(Name="Email Address : ")]

        public string Email { get; set; }

        [DataType (DataType.Password)]
        [Required]
        [MinLength (8, ErrorMessage = "Password must be 8 characters or longer!")]
        [Display(Name="Password : ")]
        public string Password { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name="Confirm Password : ")]
        public string ConfirmPassword {get;set;}
        public int AccountId { get; set; }
        public List<BankAccount> AccountsActive { get; set; }
    }
}