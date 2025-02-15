﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace panel.Models
{
    public class User : IEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string ResetPassword { get; set; }
        public string NewPassword { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
        public string Salt { get; set; }
    }
}
