namespace MagFlow.DAL
{
    public class Class1
    {
        public string Message
        {
            get;
            set => field = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
