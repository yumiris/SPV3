using System;

namespace Registrar
{
    /// <summary>
    /// Represents an option to be validated, stored in the registry, and stored in the settings dictionary.
    /// </summary>
    public class RegOption
    {
        private string _keyName = null;
        private string _subKeys = null;
        private IValidator _validator = null;
        private object _optionValue = null;
        private Type _optionType = null;

        /// <summary>
        /// The constructor for the option.
        /// </summary>
        /// <param name="key_name">The name of the key to use in the registry for the option.</param>
        /// <param name="validator">An implementation of the IValidator interface which will be used to validate the option.</param>
        /// <param name="value">On instantiation, this is the default value of the option. Otherwise, it's the recently set value.</param>
        /// <param name="value_type">The type of the value. This is used when loading options from the registry so they are converted to the right type.</param>
        /// <param name="sub_keys">Subkeys to put the option in, if any. EG: /Subkey1 puts it in a key 'Subkey1' under the root key. Can have multiple subkeys: /Subkey1/Subkey2, etc.</param>
        public RegOption(string key_name, IValidator validator, Object value, Type value_type, string sub_keys = null)
        {
            _keyName = key_name;

            string keyOut;
            if (sub_keys != null)
            {
                if (sub_keys[0] != '/')
                {
                    sub_keys = '/' + sub_keys;

                }
                keyOut = sub_keys.Replace(@"/", @"\\");
                _subKeys = keyOut;
            }

            _validator = validator;
            _optionValue = value;
            _optionType = value_type;
        }
        /// <summary>
        /// If the option has a validator interface implementation, then this will either:
        /// A: Run the validator with the current option value
        /// B: Run the validator with a value passed.
        /// If there is no validator, the ValidationResponse is always successful.
        /// </summary>
        /// <param name="value">If passed, the value to be validated.</param>
        /// <returns>Returns a ValidationResponse instance which contains the result of the validation.</returns>
        internal ValidationResponse Validate(Object value = null)
        {
            if (value == null)
            {
                value = _optionValue;
            }

            ValidationResponse validationResult = new ValidationResponse
            {
                Successful = true,
                Information = "Successfully processed option."
            };

            if (_validator != null)
            {
                try
                {
                    bool optionValid = _validator.Validate(value);
                    if (!optionValid)
                    {
                        validationResult.Successful = false;
                        validationResult.Information = _validator.Description();
                    }
                }
                catch (RegConversionException ex)
                {
                    validationResult.Successful = false;
                    validationResult.Information = ex.Message;
                }
            }

            return validationResult;
        }

        /// <summary>
        /// Retrieve the current value of the option.
        /// </summary>
        public Object OptionValue
        {
            get { return _optionValue; }
        }

        /// <summary>
        /// Attempt to set the current value of the option to the passed one.
        /// </summary>
        /// <param name="value">The value to be attempted.</param>
        /// <returns>A ValidationResponse instance which contains the result of the validation.</returns>
        internal ValidationResponse SetOptionValue(Object value)
        {
            ValidationResponse validationResponse = Validate(value);

            if (validationResponse.Successful)
            {
                _optionValue = Convert.ChangeType(value.ToString(), _optionType);
            }

            return validationResponse;
        }

        /// <summary>
        /// Gets the subkeys the option uses.
        /// </summary>
        /// <returns>The subkeys string.</returns>
        public string GetSubKeys()
        {
            return _subKeys;
        }

        /// <summary>
        /// Gets the key name the option will be using in the registry.
        /// </summary>
        /// <returns>The keyname string.</returns>
        public string GetKeyName()
        {
            return _keyName;
        }
    }
}
