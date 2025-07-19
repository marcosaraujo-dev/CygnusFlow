namespace CygnusFlow.Application.DTOs.Common
{
    public class ComboBoxItem
    {
        public string Text { get; set; } = string.Empty;
        public object? Value { get; set; }

        public override string ToString() => Text;
    }
}
