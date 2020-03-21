using System;

namespace RegistrarTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the validators object, which contains implementations of the validator interface
            // to be used for validating options when they are set.
            Validators validators = new Validators();
            
            // This will create a key in the BaseKey 'HKEY_CURRENT_USER' called 'TestOptionsRootKey', which is where all
            // the registry entries for this particular instance will reside.
            Registrar.RegSettings settings = new Registrar.RegSettings(Registrar.RegBaseKeys.HKEY_CURRENT_USER, "Software/Test");
            
            /*
             * Option instances.
             * The first parameter is the keyname to be used in the registry entry for the option, the second is
             * the validator instace to be used for the option to run against, the third is the default value, and
             * the forth is the type of variable the option is to use. Optionally, the fifth is subkeys,
             * which will put the key into HKEY_CURRENT_USER\TestOptionsRootKey\Subkey etc.
             */
            Registrar.RegOption optionOne = new Registrar.RegOption("option_one", validators.OptionOneValidator, 1, typeof(int));
            Registrar.RegOption optionTwo = new Registrar.RegOption("option_two", validators.OptionTwoValidator, "hello!", typeof(string));
            Registrar.RegOption optionThree = new Registrar.RegOption("option_three", validators.OptionThreeValidator, true, typeof(bool), "/SubKey");
            Registrar.RegOption optionFour = new Registrar.RegOption("option_four", validators.OptionFourValidator, @"C:\", typeof(string), "/SubKey/AnotherSubkey");
            Registrar.RegOption optionFive = new Registrar.RegOption("option_five", null, "Just a string.", typeof(string));

            // Register the options. This puts them in the settings objects internal dictionary of option objects.
            // The first parameter is the name to use for the option when retrieving it with GetSetting. The second
            // is the option instance.
            // The name of the option can be different from the key used in the registry.
            settings.RegisterSetting("OptionOne", optionOne);
            settings.RegisterSetting("OptionTwo", optionTwo);
            settings.RegisterSetting("OptionThree", optionThree);
            settings.RegisterSetting("OptionFour", optionFour);
            settings.RegisterSetting("OptionFive", optionFive);
            
            /*
             * If the root key does not exist in the registry, call save settings to create one with current values.
             * Otherwise, load settings.
            */

            if (!settings.RootKeyExists())
            {
                Console.WriteLine("Didn't find the root key in the registry. \r\nPopulating one with current values.");
                try
                {
                    settings.SaveSettings();
                }
                catch (Registrar.RegSaveException ex)
                {
                    Console.WriteLine($"Error(s) occurred while saving settings: \r\n{ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Registry entry for root key was found. \r\nLoading settings.");
                try
                {
                    settings.LoadSettings();
                }
                catch (Registrar.RegLoadException ex)
                {
                    Console.WriteLine($"Error(s) occurred while loading settings: \r\n{ex.Message}");
                }
            }
            
            // GetOption and SetOption examples
            Console.WriteLine(settings.GetOption<int>("OptionOne"));
            Console.WriteLine(settings.GetOption<string>("OptionTwo"));
            Console.WriteLine(settings.GetOption<bool>("OptionThree"));
            Console.WriteLine(settings.GetOption<string>("OptionFour"));
            Console.WriteLine(settings.GetOption<string>("OptionFive"));

            Console.WriteLine("Setting OptionOne to 2 (this will fail since the validator makes sure its <= 1)");

            try
            {
                settings.SetOption("OptionOne", 2);
                Console.WriteLine("Successfully set OptionOne to 2"); // This will never be reached but its here just for show.
            }
            catch (Registrar.RegOptionAssignmentException ex)
            {
                Console.WriteLine($"Error occurred while setting OptionOne to 2: {ex.Message}");
            }

            Console.WriteLine(settings.GetOption<int>("OptionOne")); // Prints 1, since the option failed to be set so it kept its previous value

            // Keep the window open
            Console.Read();
        }
    }
}
