## Registrar - A Registry Handling Configuration thing
I made this because I was tired of writing things like [this](https://pastebin.com/m8vY9vwb). I believe everything is working properly,
as I am currently using this in [Halo CE Mouse Tool](https://github.com/AWilliams17/Halo-CE-Mouse-Tool), [Jollypop Injector](https://github.com/AWilliams17/Jollypop-Injector), and [Starbound Asset Ripper](https://github.com/AWilliams17/Starbound-Asset-Ripper) to great success, so
it should be fine.  
  
If it isn't please do feel free to open an [issue](https://github.com/AWilliams17/Registrar), I would greatly appreciate it.
  
## Usage
### Setting it all up
First, create a RegSettings instance. This will hold and handle all the settings. Then, register some settings. They automatically will be put into the registry when calling the SaveSettings() method of the settings object.  

```csharp
/*
	Creates a Registry key under HKEY_CURRENT_USER\Software called 'Test',
	and also creates three keys under Test.
	Calling SaveSettings() pushes the current values to the Registry.
*/
RegSettings settings = new RegSettings(Registrar.RegBaseKeys.HKEY_CURRENT_USER, "Software/Test");

// Instantiate some RegOption objects
RegOption optionOne = new RegOption("option_one", 1, typeof(int));
RegOption optionTwo = new RegOption("option_two", "hello!", typeof(string));
RegOption optionThree = new RegOption("option_three", true, typeof(bool));

// Register the settings - this puts them in the settings object's internal dictionary.
settings.RegisterSetting("OptionOne", optionOne);
settings.RegisterSetting("OptionTwo", optionTwo);
settings.RegisterSetting("OptionThree", optionThree);

settings.SaveSettings(); // Will throw RegSaveException if for whatever reason it fails, with information as to which values failed and why.
```

Calling LoadSettings() will (attempt to) load the registered keys from the registry and set the appropriate values in the settings object's internal dictionary, and also will throw an exception of type RegLoadException if it fails, with information on what values failed to load and why.  
  
So when saving or loading exceptions, you only need to try to catch those.  
  
### Retrieving/Setting options  
To retrieve options:  
```csharp
settings.GetOption<int>("OptionOne")
settings.GetOption<string>("OptionTwo")
settings.GetOption<bool>("OptionThree")
```
To set options:  
```csharp
settings.SetOption("OptionOne", 1);
settings.SetOption("OptionTwo", "Bye!!!");
settings.SetOption("OptionThree", false);
```  
If a set option call fails, then an exception of type RegOptionAssignmentException will be thrown.  
If a get option call fails, then an exception of type RegOptionRetrievalException will be thrown.  
When loading/saving settings, both of these exceptions are handled internally, and their message is added to the RegLoadException/RegSaveException messages.  
  
**NOTE: Make sure you call SaveSettings() to push the saved options to the registry.**

### Validation  
SetOption, LoadSettings, and SaveSettings will automatically run the options they are working with through a validator interface you implement. A validator is optional.  
Here is how you do it:  
```csharp
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
```
And in order to use the validator with an option, just pass it during instantian of the option object:
```csharp
Registrar.RegOption optionOne = new Registrar.RegOption("option_one", validators.OptionOneValidator, 1, typeof(int));
```
Here is it in action:
```csharp
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
```
If it fails to validate during loading and saving, the RegOptionAssignmentException is handled internally and its message is put into the RegLoadException/RegSaveException message.  

#### Validator Converters
Registrar contains (at this moment, three) built-in helper classes for converting to specific value types. These are intended for use in user-implementations of the IValidator interface.  
```csharp
int convertedValue = Registrar.ValidatorConverters.ValidatorIntConverter(value);
```
If the conversion here fails, then an exception of type RegConversionException will occur with information as to why it failed. This exception is caught internally, and its result is appended to any exception message (EG: RegLoadException, RegSaveException, RegOptionAssignmentException)  which is generated with LoadSettings, SaveSettings, and SetOption.  

A basic converter class is as follows:  
```csharp
public static int ValidatorIntConverter(Object value)
{
	bool conversionSuccessful = int.TryParse(value.ToString(), out int convertedValue);
	
	if (!conversionSuccessful)
	{
		throw new RegConversionException("Failed to convert the passed value to an int.");
	}

	return convertedValue;
}
```
Make sure when making your own converter you follow this general template. In the future I plan on making this an interface for more ease of use.  
### Subkeys
Registrar can do subkeys.  
EG: To make the root key HKEY_CURRENT_USER\Software\Subkey\Test:
```csharp
Registrar.RegSettings settings = new Registrar.RegSettings(Registrar.RegBaseKeys.HKEY_CURRENT_USER, "Software/Subkey/Test");
```
To do HKEY_CURRENT_USER\Software\Subkey\Subkey2\Subkey3\Test:
```csharp
Registrar.RegSettings settings = new Registrar.RegSettings(Registrar.RegBaseKeys.HKEY_CURRENT_USER, "Software/Subkey/Subkey2/Subkey3/Test");
```
It also can do subkeys with options, as in, to put the option_one key into its own subkey(s):
```csharp
Registrar.RegOption optionOne = new Registrar.RegOption("option_one", validators.OptionOneValidator, 1, typeof(int), "Subkey/Subkey2/Subkey3");
```
This will equate to Software\Test\Subkey\Subkey2\Subkey3\option_one  
### Note on Releases  
In the unlikely event someone else is going to use this, note that I am no longer posting updated releases.  
Just the repository is being updated. If you want to use it, just compile it.
