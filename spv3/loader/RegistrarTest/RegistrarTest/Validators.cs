using System.IO;

namespace RegistrarTest
{
    /*
     * Define implementations of the validator interface.
     * Description is used in the exception which is raised if the option fails
     * to validate.
     * Validate() is the actual validation, where it returns a boolean, true if
     * the option is valid, false if it's not.
     */
    class Validators
    {
        class TestOptionOneValidator : Registrar.IValidator
        {
            public string Description()
            {
                return "Option is an int which can not be greater than 1.";
            }

            public bool Validate(object value)
            {
                int convertedValue = Registrar.ValidatorConverters.ValidatorIntConverter(value);
                return convertedValue <= 1;
            }
        }

        class TestOptionTwoValidator : Registrar.IValidator
        {
            public string Description()
            {
                return "Option is a string which must be set to 'hello!'.";
            }

            public bool Validate(object value)
            {
                string convertedValue = Registrar.ValidatorConverters.ValidatorStringConverter(value);
                return convertedValue == "hello!";
            }
        }

        class TestOptionThreeValidator : Registrar.IValidator
        {
            public string Description()
            {
                return "Option is a bool. It must be 1/0/true/false in the registry.";
            }

            public bool Validate(object value)
            {
                return Registrar.ValidatorConverters.ValidatorBooleanConverter(value);
            }
        }
        
        class TestOptionFourValidator : Registrar.IValidator
        {
            public string Description()
            {
                return "Option is a directory path. The path must exist.";
            }

            public bool Validate(object value)
            {
                string convertedValue = Registrar.ValidatorConverters.ValidatorStringConverter(value);
                return Directory.Exists(convertedValue);
            }
        }
        
        // Implement the validators so they can be used.
        public Registrar.IValidator OptionOneValidator = new TestOptionOneValidator();
        public Registrar.IValidator OptionTwoValidator = new TestOptionTwoValidator();
        public Registrar.IValidator OptionThreeValidator = new TestOptionThreeValidator();
        public Registrar.IValidator OptionFourValidator = new TestOptionFourValidator();
    }
}
