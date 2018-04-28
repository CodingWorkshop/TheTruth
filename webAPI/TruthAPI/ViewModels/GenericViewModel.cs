using System.Collections.Generic;

namespace TruthAPI.ViewModels
{
    public class GenericViewModel<T>
    {
        public T Content { get; set; }
        
        public string Message { get; set; }

        public static GenericViewModel<T> New(T content = default(T), string message = default(string))
        {
            return new GenericViewModel<T>(content, message);
        }

        public GenericViewModel(T content, string message)
        {
            Content = content;
            Message = message;
        }

        public GenericViewModel<T> SetContent(T content)
        {
            Content = content;
            return this;
        }

        public GenericViewModel<T> SetMessage(string message)
        {
            Message = message;
            return this;
        }
    }
}