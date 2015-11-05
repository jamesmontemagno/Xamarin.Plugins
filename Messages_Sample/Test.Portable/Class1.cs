using Refractored.Xam.Messages;

namespace Test.Portable
{
    public static class Class1
    {

        public static object Context { get; set; }

        public static void ShowMessage(string text)
        {
            var dialog = new Messages();
            dialog.ShowMessage(text);
        }

        public static void ShowMessageEx(string title, string message)
        {
            var dialog = new MessagesEx();
            dialog.ShowMessage(title, message, Context);
        }
    }
}
