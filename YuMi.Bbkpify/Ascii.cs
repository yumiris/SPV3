namespace YuMi.Bbkpify
{
    public static class Ascii
    {
        public static string Banner => @"
    __    __    __         _ ____     
   / /_  / /_  / /______  (_) __/_  __
  / __ \/ __ \/ //_/ __ \/ / /_/ / / /
 / /_/ / /_/ / ,< / /_/ / / __/ /_/ / 
/_.___/_.___/_/|_/ .___/_/_/  \__, /  
                /_/          /____/   
======================================
                           // Yu:YuMi
";

        public static string Progress(int index, int arrayLength)
        {
            return $"[{index + 1}/{arrayLength}]";
        }
    }
}