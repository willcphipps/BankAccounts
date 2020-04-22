using System;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace BankAccounts.Models {
    public class BankAccount {
        [Key]
        public int AccountId { get; set; }

        // [DataType (DataType.Currency)]
        [Display (Name = "Deposit/WithDraw : ")]
        public int Ballance { get; set; }
        public int UserId { get; set; }
        public User Client { get; set; }

    }
}