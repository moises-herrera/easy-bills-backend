﻿namespace EasyBills.Domain.User;

public record CreateUserDTO(
    string FirstName,
    string LastName,
    string Email,
    string Password);
