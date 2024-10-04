﻿using System.ComponentModel.DataAnnotations;

namespace MovieBookingAuthApi.Models
{
    public class ForgotPassword
    {
        [Required]
        public string LoginId { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;

        [Required, Compare(nameof(NewPassword), ErrorMessage = "New Password and Confirm New Password are not matching.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
