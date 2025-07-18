using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSizeInBytes;

    public MaxFileSizeAttribute(int maxFileSizeInBytes)
    {
        _maxFileSizeInBytes = maxFileSizeInBytes;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file && file.Length > _maxFileSizeInBytes)
        {
            var maxSizeInMB = _maxFileSizeInBytes / 1024 / 1024;
            return new ValidationResult($"Maximum allowed file size is {maxSizeInMB} MB.");
        }

        return ValidationResult.Success;
    }
}
