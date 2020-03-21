using System;
using System.Collections.Generic;

namespace Registrar
{
    /// <summary>
    /// Interface which defines the behavior of a validator to be used with an option's value.
    /// Validate(object value) should take the value and run it through some checks. Returning
    /// true will make the check pass, false will fail it.
    /// Description should be a description of the criteria which makes it fail or pass.
    /// </summary>
    public interface IValidator
    {
        bool Validate(object value);
        string Description();
    }

    /// <summary>
    /// A class which when instantiated, is to contain information concerning the result of
    /// the validation.
    /// If the option successfully was validated, Successful will be true, otherwise it will
    /// be false.
    /// Information will contain information on why the check failed.
    /// </summary>
    internal class ValidationResponse
    {
        public bool Successful { get; set; }
        public string Information { get; set; }
    }

    /// <summary>
    /// A helper class of converters to be used when writing implementations of the IValidator interface.
    /// Returns the converted value associated with the method name, or throws an exception of type RegConversionException
    /// if it failed to be converted.
    /// </summary>
    public static class ValidatorConverters
    {
        public static int ValidatorIntConverter(Object value)
        {
            bool conversionSuccessful = int.TryParse(value.ToString(), out int convertedValue);

            if (!conversionSuccessful)
            {
                throw new RegConversionException("Failed to convert the passed value to an int.");
            }

            return convertedValue;
        }

        public static float ValidatorFloatConverter(Object value)
        {
            bool conversionSuccessful = float.TryParse(value.ToString(), out float convertedValue);

            if (!conversionSuccessful)
            {
                throw new RegConversionException("Failed to convert the passed value to a float.");
            }

            return convertedValue;
        }

        public static string ValidatorStringConverter(Object value)
        {
            return value.ToString();
        }
    }
}
